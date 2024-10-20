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
using SoloX.BlazorLayout.UTests.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTests.Containers.Dock
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
        public void ItShouldRenderWithTheGivenStyle()
        {
            PanelHelpers.AssertStyleIsProperlyRendered<DockContainer>();
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
        [InlineData(Fill.Full)]
        [InlineData(Fill.None)]
        [InlineData(Fill.Vertical)]
        [InlineData(Fill.Horizontal)]
        public void ItShouldFillParentSpaceAccordinglyToFillParameter(Fill fill)
        {
            ContainerHelper.AssertFillClassIsProperlyRendered<DockContainer>(fill);
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

            var style = StyleHelper.LoadStyleAttribute(rootElement);
            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == expectedGridColumns);
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == expectedGridRows);

            var dockPanelElt = cut.Find($"#{dockPanelId}");
            dockPanelElt.Should().NotBeNull();

            var dockPanelStyle = StyleHelper.LoadStyleAttribute(dockPanelElt);
            dockPanelStyle.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedColumn);
            dockPanelStyle.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow);
        }


        [Theory]
        [InlineData(Side.Left, "var(--dock-container-panel-proportion) 1fr", "1fr", 20, false)]
        [InlineData(Side.Right, "1fr var(--dock-container-panel-proportion)", "1fr", 10, false)]
        [InlineData(Side.Top, "1fr", "var(--dock-container-panel-proportion) 1fr", 33, false)]
        [InlineData(Side.Bottom, "1fr", "1fr var(--dock-container-panel-proportion)", 25, false)]
        [InlineData(Side.Left, "fit-content(var(--dock-container-panel-proportion)) 1fr", "1fr", 20, true)]
        [InlineData(Side.Right, "1fr fit-content(var(--dock-container-panel-proportion))", "1fr", 10, true)]
        [InlineData(Side.Top, "1fr", "fit-content(var(--dock-container-panel-proportion)) 1fr", 33, true)]
        [InlineData(Side.Bottom, "1fr", "1fr fit-content(var(--dock-container-panel-proportion))", 25, true)]
        public void ItShouldRenderWithAGivenProportion(Side side, string expectedGridColumns, string expectedGridRows, int proportion, bool isMax)
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<DockContainer>(
                builder =>
                {
                    if (isMax)
                    {
                        builder.Add(c => c.MaxProportion, proportion);
                    }
                    else
                    {
                        builder.Add(c => c.Proportion, proportion);
                    }
                    builder.AddChildContent<DockPanel>(
                        dockPanelBuilder =>
                        {
                            dockPanelBuilder.Add(d => d.Side, side);
                        });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var rootElement = cut.Nodes[0].As<IElement>();

            var style = StyleHelper.LoadStyleAttribute(rootElement);
            style.Should().ContainSingle(x => x.Name == "--dock-container-panel-proportion" && x.Value == $"{proportion}%");
            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == expectedGridColumns);
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == expectedGridRows);
        }

        [Theory]
        [InlineData(
            Side.Left, "auto 1fr", "1fr", "1 / 2", "1 / 2",
            Side.Right, "1fr auto", "1fr", "2 / 3", "1 / 2")]
        public void ItShouldRenderAndUpdateTheDockPanelLocation(
            Side side1, string expectedGridColumns1, string expectedGridRows1, string expectedColumn1, string expectedRow1,
            Side side2, string expectedGridColumns2, string expectedGridRows2, string expectedColumn2, string expectedRow2)
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
                            dockPanelBuilder.Add(d => d.Side, side1);
                            dockPanelBuilder.Add(d => d.Id, dockPanelId);
                        });
                });

            var dockPanel = cut.FindComponent<DockPanel>();

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var dockPanelElt = cut.Find($"#{dockPanelId}");
            dockPanelElt.Should().NotBeNull();

            var rootElement = cut.Nodes[0].As<IElement>();

            var style1 = StyleHelper.LoadStyleAttribute(rootElement);
            style1.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == expectedGridColumns1);
            style1.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == expectedGridRows1);

            var dockPanelStyle1 = StyleHelper.LoadStyleAttribute(dockPanelElt);
            dockPanelStyle1.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedColumn1);
            dockPanelStyle1.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow1);

            dockPanel.SetParametersAndRender(
                builder =>
                {
                    builder.Add(d => d.Side, side2);
                });

            rootElement = cut.Nodes[0].As<IElement>();

            var style2 = StyleHelper.LoadStyleAttribute(rootElement);
            style2.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == expectedGridColumns2);
            style2.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == expectedGridRows2);

            var dockPanelStyle2 = StyleHelper.LoadStyleAttribute(dockPanelElt);
            dockPanelStyle2.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedColumn2);
            dockPanelStyle2.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow2);
        }
    }
}
