// ----------------------------------------------------------------------
// <copyright file="PanelHelpers.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using SoloX.BlazorLayout.Core;

namespace SoloX.BlazorLayout.UTest.Helpers
{
    public static class PanelHelpers
    {
        public static void AssertIdIsProperlyRendered<TPanel>(
            Action<ComponentParameterCollectionBuilder<TPanel>>? setup = null)
            where TPanel : APanel
        {
            var id = "id-to-render";
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<TPanel>(
                builder =>
                {
                    builder.Add(x => x.Id, id);

                    setup?.Invoke(builder);
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.Id.Should().Be(id);
        }

        public static void AssertElementReferenceIsProperlySet<TPanel>(
            Action<ComponentParameterCollectionBuilder<TPanel>>? setup = null)
            where TPanel : APanel
        {
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<TPanel>(
                builder =>
                {
                    setup?.Invoke(builder);
                });

            // Assert
            cut.Instance.ElementReference.Id.Should().NotBeNullOrEmpty();
        }

        public static void AssertClassIsProperlyRendered<TPanel>(
            Action<ComponentParameterCollectionBuilder<TPanel>>? setup = null)
            where TPanel : APanel
        {
            var classToAdd = "class-to-add";
            // Arrange
            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<TPanel>(
                builder =>
                {
                    builder.Add(x => x.Class, classToAdd);

                    setup?.Invoke(builder);
                });

            // Assert
            cut.Nodes.Length.Should().Be(1);
            var rootElement = cut.Nodes[0].As<IElement>();

            rootElement.ClassList.Should().Contain(x => x == classToAdd);
        }
    }
}
