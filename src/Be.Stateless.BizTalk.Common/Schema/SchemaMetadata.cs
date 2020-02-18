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
using Be.Stateless.BizTalk.Extensions;
using Be.Stateless.BizTalk.Runtime.Caching;
using Be.Stateless.Extensions;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Schema
{
	/// <summary>
	/// Gives access to metadata associated to any <see cref="SchemaBase"/>-derived <see cref="Type"/>, ranging from information
	/// such as <see cref="ISchemaMetadata.BodyXPath"/>, <see cref="ISchemaMetadata.MessageType"/>, <see
	/// cref="ISchemaMetadata.TargetNamespace"/>, to annotations embedded in the XML schema definition.
	/// </summary>
	public static class SchemaMetadata
	{
		#region Nested Type: RootedSchemaMetadata

		private class RootedSchemaMetadata : ISchemaMetadata
		{
			internal RootedSchemaMetadata(Type type)
			{
				Type = type ?? throw new ArgumentNullException(nameof(type));
				_annotations = new Lazy<ISchemaAnnotations>(() => SchemaAnnotations.Create(this));
			}

			#region ISchemaMetadata Members

			public ISchemaAnnotations Annotations => _annotations.Value;

			public string BodyXPath => Microsoft.XLANGs.RuntimeTypes.SchemaMetadata.For(Type).BodyXPath ?? string.Empty;

			public DocumentSpec DocumentSpec => new DocumentSpec(Type.FullName, Type.Assembly.FullName);

			public bool IsEnvelopeSchema => !BodyXPath.IsNullOrEmpty();

			public string MessageType => Microsoft.XLANGs.RuntimeTypes.SchemaMetadata.For(Type).SchemaName;

			public string RootElementName => Microsoft.XLANGs.RuntimeTypes.SchemaMetadata.For(Type).RootElementName;

			public string TargetNamespace => Microsoft.XLANGs.RuntimeTypes.SchemaMetadata.For(Type).TargetNamespace;

			public Type Type { get; }

			#endregion

			private readonly Lazy<ISchemaAnnotations> _annotations;
		}

		#endregion

		#region Nested Type: RootlessSchemaMetadata

		private class RootlessSchemaMetadata : ISchemaMetadata
		{
			[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
			internal RootlessSchemaMetadata(Type type)
			{
				Type = type ?? throw new ArgumentNullException(nameof(type));
				if (!type.IsDocumentSchema())
					throw new ArgumentException(
						$"{nameof(SchemaMetadata)} only supports schemas qualified with a {nameof(SchemaTypeAttribute)} whose {nameof(SchemaTypeAttribute.Type)} is equal to {nameof(SchemaTypeEnum.Document)}.",
						nameof(type));

				var schemaBase = (SchemaBase) Activator.CreateInstance(type);
				TargetNamespace = schemaBase.Schema.TargetNamespace;
			}

			#region ISchemaMetadata Members

			public ISchemaAnnotations Annotations => SchemaAnnotations.Empty;

			public string BodyXPath => string.Empty;

			public DocumentSpec DocumentSpec => null;

			public bool IsEnvelopeSchema => false;

			public string MessageType => string.Empty;

			public string RootElementName => string.Empty;

			public string TargetNamespace { get; }

			public Type Type { get; }

			#endregion
		}

		#endregion

		internal static ISchemaMetadata Create(Type type)
		{
			return type.IsSchemaRoot()
				? (ISchemaMetadata) new RootedSchemaMetadata(type)
				: new RootlessSchemaMetadata(type);
		}

		public static ISchemaMetadata For<T>() where T : SchemaBase
		{
			return For(typeof(T));
		}

		[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
		public static ISchemaMetadata For(Type type)
		{
			if (!type.IsSchema()) throw new ArgumentException("Type is not a SchemaBase derived Type instance.", nameof(type));
			return SchemaMetadataCache.Instance[type];
		}
	}
}