﻿#region Copyright & License

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
using System.IO;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Streaming.Extensions
{
	/// <summary>
	/// Provides dependency injection support to <see cref="Stream"/> extension methods through various categories of dedicated
	/// extension interfaces.
	/// </summary>
	/// <remarks>
	/// The purpose of this factory is to make <see cref="Stream"/> extension methods amenable to mocking, <see
	/// href="http://blogs.clariusconsulting.net/kzu/how-to-mock-extension-methods/"/>.
	/// </remarks>
	/// <seealso href="http://blogs.clariusconsulting.net/kzu/how-extension-methods-ruined-unit-testing-and-oop-and-a-way-forward/"/>
	/// <seealso href="http://blogs.clariusconsulting.net/kzu/making-extension-methods-amenable-to-mocking/"/>
	public static class StreamExtensions
	{
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Mock Injection Hook")]
		[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Mock Injection Hook")]
		internal static Func<MarkableForwardOnlyEventingReadStream, IProbeStream> StreamProberFactory { get; set; } = stream => new Prober(stream);

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Mock Injection Hook")]
		[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Mock Injection Hook")]
		internal static Func<Stream[], ITransformStream> StreamTransformerFactory { get; set; } = streams => new Transformer(streams);

		/// <summary>
		/// Ensure the <see cref="Stream"/> is wrapped in a <see cref="MarkableForwardOnlyEventingReadStream"/> and thereby ready
		/// for probing, see <see cref="Probe"/>.
		/// </summary>
		/// <param name="stream">
		/// The current <see cref="Stream"/>.
		/// </param>
		/// <returns>
		/// A <see cref="MarkableForwardOnlyEventingReadStream"/> stream.
		/// </returns>
		public static MarkableForwardOnlyEventingReadStream AsMarkable(this Stream stream)
		{
			return MarkableForwardOnlyEventingReadStream.EnsureMarkable(stream);
		}

		/// <summary>
		/// Ensure the <see cref="Stream"/> is wrapped in a <see cref="MarkableForwardOnlyEventingReadStream"/> and thereby ready
		/// for probing, see <see cref="Probe"/>.
		/// </summary>
		/// <param name="stream">
		/// The current <see cref="Stream"/>.
		/// </param>
		/// <returns>
		/// A <see cref="MarkableForwardOnlyEventingReadStream"/> stream.
		/// </returns>
		/// <exception cref="InvalidCastException">
		/// If <paramref name="stream"/> is not already wrapped in a <see cref="MarkableForwardOnlyEventingReadStream"/>.
		/// </exception>
		public static MarkableForwardOnlyEventingReadStream EnsureMarkable(this Stream stream)
		{
			return (MarkableForwardOnlyEventingReadStream) stream;
		}

		/// <summary>
		/// Support for <see cref="Stream"/> probing.
		/// </summary>
		/// <param name="stream">
		/// The current <see cref="Stream"/>.
		/// </param>
		/// <returns>
		/// The <see cref="IProbeStream"/> instance that will probe the current <see cref="Stream"/>s.
		/// </returns>
		public static IProbeStream Probe(this MarkableForwardOnlyEventingReadStream stream)
		{
			return StreamProberFactory(stream);
		}

		/// <summary>
		/// Support for <see cref="TransformBase"/>-derived transforms directly applied to one <see cref="Stream"/>.
		/// </summary>
		/// <param name="stream">
		/// The current <see cref="Stream"/>.
		/// </param>
		/// <returns>
		/// The <see cref="ITransformStream"/> instance that will apply the transform on the current <see cref="Stream"/>.
		/// </returns>
		public static ITransformStream Transform(this Stream stream)
		{
			return StreamTransformerFactory(new[] { stream });
		}

		/// <summary>
		/// Support for <see cref="TransformBase"/>-derived transforms directly applied to several <see cref="Stream"/>s.
		/// </summary>
		/// <param name="streams">
		/// The current <see cref="Stream"/>s.
		/// </param>
		/// <returns>
		/// The <see cref="ITransformStream"/> instance that will apply the transform on the current <see cref="Stream"/>s.
		/// </returns>
		public static ITransformStream Transform(this Stream[] streams)
		{
			return StreamTransformerFactory(streams);
		}
	}
}
