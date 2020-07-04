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
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.Extensions;

namespace Be.Stateless.BizTalk.XPath.Extensions
{
	public static class SchemaPropertyExtractorAnnotationExtensions
	{
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Mock Injection Hook")]
		[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global", Justification = "Mock Injection Hook")]
		internal static Func<ISchemaAnnotations, PropertyExtractorCollection> SchemaPropertyExtractorCollectionFactory { get; set; } = GetAndReadXmlPropertiesAnnotation;

		public static PropertyExtractorCollection GetExtractors(this ISchemaAnnotations schemaAnnotations)
		{
			if (schemaAnnotations == null) throw new ArgumentNullException(nameof(schemaAnnotations));
			return SchemaPropertyExtractorCollectionFactory(schemaAnnotations);
		}

		private static PropertyExtractorCollection GetAndReadXmlPropertiesAnnotation(this ISchemaAnnotations schemaAnnotations)
		{
			var xElement = schemaAnnotations.GetAnnotation("Properties");
			return xElement.IfNotNull(
					p => {
						var extractorCollection = new PropertyExtractorCollection();
						extractorCollection.ReadXml(p.CreateReader());
						return extractorCollection;
					})
				?? PropertyExtractorCollection.Empty;
		}
	}
}
