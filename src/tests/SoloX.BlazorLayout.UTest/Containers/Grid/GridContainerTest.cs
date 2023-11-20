// ----------------------------------------------------------------------
// <copyright file="GridContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using SoloX.BlazorLayout.Containers.Grid;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.UTest.Containers.Grid.Components;
using SoloX.BlazorLayout.UTest.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers.Grid
{
    public class GridContainerTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            PanelHelpers.AssertIdIsProperlyRendered<GridContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            PanelHelpers.AssertClassIsProperlyRendered<GridContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenStyle()
        {
            PanelHelpers.AssertStyleIsProperlyRendered<GridContainer>();
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            PanelHelpers.AssertElementReferenceIsProperlySet<GridContainer>();
        }

        [Fact]
        public void ItShouldRenderWithAGridDisplay()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<GridContainer>(
                builder =>
                {
                    builder.AddChildContent<GridColumn>();
                    builder.AddChildContent<GridColumn>();
                    builder.AddChildContent<GridRow>();
                    builder.AddChildContent<GridRow>();
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.LocalName.Should().Be(TagNames.Div);

            rootElement.ClassName.Should().Contain("grid-container");

            var style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr 1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr 1fr");
        }

        [Fact]
        public void ItShouldChangeTheGridDisplayDependingOnTheColumnRowParameters()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<GridContainer>(
                builder =>
                {
                    builder.AddChildContent<GridColumn>(cb =>
                    {
                        cb.Add(c => c.Repeat, 2);
                    });
                    builder.AddChildContent<GridRow>(cr =>
                    {
                        cr.Add(c => c.Repeat, 2);
                    });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.LocalName.Should().Be(TagNames.Div);

            rootElement.ClassName.Should().Contain("grid-container");

            var style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr 1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr 1fr");


            // Act again
            var gridColumn = cut.FindComponent<GridColumn>();

            gridColumn.SetParametersAndRender(
                builder =>
                {
                    builder.Add(cb => cb.Repeat, 1);
                });

            // Assert new state

            rootElement = cut.Nodes[0].As<IElement>();
            style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr 1fr");
        }

        [Fact]
        public void ItShouldChangeTheGridDisplayWhenAColumnIsDisposed()
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ConditonalColumnAndRow>();

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.LocalName.Should().Be(TagNames.Div);

            rootElement.ClassName.Should().Contain("grid-container");

            var style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr 1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr 1fr");


            // Act again
            cut.SetParametersAndRender(builder => builder.Add(x => x.DisableColumn, true));

            // Assert new state
            rootElement = cut.Nodes[0].As<IElement>();
            style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr 1fr");

            // Act again
            cut.SetParametersAndRender(builder => builder.Add(x => x.DisableRow, true));

            // Assert new state
            rootElement = cut.Nodes[0].As<IElement>();
            style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr");

            // Act again
            cut.SetParametersAndRender(builder =>
            {
                builder.Add(x => x.DisableColumn, false);
                builder.Add(x => x.DisableRow, true);
            });

            // Assert new state
            rootElement = cut.Nodes[0].As<IElement>();
            style = rootElement.ComputeCurrentStyle();

            style.Should().ContainSingle(x => x.Name == "grid-template-columns" && x.Value == "1fr 1fr");
            style.Should().ContainSingle(x => x.Name == "grid-template-rows" && x.Value == "1fr");
        }

        [Theory]
        [InlineData(Fill.Full)]
        [InlineData(Fill.None)]
        [InlineData(Fill.Vertical)]
        [InlineData(Fill.Horizontal)]
        public void ItShouldFillParentSpaceAccordinglyToFillParameter(Fill fill)
        {
            ContainerHelper.AssertIdIsProperlyRendered<GridContainer>(fill);
        }

        [Fact]
        public void ItShouldRenderCellsOnTheRightLocation()
        {
            var cellId1 = "cell-id1";
            var cellId2 = "cell-id2";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<GridContainer>(
                builder =>
                {
                    builder.AddChildContent<GridColumn>();
                    builder.AddChildContent<GridColumn>();
                    builder.AddChildContent<GridRow>();
                    builder.AddChildContent<GridRow>();
                    builder.AddChildContent<GridCell>(
                        cellBuilder =>
                        {
                            cellBuilder.Add(c => c.Column, 0);
                            cellBuilder.Add(c => c.Row, 0);
                            cellBuilder.Add(c => c.Id, cellId1);
                        });
                    builder.AddChildContent<GridCell>(
                        cellBuilder =>
                        {
                            cellBuilder.Add(c => c.Column, 1);
                            cellBuilder.Add(c => c.Row, 1);
                            cellBuilder.Add(c => c.Id, cellId2);
                        });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var cellElt1 = cut.Find($"#{cellId1}");

            var style1 = StyleHelper.LoadStyleAttribute(cellElt1);
            style1.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == "1");
            style1.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == "1");

            var cellElt2 = cut.Find($"#{cellId2}");

            var style2 = StyleHelper.LoadStyleAttribute(cellElt2);
            style2.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == "2");
            style2.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == "2");
        }

        [Theory]
        [InlineData("left", "top", "1", "1")]
        [InlineData("right", "top", "2", "1")]
        [InlineData("left", "bottom", "1", "2")]
        [InlineData("right", "bottom", "2", "2")]
        [InlineData("0", "0", "1", "1")]
        [InlineData("1", "0", "2", "1")]
        [InlineData("0", "1", "1", "2")]
        [InlineData("1", "1", "2", "2")]
        public void ItShouldRenderCellsOnTheRightNamedLocation(string colName, string rowName, string expectedCol, string expectedRow)
        {
            var cellId1 = "cell-id1";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<GridContainer>(
                builder =>
                {
                    builder.AddChildContent<GridColumn>(
                        colBuilder =>
                        {
                            colBuilder.Add(c => c.Name, "left");
                        });
                    builder.AddChildContent<GridColumn>(
                        colBuilder =>
                        {
                            colBuilder.Add(c => c.Name, "right");
                        });
                    builder.AddChildContent<GridRow>(
                        rowBuilder =>
                        {
                            rowBuilder.Add(c => c.Name, "top");
                        });

                    builder.AddChildContent<GridRow>(
                        rowBuilder =>
                        {
                            rowBuilder.Add(c => c.Name, "bottom");
                        });
                    builder.AddChildContent<GridCell>(
                        cellBuilder =>
                        {
                            cellBuilder.Add(c => c.Column, colName);
                            cellBuilder.Add(c => c.Row, rowName);
                            cellBuilder.Add(c => c.Id, cellId1);
                        });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var cellElt1 = cut.Find($"#{cellId1}");

            var style1 = StyleHelper.LoadStyleAttribute(cellElt1);
            style1.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedCol);
            style1.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow);
        }

        [Theory]
        [InlineData("left", "left", "top", "top", "1/2", "1/2")]
        [InlineData("left", "middle", "top", "bottom", "1/3", "1/4")]
        [InlineData("middle", "right", "middle", "bottom", "2/4", "2/4")]
        [InlineData("left", "right", "bottom", "bottom", "1/4", "3/4")]
        public void ItShouldRenderCellsOnTheRightNamedLocationWithSpan(string colNameStart, string colNameEnd, string rowNameStart, string rowNameEnd, string expectedCol, string expectedRow)
        {
            var cellId1 = "cell-id1";

            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<GridContainer>(
                builder =>
                {
                    builder.AddChildContent<GridColumn>(
                        colBuilder =>
                        {
                            colBuilder.Add(c => c.Name, "left");
                        });
                    builder.AddChildContent<GridColumn>(
                        colBuilder =>
                        {
                            colBuilder.Add(c => c.Name, "middle");
                        });
                    builder.AddChildContent<GridColumn>(
                        colBuilder =>
                        {
                            colBuilder.Add(c => c.Name, "right");
                        });
                    builder.AddChildContent<GridRow>(
                        rowBuilder =>
                        {
                            rowBuilder.Add(c => c.Name, "top");
                        });
                    builder.AddChildContent<GridRow>(
                        rowBuilder =>
                        {
                            rowBuilder.Add(c => c.Name, "middle");
                        });
                    builder.AddChildContent<GridRow>(
                        rowBuilder =>
                        {
                            rowBuilder.Add(c => c.Name, "bottom");
                        });
                    builder.AddChildContent<GridCell>(
                        cellBuilder =>
                        {
                            cellBuilder.Add(c => c.Column, colNameStart);
                            cellBuilder.Add(c => c.ColumnEnd, colNameEnd);
                            cellBuilder.Add(c => c.Row, rowNameStart);
                            cellBuilder.Add(c => c.RowEnd, rowNameEnd);
                            cellBuilder.Add(c => c.Id, cellId1);
                        });
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);

            var cellElt1 = cut.Find($"#{cellId1}");

            var style1 = StyleHelper.LoadStyleAttribute(cellElt1);
            style1.Should().ContainSingle(x => x.Name == "grid-column" && x.Value == expectedCol);
            style1.Should().ContainSingle(x => x.Name == "grid-row" && x.Value == expectedRow);
        }
    }
}
