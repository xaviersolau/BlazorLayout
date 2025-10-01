// ----------------------------------------------------------------------
// <copyright file="ScrollObserverServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Moq;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.Services.Impl;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;
using static SoloX.BlazorLayout.Services.Impl.ScrollObserverService;

namespace SoloX.BlazorLayout.UTests.Services
{
    public class ScrollObserverServiceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ScrollObserverServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceRegisterAndUnRegisterScrollCallbackAsync()
        {
            var callbackMock = new Mock<IScrollCallback>();

            await using var service = SetupScrollObserverService(out var jsObjectReferenceMock);

            var eltRef = new ElementReference("id");

            var disposable = await service.RegisterScrollCallbackAsync(callbackMock.Object, eltRef, CancellationToken.None);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ScrollObserverService.RegisterScrollCallback,
                    It.Is<object?[]?>(a => MatchScrollCallbackRegister(a, eltRef, callbackMock.Object))),
                Times.Once);
            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ScrollObserverService.UnregisterScrollCallback,
                    It.IsAny<CancellationToken>(), It.IsAny<object?[]?>()),
                Times.Never);

            await disposable.DisposeAsync();

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ScrollObserverService.UnregisterScrollCallback,
                    It.IsAny<CancellationToken>(),
                    It.Is<object?[]?>(a => MatchUnRegister(a, eltRef))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceScrollToAsync()
        {
            var left = 333;
            var top = 444;
            await using var service = SetupScrollObserverService(out var jsObjectReferenceMock);

            var eltRef = new ElementReference("id");

            await service.ScrollToAsync(eltRef, left, top, CancellationToken.None);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<IJSVoidResult>(
                    ScrollObserverService.ScrollTo,
                    It.Is<object?[]?>(a => MatchScrollTo(a, eltRef, left, top))),
                Times.Once);
        }

        private static bool MatchScrollTo(object?[]? args, ElementReference expectedEltRef, int expectedLeft, int expectedTop)
        {
            if (args != null && args.Length == 3
                && args[0] is ElementReference eltRef
                && args[1] is int left
                && args[2] is int top)
            {
                return expectedEltRef.Id == eltRef.Id
                    && expectedLeft == left
                    && expectedTop == top;
            }

            return false;
        }

        [Fact]
        public async Task ItShouldForwardTheScrollCallAsync()
        {
            var callbackMock = new Mock<IScrollCallback>();

            var proxy = new ScrollCallbackProxy(callbackMock.Object);

            proxy.ScrollCallback.Should().BeSameAs(callbackMock.Object);

            await proxy.ScrollAsync(1, 2, 3, 4, 5, 6);

            callbackMock.Verify(
                cb => cb.ScrollAsync(It.Is<ScrollInfo>(si => si.Width == 1 && si.Left == 2 && si.ViewWidth == 3 && si.Height == 4 && si.Top == 5 && si.ViewHeight == 6)),
                Times.Once);
        }

        private ScrollObserverService SetupScrollObserverService(
            out Mock<IJSObjectReference> jsObjectReferenceMock)
        {
            var jsRuntimeMock = new Mock<IJSRuntime>();
            jsObjectReferenceMock = new Mock<IJSObjectReference>();

            var optionsMock = new Mock<IOptions<BlazorLayoutOptions>>();

            optionsMock.SetupGet(o => o.Value).Returns(new BlazorLayoutOptions());

            jsRuntimeMock
                .Setup(r => r.InvokeAsync<IJSObjectReference>(
                    ScrollObserverService.Import,
                    new object[] { ScrollObserverService.ScrollObserverJsInteropFile }))
                .ReturnsAsync(jsObjectReferenceMock.Object);

            var service = new ScrollObserverService(
                optionsMock.Object,
                jsRuntimeMock.Object,
                new TestLogger<ScrollObserverService>(this.testOutputHelper));
            return service;
        }

        private static bool MatchScrollCallbackRegister(object?[]? args, ElementReference expectedEltRef, IScrollCallback scrollCallback)
        {
            if (args != null && args.Length == 3
                && args[0] is DotNetObjectReference<ScrollCallbackProxy> callbackRef
                && args[1] is string id
                && args[2] is ElementReference eltRef)
            {
                return expectedEltRef.Id == eltRef.Id
                    && id == expectedEltRef.Id
                    && object.ReferenceEquals(callbackRef.Value.ScrollCallback, scrollCallback);
            }

            return false;
        }

        private static bool MatchUnRegister(object?[]? args, ElementReference expectedEltRef)
        {
            if (args != null && args.Length == 1
                && args[0] is string id)
            {
                return expectedEltRef.Id == id;
            }

            return false;
        }
    }
}
