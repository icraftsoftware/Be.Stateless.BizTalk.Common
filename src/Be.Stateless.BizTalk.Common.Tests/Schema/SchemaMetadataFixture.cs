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
using Be.Stateless.BizTalk.Schemas.Xml;
using FluentAssertions;
using Microsoft.BizTalk.Component.Interop;
using Xunit;

namespace Be.Stateless.BizTalk.Schema
{
	[SuppressMessage("ReSharper", "StringLiteralTypo")]
	public class SchemaMetadataFixture
	{
		[Fact]
		public void GetBodyXPathForEnvelopeSchema()
		{
			SchemaMetadata.For<Envelope>().BodyXPath.Should().Be("/*[local-name()='Envelope' and namespace-uri()='urn:schemas.stateless.be:biztalk:envelope:2013:07']");
		}

		[Fact]
		public void GetBodyXPathForNonEnvelopeSchema()
		{
			SchemaMetadata.For<Any>().BodyXPath.Should().BeEmpty();
		}

		[Fact]
		public void GetDocumentSpecForRootedSchema()
		{
			SchemaMetadata.For<Any>().DocumentSpec.Should().BeEquivalentTo(new DocumentSpec(typeof(Any).FullName, typeof(Any).Assembly.FullName));
		}

		[Fact]
		public void GetDocumentSpecForRootlessSchema()
		{
			SchemaMetadata.For<RootlessSchema>().DocumentSpec.Should().BeNull();
		}

		[Fact]
		public void GetMessageTypeForRootedSchema()
		{
			SchemaMetadata.For<Any>().MessageType.Should().Be("urn:schemas.stateless.be:biztalk:any:2012:12#Any");
		}

		[Fact]
		public void GetMessageTypeForRootlessSchema()
		{
			SchemaMetadata.For<RootlessSchema>().MessageType.Should().BeEmpty();
		}

		[Fact]
		public void GetRootElementNameForRootedSchema()
		{
			SchemaMetadata.For<Any>().RootElementName.Should().Be("Any");
		}

		[Fact]
		public void GetRootElementNameForRootlessSchema()
		{
			SchemaMetadata.For<RootlessSchema>().RootElementName.Should().BeEmpty();
		}

		[Fact]
		public void GetTargetNamespaceForRootedSchema()
		{
			SchemaMetadata.For<Any>().TargetNamespace.Should().Be("urn:schemas.stateless.be:biztalk:any:2012:12");
		}

		[Fact]
		public void GetTargetNamespaceForRootlessSchema()
		{
			SchemaMetadata.For<RootlessSchema>().TargetNamespace.Should().Be("urn:schemas.stateless.be:unit:type");
		}

		[Fact]
		public void IsEnvelopeSchemaForEnvelopeSchema()
		{
			SchemaMetadata.For<Envelope>().IsEnvelopeSchema.Should().BeTrue();
		}

		[Fact]
		public void IsEnvelopeSchemaForNonEnvelopeSchema()
		{
			SchemaMetadata.For<Any>().IsEnvelopeSchema.Should().BeFalse();
		}

		[Fact]
		public void ThrowsForPropertySchema()
		{
			Func<ISchemaMetadata> act = SchemaMetadata.For<Schemas.BizTalkFactory.Properties>;
			act.Should()
				.Throw<ArgumentException>()
				.WithMessage("SchemaMetadata only supports schemas qualified with a SchemaTypeAttribute whose Type is equal to Document*");
		}
	}
}
