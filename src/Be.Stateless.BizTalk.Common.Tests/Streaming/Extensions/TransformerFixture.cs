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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Runtime.Caching;
using Be.Stateless.BizTalk.Transform;
using Be.Stateless.BizTalk.Xml.Xsl;
using Be.Stateless.IO;
using FluentAssertions;
using Microsoft.BizTalk.Message.Interop;
using Moq;
using Xunit;

namespace Be.Stateless.BizTalk.Streaming.Extensions
{
	public class TransformerFixture
	{
		[Fact]
		public void ApplySatisfiesExtensionRequirementsWithMessageContext()
		{
			var contextMock = new Mock<IBaseMessageContext>();
			var transform = typeof(CompoundContextMapTransform);
			var arguments = XsltCache.Instance[transform].Arguments;

			arguments.GetExtensionObject(BaseMessageContextFunctions.TARGET_NAMESPACE).Should().BeNull();

			var stream = new StringStream("<?xml version='1.0' encoding='utf-16'?><root></root>");
			var sut = new Transformer(new Stream[] { stream });
			sut.ExtendWith(contextMock.Object).Apply(transform);

			arguments.GetExtensionObject(BaseMessageContextFunctions.TARGET_NAMESPACE).Should().BeNull();

			contextMock.Verify(c => c.Read(BizTalkFactoryProperties.EnvironmentTag.Name, BizTalkFactoryProperties.EnvironmentTag.Namespace), Times.Once());
			contextMock.Verify(c => c.Read(BtsProperties.Operation.Name, BtsProperties.Operation.Namespace), Times.Once());
		}

		[Fact]
		public void ApplySatisfiesExtensionRequirementsWithoutMessageContext()
		{
			var transform = typeof(IdentityTransform);
			var arguments = XsltCache.Instance[transform].Arguments;
			arguments.GetExtensionObject(BaseMessageContextFunctions.TARGET_NAMESPACE).Should().BeNull();

			var stream = new StringStream("<?xml version='1.0' encoding='utf-16'?><root></root>");
			var sut = new Transformer(new Stream[] { stream });
			sut.Apply(transform);

			arguments.GetExtensionObject(BaseMessageContextFunctions.TARGET_NAMESPACE).Should().BeNull();
		}

		[Fact]
		public void ApplyThrowsIfExtensionRequirementsWithMessageContextCannotBeSatisfied()
		{
			var transform = typeof(CompoundContextMapTransform);

			var stream = new StringStream("<?xml version='1.0' encoding='utf-16'?><root></root>");
			var sut = new Transformer(new Stream[] { stream });

			Action act = () => sut.Apply(transform);
			act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: context");
		}

		[Fact]
		public void ApplyTransformsWithImportedAndIncludedStylesheets()
		{
			const string xml = @"<root><one>a</one><two>b</two><six>sense</six></root>";
			using (var stream = new StringStream(xml).Transform().ExtendWith(new Mock<IBaseMessageContext>().Object).Apply(typeof(CompositeMapTransform)))
			using (var reader = XmlReader.Create(stream))
			{
				reader.MoveToContent();
				reader.ReadOuterXml().Should().Be("<root><first>a</first><second>b</second><sixth>sense</sixth></root>");
			}
		}

		[Fact]
		public void BuildArgumentListYieldsFreshCopyWhenRequired()
		{
			var arguments = new XsltArgumentList();
			var contextMock = new Mock<IBaseMessageContext>();

			var sut = new Transformer(new Stream[] { new MemoryStream() });
			sut.ExtendWith(contextMock.Object);

			var descriptor = XsltCache.Instance[typeof(IdentityTransform)];

			// no specific arguments, no message ctxt requirement, same XsltArgumentList instance can be shared
			sut.BuildArgumentList(descriptor, null).Should().BeSameAs(descriptor.Arguments);

			// specific arguments, no message ctxt requirement, new XsltArgumentList instance is required
			sut.BuildArgumentList(descriptor, arguments).Should().NotBeSameAs(descriptor.Arguments).And.NotBeSameAs(arguments);

			descriptor = XsltCache.Instance[typeof(CompoundContextMapTransform)];

			// no specific arguments but message ctxt requirement, new XsltArgumentList instance is required
			sut.BuildArgumentList(descriptor, null).Should().NotBeSameAs(descriptor.Arguments).And.NotBeSameAs(arguments);

			// specific arguments and message ctxt requirement, new XsltArgumentList instance is required
			sut.BuildArgumentList(descriptor, null).Should().NotBeSameAs(descriptor.Arguments).And.NotBeSameAs(arguments);
		}
	}
}
