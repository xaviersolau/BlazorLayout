// ----------------------------------------------------------------------
// <copyright file="CellTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers.Grid;
using SoloX.BlazorLayout.UTest.Core;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers.Grid
{
    public class CellTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertIdIsProperlyRendered<Cell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertClassIsProperlyRendered<Cell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            var grid = new GridContainer();

            PanelHelpers.AssertElementReferenceIsProperlySet<Cell>(
                builder =>
                {
                    builder.AddCascadingValue(grid);
                });
        }

    }
}
