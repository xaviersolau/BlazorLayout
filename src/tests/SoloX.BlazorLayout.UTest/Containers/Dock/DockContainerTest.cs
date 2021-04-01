// ----------------------------------------------------------------------
// <copyright file="DockContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers.Dock;
using SoloX.BlazorLayout.UTest.Core;
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
    }
}
