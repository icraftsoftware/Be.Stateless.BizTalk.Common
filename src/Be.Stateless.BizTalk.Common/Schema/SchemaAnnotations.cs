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
using System.Linq;
using System.Xml.Linq;
using Be.Stateless.BizTalk.Schema.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Schema
{
	/// <summary>
	/// Provides access to annotations embedded in <see cref="SchemaBase"/>-derived schema definitions.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Only annotations declared in the schema annotation namespace defined by BizTalk Factory, i.e.
	/// <c>urn:schemas.stateless.be:biztalk:annotations:2013:01</c> are considered; all other annotations are ignored.
	/// </para>
	/// <para>
	/// Notice that there is no transitive discovery of the annotations across the XSD type definitions and that only annotations
	/// embedded directly underneath the root node of the relevant <see cref="SchemaBase"/>-derived schema are loaded.
	/// </para>
	/// </remarks>
	/// <example>
	/// The following example illustrates how to embed BizTalk Factory annotations. Note that other annotations specific to
	/// Microsoft BizTalk Server might coexist but are not illustrated.
	/// <code>
	/// <![CDATA[
	/// <xs:schema targetNamespace='urn:schemas.stateless.be:biztalk:tests:annotated:2013:01'
	///            xmlns:san='urn:schemas.stateless.be:biztalk:annotations:2013:01'
	///            xmlns:xs='http://www.w3.org/2001/XMLSchema'>
	///   <xs:element name='Root'>
	///     <xs:annotation>
	///       <xs:appinfo>
	///         ...
	///         <san:EnvelopeMapSpecName>
	///           Be.Stateless.BizTalk.Unit.Transform.IdentityTransform, Be.Stateless.BizTalk.Unit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3707daa0b119fc14
	///         </san:EnvelopeMapSpecName>
	///         <san:Properties xmlns:tp='urn:schemas.stateless.be:biztalk:properties:tracking:2012:04'>
	///           <tp:Value1 xpath="/*[local-name()='Root']/*[local-name()='Message']/*[local-name()='Id']" />
	///         </san:Properties>
	///         ...
	///       </xs:appinfo>
	///     </xs:annotation>
	///     <xs:complexType />
	///   </xs:element>
	/// </xs:schema>
	/// ]]>
	/// </code>
	/// </example>
	[SuppressMessage("ReSharper", "CommentTypo")]
	public class SchemaAnnotations : ISchemaAnnotations
	{
		#region Nested Type: EmptySchemaAnnotations

		private sealed class EmptySchemaAnnotations : ISchemaAnnotations
		{
			#region ISchemaAnnotations Members

			public XElement GetAnnotation(string annotationElementName)
			{
				return null;
			}

			#endregion
		}

		#endregion

		public static ISchemaAnnotations Create(ISchemaMetadata schemaMetadata)
		{
			if (schemaMetadata == null) throw new ArgumentNullException(nameof(schemaMetadata));
			if (schemaMetadata.Type.Assembly.FullName.StartsWith("Microsoft.", StringComparison.Ordinal)
				|| !schemaMetadata.HasAnnotations()) return Empty;
			return new SchemaAnnotations(schemaMetadata);
		}

		private SchemaAnnotations(ISchemaMetadata metadata)
		{
			_metadata = metadata;
		}

		#region ISchemaAnnotations Members

		public XElement GetAnnotation(string annotationElementName)
		{
			return _metadata.GetAnnotations().SingleOrDefault(e => e.Name.LocalName == annotationElementName);
		}

		#endregion

		public const string NAMESPACE = "urn:schemas.stateless.be:biztalk:annotations:2013:01";
		public static readonly ISchemaAnnotations Empty = new EmptySchemaAnnotations();
		private readonly ISchemaMetadata _metadata;
	}
}
