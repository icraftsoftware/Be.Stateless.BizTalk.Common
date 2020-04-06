#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Runtime.Caching
{
	public class MemoryCacheAssumptionFixture
	{
		[Fact]
		public void AccessingItemRenewsItWhenUsingSlidingExpiration()
		{
			// Note: the real MemoryCache from System.runtime.caching does not update sliding expiration
			// data for expiration that is below 1 second, therefore we use longer timespans in this test
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(2000d) });

			Thread.Sleep(1000);
			_memoryCache["test"].Should().NotBeNull(); // should renew for another 2000 ms
			Thread.Sleep(1500);

			_memoryCache["test"].Should().BeSameAs(value);
		}

		[Fact]
		public void AddReturnsFalseWhenKeyIsAlreadyInCache()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy());

			_memoryCache.Add(new CacheItem("test", new object()), new CacheItemPolicy()).Should().BeFalse();
		}

		[Fact]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
		public void ConstructorThrowsWhenNameIsNull()
		{
			Action act = () => new MemoryCache(null);
			act.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public void ContainsReturnsFalseWithExistingSlidingExpiredKey()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(50d) });
			Thread.Sleep(60);

			_memoryCache.Contains("test").Should().BeFalse();
		}

		[Fact]
		public void ContainsReturnsFalseWithNonexistentKey()
		{
			_memoryCache.Contains("stuff").Should().BeFalse();
		}

		[Fact]
		public void ContainsReturnsTrueWithExistingUnexpiredKey()
		{
			_memoryCache.Add(new CacheItem("test", new object()), new CacheItemPolicy());

			_memoryCache.Contains("test").Should().BeTrue();
		}

		[Fact]
		public void ContainsReturnsTrueWithExistingUnexpiredKeyWhenUsingSlidingExpiration()
		{
			_memoryCache.Add(new CacheItem("test", new object()), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(50d) });

			_memoryCache.Contains("test").Should().BeTrue();
		}

		[Fact]
		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		public void ContainsThrowsWhenKeyIsNull()
		{
			Action act = () => _memoryCache.Contains(null);
			act.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public void IndexerReturnsItemWithExistingUnexpiredKey()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy());

			_memoryCache["test"].Should().BeSameAs(value);
		}

		[Fact]
		public void IndexerReturnsItemWithExistingUnexpiredKeyWhenUsingAbsoluteExpiration()
		{
			var value = new object();

			_memoryCache.Add(
				new CacheItem("test", value),
				new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow + TimeSpan.FromMilliseconds(50d)) });

			_memoryCache["test"].Should().BeSameAs(value);
		}

		[Fact]
		public void IndexerReturnsNullWithExistingSlidingExpiredKey()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMilliseconds(50d) });
			Thread.Sleep(60);

			_memoryCache["test"].Should().BeNull();
		}

		[Fact]
		public void ItemIsNotRenewedWhenUsingAbsoluteExpiration()
		{
			var value = new object();

			_memoryCache.Add(
				new CacheItem("test", value),
				new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow + TimeSpan.FromMilliseconds(50d)) });
			Thread.Sleep(30);
			var unused = _memoryCache["test"];
			Thread.Sleep(30);

			_memoryCache["test"].Should().BeNull();
		}

		[Fact]
		public void RemoveMakesItemNoLongerAccessible()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy());

			_memoryCache.Remove("test");

			_memoryCache["test"].Should().BeNull();
		}

		[Fact]
		public void RemoveReturnsRemovedItem()
		{
			var value = new object();

			_memoryCache.Add(new CacheItem("test", value), new CacheItemPolicy());

			_memoryCache.Remove("test").Should().BeSameAs(value);
		}

		public MemoryCacheAssumptionFixture()
		{
			_memoryCache = new MemoryCache("test");
		}

		private readonly MemoryCache _memoryCache;
	}
}
