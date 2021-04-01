// ----------------------------------------------------------------------
// <copyright file="ContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Bunit;
using AngleSharp.Dom;
using FluentAssertions;
using SoloX.BlazorLayout.Containers;
using Xunit;
using SoloX.BlazorLayout.UTest.Helpers;
using SoloX.BlazorLayout.Core;

namespace SoloX.BlazorLayout.UTest.Containers
{
    public class ContainerTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            PanelHelpers.AssertIdIsProperlyRendered<Container>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            PanelHelpers.AssertClassIsProperlyRendered<Container>();
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            PanelHelpers.AssertElementReferenceIsProperlySet<Container>();
        }

        [Theory]
        [InlineData(Fill.Full)]
        [InlineData(Fill.None)]
        [InlineData(Fill.Vertical)]
        [InlineData(Fill.Horizontal)]
        public void ItShouldFillParentSpaceAccordinglyToFillParameter(Fill fill)
        {
            ContainerHelper.AssertIdIsProperlyRendered<Container>(fill);
        }

        [Fact]
        public void ItShouldRenderChildContentAsChildOfTheDivRootElement()
        {
            var childContent = "<span id=\"test-id\">ChildContent</span>";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<Container>(
                builder =>
                {
                    builder.AddChildContent(childContent);
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.LocalName.Should().Be(TagNames.Div);

            rootElement.QuerySelector("#test-id").MarkupMatches(childContent);
        }

    }
}
