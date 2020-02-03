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
using System.Xml;

namespace Be.Stateless.BizTalk.Xml
{
	/// <summary>
	/// <see cref="XmlReader"/> wrapper that delegates all operations to the wrapped <see cref="XmlReader"/>-derived instance.
	/// </summary>
	public abstract class XmlReaderWrapper : XmlReader
	{
		protected XmlReaderWrapper(XmlReader reader)
		{
			InnerReader = reader ?? throw new ArgumentNullException(nameof(reader));
		}

		#region Base Class Member Overrides

		public override int AttributeCount => InnerReader.AttributeCount;

		public override string BaseURI => InnerReader.BaseURI;

		public override void Close()
		{
			InnerReader.Close();
		}

		public override int Depth => InnerReader.Depth;

		public override bool EOF => InnerReader.EOF;

		public override string GetAttribute(string name)
		{
			return InnerReader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string ns)
		{
			return InnerReader.GetAttribute(name, ns);
		}

		public override string GetAttribute(int i)
		{
			return InnerReader.GetAttribute(i);
		}

		public override bool HasValue => InnerReader.HasValue;

		public override bool IsEmptyElement => InnerReader.IsEmptyElement;

		public override string LocalName => InnerReader.LocalName;

		public override string LookupNamespace(string prefix)
		{
			return InnerReader.LookupNamespace(prefix);
		}

		public override bool MoveToAttribute(string name)
		{
			return InnerReader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return InnerReader.MoveToAttribute(name, ns);
		}

		public override bool MoveToElement()
		{
			return InnerReader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return InnerReader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return InnerReader.MoveToNextAttribute();
		}

		public override string NamespaceURI => InnerReader.NamespaceURI;

		public override XmlNameTable NameTable => InnerReader.NameTable;

		public override XmlNodeType NodeType => InnerReader.NodeType;

		public override string Prefix => InnerReader.Prefix;

		public override bool Read()
		{
			return InnerReader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return InnerReader.ReadAttributeValue();
		}

		public override ReadState ReadState => InnerReader.ReadState;

		public override void ResolveEntity()
		{
			InnerReader.ResolveEntity();
		}

		public override string Value => InnerReader.Value;

		#endregion

		protected XmlReader InnerReader { get; set; }
	}
}
