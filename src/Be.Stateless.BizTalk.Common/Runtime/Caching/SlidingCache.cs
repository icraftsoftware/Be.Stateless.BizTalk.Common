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
using System.Runtime.Caching;

namespace Be.Stateless.BizTalk.Runtime.Caching
{
	/// <summary>
	/// Simple runtime memory cache base class with sliding expiration <seealso cref="CacheItemPolicy"/>.
	/// </summary>
	/// <typeparam name="TKey">
	/// The type of the objects to be used as key.
	/// </typeparam>
	/// <typeparam name="TItem">
	/// The type of the objects to be cached.
	/// </typeparam>
	/// <remarks>
	/// This cache attaches a sliding expiration <seealso cref="CacheItemPolicy"/> to each item it adds to the <see
	/// cref="MemoryCache"/> that is created behind the scene.
	/// </remarks>
	/// <seealso cref="Cache{TKey,TItem}"/>
	/// <seealso cref="MemoryCache"/>
	public abstract class SlidingCache<TKey, TItem> : Cache<TKey, TItem>
	{
		/// <summary>
		/// Create the <see cref="Cache{TKey,TItem}"/>-derived instance with a default sliding expiration of 30 minutes.
		/// </summary>
		protected SlidingCache() : this(TimeSpan.FromMinutes(30)) { }

		/// <summary>
		/// Create the <see cref="Cache{TKey,TItem}"/>-derived instance and overrides the default sliding expiration.
		/// </summary>
		/// <param name="slidingExpiration">
		/// The <see cref="TimeSpan"/> denoting the sliding expiration to apply to newly inserted items in cache.
		/// </param>
		protected SlidingCache(TimeSpan slidingExpiration)
		{
			if (slidingExpiration.TotalMinutes <= 0) throw new ArgumentException("Sliding expiration time span must be greater than 0 minutes", nameof(slidingExpiration));
			_slidingExpiration = slidingExpiration;
		}

		#region Base Class Member Overrides

		/// <summary>
		/// Returns a sliding expiration <see cref="CacheItemPolicy"/> according to the <see cref="TimeSpan"/> provided at
		/// construction time of this <see cref="SlidingCache{TKey,TItem}"/> object.
		/// </summary>
		/// <remarks>
		/// The sliding expiration defaults to 30 minutes, unless specified otherwise via the <see
		/// cref="SlidingCache{TKey,TItem}(TimeSpan)"/> constructor.
		/// </remarks>
		/// <seealso cref="SlidingCache{TKey,TItem}"/>
		/// <seealso cref="SlidingCache{TKey,TItem}(TimeSpan)"/>
		protected override CacheItemPolicy CacheItemPolicy => new CacheItemPolicy { SlidingExpiration = _slidingExpiration };

		#endregion

		private readonly TimeSpan _slidingExpiration;
	}
}
