// ----------------------------------------------------------------------
// <copyright file="DockPanelTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers.Dock;
using SoloX.BlazorLayout.UTest.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers.Dock
{
    public class DockPanelTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            var dock = new DockContainer();

            PanelHelpers.AssertIdIsProperlyRendered<DockPanel>(
                builder =>
                {
                    builder.AddCascadingValue<DockContainer>(dock);
                });
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            var dock = new DockContainer();

            PanelHelpers.AssertClassIsProperlyRendered<DockPanel>(
                builder =>
                {
                    builder.AddCascadingValue<DockContainer>(dock);
                });
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            var dock = new DockContainer();

            PanelHelpers.AssertElementReferenceIsProperlySet<DockPanel>(
                builder =>
                {
                    builder.AddCascadingValue<DockContainer>(dock);
                });
        }
    }
}
