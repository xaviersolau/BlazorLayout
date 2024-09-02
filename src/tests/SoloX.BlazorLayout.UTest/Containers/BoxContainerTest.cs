// ----------------------------------------------------------------------
// <copyright file="BoxContainerTest.cs" company="Xavier Solau">
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
using System;

namespace SoloX.BlazorLayout.UTest.Containers
{
    public class BoxContainerTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            PanelHelpers.AssertIdIsProperlyRendered<BoxContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            PanelHelpers.AssertClassIsProperlyRendered<BoxContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenStyle()
        {
            PanelHelpers.AssertStyleIsProperlyRendered<BoxContainer>();
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            PanelHelpers.AssertElementReferenceIsProperlySet<BoxContainer>();
        }

        [Theory]
        [InlineData(Fill.Full)]
        [InlineData(Fill.None)]
        [InlineData(Fill.Vertical)]
        [InlineData(Fill.Horizontal)]
        public void ItShouldFillParentSpaceAccordinglyToFillParameter(Fill fill)
        {
            ContainerHelper.AssertFillClassIsProperlyRendered<BoxContainer>(fill);
        }

        [Theory]
        [InlineData(DisplaySize.Small)]
        [InlineData(DisplaySize.Large)]
        [InlineData(DisplaySize.VeryLarge)]
        [InlineData(DisplaySize.VeryVeryLarge)]
        [InlineData(DisplaySize.Small | DisplaySize.Large)]
        [InlineData(DisplaySize.Small | DisplaySize.Large | DisplaySize.VeryLarge)]
        [InlineData(DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge)]
        [InlineData(DisplaySize.Large | DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge)]
        public void ItShouldDisplayComponentAccordingToDisplayOnParameter(DisplaySize displaySize)
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<BoxContainer>(
                builder =>
                {
                    builder.Add(c => c.DisplayOn, displaySize);
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.ClassName.Should().Contain(GetExpectedDisplayClass(displaySize));
        }

        private static string GetExpectedDisplayClass(DisplaySize displaySize)
#pragma warning disable IDE0072 // Add missing cases
            => displaySize switch
            {
                DisplaySize.None => "no-display",
                DisplaySize.Small => "display-on-small",
                DisplaySize.Large => "display-on-large",
                DisplaySize.VeryLarge => "display-on-very-large",
                DisplaySize.VeryVeryLarge => "display-on-very-very-large",
                DisplaySize.Small | DisplaySize.Large => "display-from-small-to-large",
                DisplaySize.Small | DisplaySize.Large | DisplaySize.VeryLarge => "display-from-small-to-very-large",
                DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge => "display-from-very-large-to-very-very-large",
                DisplaySize.Large | DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge => "display-from-large-to-very-very-large",
                _ => throw new ArgumentOutOfRangeException(nameof(displaySize)),
            };
#pragma warning restore IDE0072 // Add missing cases

        [Theory]
        [InlineData(DisplaySize.Large | DisplaySize.VeryLarge)]
        public void ItShouldThrowWhenDisplayOnParameterIsInvalid(DisplaySize displaySize)
        {
            using var ctx = new TestContext();

            var act = () => ctx.RenderComponent<BoxContainer>(
                builder =>
                {
                    builder.Add(c => c.DisplayOn, displaySize);
                });

            act.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void ItShouldRenderChildContentAsChildOfTheDivRootElement()
        {
            var childContent = "<span id=\"test-id\">ChildContent</span>";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<BoxContainer>(
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
