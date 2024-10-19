// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayoutTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using AngleSharp.Dom;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Layouts;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.UTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace SoloX.BlazorLayout.UTests.Layouts
{
    public class ResponsiveLayoutTest
    {
        [Fact]
        public void ItShouldRenderWithHeader()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";
            using var ctx = BuildTestContext();

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
        public void ItShouldRenderWithHeaderHidden()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";
            using var ctx = BuildTestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder
                        .Add(l => l.ChildContent, childHtml)
                        .Add(l => l.HideHeader, true);
                });

            // Assert
            var headerElement = cut.Find($"#{ResponsiveLayout.HeaderPanelId}");

            headerElement.ClassList.Should().Contain("hide-header");
        }

        [Fact]
        public void ItShouldRenderWithFooter()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = BuildTestContext();

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
        public void ItShouldRenderWithFooterHidden()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = BuildTestContext();

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>(
                builder =>
                {
                    builder
                        .Add(l => l.Footer, childHtml)
                        .Add(l => l.HideFooter, true);
                });

            // Assert
            var headerElement = cut.Find($"#{ResponsiveLayout.FooterPanelId}");

            headerElement.ClassList.Should().Contain("hide-footer");
        }

        [Fact]
        public void ItShouldRenderWithNavMenu()
        {
            // Arrange
            var childId = "child-id";
            var childHtml = $"<strong id=\"{childId}\">child</strong>";

            using var ctx = BuildTestContext();

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

            using var ctx = BuildTestContext();

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

            using var ctx = BuildTestContext();

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

            using var ctx = BuildTestContext();

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

        [Fact]
        public async void ItShouldUpdateScreenSize()
        {
            // Arrange
            var resizeObserverServiceMock = SetupResizeObserverServiceMock(out var cbMap);

            using var ctx = BuildTestContext(resizeObserverServiceMock: resizeObserverServiceMock);

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>();
            var screenSizeBefore = cut.Instance.ScreenSize;

            // Simulate resize on component.
            await cbMap[cut.Instance.ElementReference.Id].ResizeAsync(100, 200).ConfigureAwait(false);

            // Assert
            screenSizeBefore.Height.Should().Be(0);
            screenSizeBefore.Width.Should().Be(0);

            var screenSize = cut.Instance.ScreenSize;
            screenSize.Width.Should().Be(100);
            screenSize.Height.Should().Be(200);
        }

        [Fact]
        public async void ItShouldUpdateScrollInfo()
        {
            // Arrange
            var scrollObserverServiceMock = SetupScrollObserverServiceMock(out var cbMap);

            using var ctx = BuildTestContext(scrollObserverServiceMock: scrollObserverServiceMock);

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>();
            var scrollInfoBefore = cut.Instance.ScrollInfo;

            // Simulate scroll on component.
            await cbMap[cut.Instance.ElementReference.Id].ScrollAsync(new ScrollInfo()
            {
                Left = 100,
                Top = 200,
            }).ConfigureAwait(false);

            // Assert
            scrollInfoBefore.Left.Should().Be(0);
            scrollInfoBefore.Top.Should().Be(0);

            var scrollInfo = cut.Instance.ScrollInfo;
            scrollInfo.Left.Should().Be(100);
            scrollInfo.Top.Should().Be(200);
        }

        [Theory]
        [InlineData(ResponsiveLayout.HeaderPanelId)]
        [InlineData(ResponsiveLayout.FooterPanelId)]
        public async void ItShouldRenderHeaderOrFooterWithHeightVariableInStyle(string id)
        {
            // Arrange
            var resizeObserverServiceMock = SetupResizeObserverServiceMock(out var cbMap);

            using var ctx = BuildTestContext(resizeObserverServiceMock: resizeObserverServiceMock);

            // Act
            var cut = ctx.RenderComponent<ResponsiveLayout>();

            (var eltRefId, var styleName) = id switch
            {
                ResponsiveLayout.HeaderPanelId => (cut.Instance.HeaderElementReference.Id, "--header-height"),
                ResponsiveLayout.FooterPanelId => (cut.Instance.FooterElementReference.Id, "--footer-height"),
                _ => throw new NotSupportedException(),
            };

            // Simulate resize on component.
            await cbMap[eltRefId].ResizeAsync(100, 200).ConfigureAwait(false);

            cut.Render();

            // Assert
            var element = cut.Find($"#{id}");

            var styles = StyleHelper.LoadStyleAttribute(element);
            styles.Should().Contain(x => x.Name == styleName).Which.Value.Should().Be("200px");

        }

        private static Mock<IScrollObserverService> SetupScrollObserverServiceMock(out Dictionary<string, IScrollCallback> cbMap)
        {
            var scrollObserverServiceMock = new Mock<IScrollObserverService>();

            var map = new Dictionary<string, IScrollCallback>();

            scrollObserverServiceMock
                .Setup(s => s.RegisterScrollCallbackAsync(It.IsAny<IScrollCallback>(), It.IsAny<ElementReference>()))
                .Callback(new InvocationAction((invocation) =>
                {
                    var cb = (IScrollCallback)invocation.Arguments[0];
                    var er = (ElementReference)invocation.Arguments[1];

                    map.Add(er.Id, cb);
                }));

            cbMap = map;

            return scrollObserverServiceMock;
        }

        private static Mock<IResizeObserverService> SetupResizeObserverServiceMock(out Dictionary<string, IResizeCallback> cbMap)
        {
            var resizeObserverServiceMock = new Mock<IResizeObserverService>();

            var map = new Dictionary<string, IResizeCallback>();

            resizeObserverServiceMock
                .Setup(s => s.RegisterResizeCallbackAsync(It.IsAny<IResizeCallback>(), It.IsAny<ElementReference>()))
                .Callback(new InvocationAction((invocation) =>
                {
                    var cb = (IResizeCallback)invocation.Arguments[0];
                    var er = (ElementReference)invocation.Arguments[1];

                    map.Add(er.Id, cb);
                }));

            cbMap = map;

            return resizeObserverServiceMock;
        }

        private static TestContext BuildTestContext(
            Mock<IResizeObserverService>? resizeObserverServiceMock = null,
            Mock<IScrollObserverService>? scrollObserverServiceMock = null,
            Mock<IResponsiveLayoutServiceInternal>? responsiveLayoutServiceInternalMock = null)
        {
            var ctx = new TestContext();

            ctx.Services.AddSingleton<IResizeObserverService>(resizeObserverServiceMock?.Object ?? Mock.Of<IResizeObserverService>());
            ctx.Services.AddSingleton<IScrollObserverService>(scrollObserverServiceMock?.Object ?? Mock.Of<IScrollObserverService>());
            ctx.Services.AddSingleton<IResponsiveLayoutServiceInternal>(responsiveLayoutServiceInternalMock?.Object ?? Mock.Of<IResponsiveLayoutServiceInternal>());
            return ctx;
        }
    }
}