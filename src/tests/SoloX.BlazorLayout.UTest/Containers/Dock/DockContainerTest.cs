// ----------------------------------------------------------------------
// <copyright file="DockContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using SoloX.BlazorLayout.Containers.Dock;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.UTest.Core;
using SoloX.BlazorLayout.UTest.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers.Dock
{
    public class DockContainerTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            PanelHelpers.AssertIdIsProperlyRendered<DockContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            PanelHelpers.AssertClassIsProperlyRendered<DockContainer>();
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            PanelHelpers.AssertElementReferenceIsProperlySet<DockContainer>();
        }

        [Fact]
        public void ItShouldRenderWithADockDisplay()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<DockContainer>();

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.LocalName.Should().Be(TagNames.Div);

            rootElement.ClassName.Should().Contain("dock-container");

            var style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr");
        }

        [Theory]
        [InlineData(Side.Left, "auto 1fr", "1fr", "1 / 2", "1 / 2")]
        [InlineData(Side.Right, "1fr auto", "1fr", "2 / 3", "1 / 2")]
        [InlineData(Side.Top, "1fr", "auto 1fr", "1 / 2", "1 / 2")]
        [InlineData(Side.Bottom, "1fr", "1fr auto", "1 / 2", "2 / 3")]
        public void ItShouldRenderWithADockPanelOnTheRightLocation(Side side, string expectedGridColumns, string expectedGridRows, string expectedColumn, string expectedRow)
        {
            var dockPanelId = "dock-panel-id";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<DockContainer>(
                builder =>
                {
                    builder.AddChildContent<DockPanel>(
                        dockPanelBuilder =>
                        {
                            dockPanelBuilder.Add(d => d.Side, side);
                            dockPanelBuilder.Add(d => d.Id, dockPanelId);
                        });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var rootElement = cut.Nodes[0].As<IElement>();

            var style = rootElement.ComputeCurrentStyle();
            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == expectedGridColumns);
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == expectedGridRows);

            var dockPanelElt = cut.Find($"#{dockPanelId}");
            dockPanelElt.Should().NotBeNull();

            var dockPanelStyle = StyleHelper.LoadStyleAttribute(dockPanelElt);
            dockPanelStyle.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedColumn);
            dockPanelStyle.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow);
        }
    }
}
