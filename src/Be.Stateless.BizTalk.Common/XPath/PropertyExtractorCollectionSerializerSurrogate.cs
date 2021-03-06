#region Copyright & License

// Copyright � 2012 - 2020 Fran�ois Chabot
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
using System.Xml;
using Be.Stateless.Xml.Extensions;

namespace Be.Stateless.BizTalk.XPath
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required by XML serialization")]
	public class PropertyExtractorCollectionSerializerSurrogate : PropertyExtractorCollection
	{
		public PropertyExtractorCollectionSerializerSurrogate() { }

		public PropertyExtractorCollectionSerializerSurrogate(PropertyExtractorCollection extractors)
			: base(extractors?.Precedence ?? throw new ArgumentNullException(nameof(extractors)), extractors) { }

		#region Base Class Member Overrides

		public override void ReadXml(XmlReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			reader.AssertStartElement("Extractors");
			var isEmpty = reader.IsEmptyElement;
			reader.ReadStartElement("Extractors");
			if (isEmpty) return;
			base.ReadXml(reader);
			reader.ReadEndElement("Extractors");
		}

		#endregion
	}
}
