﻿// ----------------------------------------------------------------------
// <copyright file="InlineContainerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Containers;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.UTests.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTests.Containers
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

        [Fact]
        public void ItShouldRenderWithTheGivenStyle()
        {
            PanelHelpers.AssertStyleIsProperlyRendered<InlineContainer>();
        }

        [Fact]
        public void ItShouldInitializeElementReference()
        {
            PanelHelpers.AssertElementReferenceIsProperlySet<InlineContainer>();
        }

        [Theory]
        [InlineData(Fill.Full)]
        [InlineData(Fill.None)]
        [InlineData(Fill.Vertical)]
        [InlineData(Fill.Horizontal)]
        public void ItShouldFillParentSpaceAccordinglyToFillParameter(Fill fill)
        {
            ContainerHelper.AssertFillClassIsProperlyRendered<InlineContainer>(fill);
        }
    }
}
