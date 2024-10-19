// ----------------------------------------------------------------------
// <copyright file="ContainerHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using SoloX.BlazorLayout.Core;
using System;

namespace SoloX.BlazorLayout.UTests.Helpers
{
    public static class ContainerHelper
    {
        public static void AssertFillClassIsProperlyRendered<TContainer>(
            Fill fill,
            Action<ComponentParameterCollectionBuilder<TContainer>>? setup = null)
            where TContainer : AContainer
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<TContainer>(
                builder =>
                {
                    builder.Add(c => c.Fill, fill);

                    setup?.Invoke(builder);
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.ClassName.Should().Contain(GetExpectedFillClass(fill));
        }

        private static string GetExpectedFillClass(Fill fill)
            => fill switch
            {
                Fill.None => "container-size-content",
                Fill.Full => "container-size-100",
                Fill.Horizontal => "container-width-100",
                Fill.Vertical => "container-height-100",
                _ => throw new ArgumentOutOfRangeException(nameof(fill)),
            };
    }
}