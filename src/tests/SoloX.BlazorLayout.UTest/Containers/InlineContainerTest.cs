// ----------------------------------------------------------------------
// <copyright file="InlineContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers;
using SoloX.BlazorLayout.UTest.Core;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Containers
{
    public class InlineContainerTest
    {
        [Fact]
        public void ItShouldRenderWithTheGivenId()
        {
            PanelHelpers.AssertIdIsProperlyRendered<InlineContainer>();
        }

        [Fact]
        public void ItShouldRenderWithTheGivenClass()
        {
            PanelHelpers.AssertClassIsProperlyRendered<InlineContainer>();
        }
    }
}
