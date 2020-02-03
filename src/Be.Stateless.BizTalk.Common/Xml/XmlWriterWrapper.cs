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
using System.Xml;

namespace Be.Stateless.BizTalk.Xml
{
	/// <summary>
	/// <see cref="XmlWriter"/> wrapper that delegates all operations to the wrapped <see cref="XmlWriter"/>-derived instance.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public abstract class XmlWriterWrapper : XmlWriter
	{
		protected XmlWriterWrapper(XmlWriter writer)
		{
			InnerWriter = writer ?? throw new ArgumentNullException(nameof(writer));
		}

		#region Base Class Member Overrides

		public override void Flush()
		{
			InnerWriter.Flush();
		}

		public override string LookupPrefix(string ns)
		{
			return InnerWriter.LookupPrefix(ns);
		}

		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			InnerWriter.WriteBase64(buffer, index, count);
		}

		public override void WriteCData(string text)
		{
			InnerWriter.WriteCData(text);
		}

		public override void WriteCharEntity(char ch)
		{
			InnerWriter.WriteCharEntity(ch);
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			InnerWriter.WriteChars(buffer, index, count);
		}

		public override void WriteComment(string text)
		{
			InnerWriter.WriteComment(text);
		}

		[SuppressMessage("ReSharper", "IdentifierTypo")]
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			InnerWriter.WriteDocType(name, pubid, sysid, subset);
		}

		public override void WriteEndAttribute()
		{
			InnerWriter.WriteEndAttribute();
		}

		public override void WriteEndDocument()
		{
			InnerWriter.WriteEndDocument();
		}

		public override void WriteEndElement()
		{
			InnerWriter.WriteEndElement();
		}

		public override void WriteEntityRef(string name)
		{
			InnerWriter.WriteEntityRef(name);
		}

		public override void WriteFullEndElement()
		{
			InnerWriter.WriteFullEndElement();
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			InnerWriter.WriteProcessingInstruction(name, text);
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			InnerWriter.WriteRaw(buffer, index, count);
		}

		public override void WriteRaw(string data)
		{
			InnerWriter.WriteRaw(data);
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			InnerWriter.WriteStartAttribute(prefix, localName, ns);
		}

		public override void WriteStartDocument(bool standalone)
		{
			InnerWriter.WriteStartDocument(standalone);
		}

		public override void WriteStartDocument()
		{
			InnerWriter.WriteStartDocument();
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			InnerWriter.WriteStartElement(prefix, localName, ns);
		}

		public override WriteState WriteState => InnerWriter.WriteState;

		public override void WriteString(string text)
		{
			InnerWriter.WriteString(text);
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			InnerWriter.WriteSurrogateCharEntity(lowChar, highChar);
		}

		public override void WriteWhitespace(string ws)
		{
			InnerWriter.WriteWhitespace(ws);
		}

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		protected XmlWriter InnerWriter { get; set; }
	}
}
