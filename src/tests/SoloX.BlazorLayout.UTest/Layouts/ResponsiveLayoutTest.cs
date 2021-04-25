// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayoutTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Bunit;
using FluentAssertions;
using SoloX.BlazorLayout.Layouts;
using SoloX.BlazorLayout.UTest.Helpers;
using Xunit;

namespace SoloX.BlazorLayout.UTest.Layouts
{
    public class ResponsiveLayoutTest
    {
        [Fact]
        public void ItShouldRenderWithHeader()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.ChildContent, childHtml);
                });

            // Assert
            var childElement = cut.Find($"#{childId}");

            childElement.ToMarkup().Trim().Should().BeEquivalentTo(childHtml);
        }

        [Fact]
        public void ItShouldRenderWithFooter()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.Footer, childHtml);
                });

            // Assert
            var childElement = cut.Find($"#{childId}");

            childElement.ToMarkup().Trim().Should().BeEquivalentTo(childHtml);
        }

        [Fact]
        public void ItShouldRenderWithNavMenu()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.NavigationMenu, childHtml);
                });

            // Assert
            var childElement = cut.Find($"#{childId}");

            childElement.ToMarkup().Trim().Should().BeEquivalentTo(childHtml);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ItShouldRenderWithOutlineIfEnabled(bool enabled)
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.EnableOutline, enabled);
                    builder.Add(l => l.Outline, childHtml);
                });

            // Assert
            if (enabled)
            {
                var childElement = cut.Find($"#{childId}");

                childElement.ToMarkup().Trim().Should().BeEquivalentTo(childHtml);
            }
            else
            {
                var childElements = cut.FindAll($"#{childId}");

                childElements.Should().BeEmpty();
            }
        }

        [Fact]
        public void ItShouldRenderNavMenuWithGivenClass()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            var navClass = "my-nav-class";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.NavigationClass, navClass);
                    builder.Add(l => l.NavigationMenu, childHtml);
                });

            // Assert

            var navPanel = cut.Find($"#{ResponsiveLayout.NavigationPanelId}");

            navPanel.ClassList.Should().Contain(navClass);
        }

        [Fact]
        public void ItShouldRenderNavMenuWithGivenStyle()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            var styleKey = "some-style";
            var styleValue = "some-value";

            var navStyle = $"{styleKey}: {styleValue};";

            using var ctx = new TestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder.Add(l => l.NavigationStyle, navStyle);
                    builder.Add(l => l.NavigationMenu, childHtml);
                });

            // Assert

            var navPanel = cut.Find($"#{ResponsiveLayout.NavigationPanelId}");

            var style = StyleHelper.LoadStyleAttribute(navPanel);

            style.Should().ContainSingle(x => x.Name == styleKey && x.Value == styleValue);
        }
    }
}
