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

using Be.Stateless.BizTalk.ContextProperties;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.BizTalk.Schemas.Xml;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.XPath.Extensions
{
	public class SchemaAnnotationsExtensionsFixture
	{
		[Fact]
		public void GetExtractors()
		{
			SchemaMetadata.For<RootedSchema>().Annotations.GetExtractors()
				.Should().BeEquivalentTo(
					new PropertyExtractorCollection(
						new XPathExtractor(BizTalkFactoryProperties.CorrelationToken, "/*[local-name()='Root']//*[local-name()='Id']")
					));
		}

		[Fact]
		public void GetExtractorsWhenSchemaContainsNoAnnotation()
		{
			SchemaMetadata.For<Any>().Annotations.GetExtractors()
				.Should().BeEmpty();
		}
	}
}
