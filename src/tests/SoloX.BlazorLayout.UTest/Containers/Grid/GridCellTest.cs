// ----------------------------------------------------------------------
// <copyright file="GridCellTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers.Grid;
using SoloX.BlazorLayout.UTest.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers.Grid
{
    public class GridCellTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertIdIsProperlyRendered<GridCell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertClassIsProperlyRendered<GridCell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

        [Fact]
        public void ItShouldRenderWithTheGivenStyle()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertStyleIsProperlyRendered<GridCell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertElementReferenceIsProperlySet<GridCell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

    }
}
