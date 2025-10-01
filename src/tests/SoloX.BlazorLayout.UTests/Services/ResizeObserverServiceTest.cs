// ----------------------------------------------------------------------
// <copyright file="ResizeObserverServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.Services.Impl;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static SoloX.BlazorLayout.Services.Impl.ResizeObserverService;

namespace SoloX.BlazorLayout.UTests.Services
{
    public class ResizeObserverServiceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ResizeObserverServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceRegisterAndUnRegisterResizeCallbackAsync()
        {
            var callbackMock = new Mock<IResizeCallback>();

            await using var service = SetupResizeObserverService(out var jsObjectReferenceMock);

            var eltRef = new ElementReference("id");

            var disposable = await service.RegisterResizeCallbackAsync(callbackMock.Object, eltRef, CancellationToken.None);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.RegisterResizeCallback,
                    It.Is<object?[]?>(a => MatchResizeCallbackRegister(a, eltRef, callbackMock.Object))),
                Times.Once);
            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterResizeCallback,
                    It.IsAny<CancellationToken>(), It.IsAny<object?[]?>()),
                Times.Never);

            await disposable.DisposeAsync();

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterResizeCallback,
                    It.IsAny<CancellationToken>(),
                    It.Is<object?[]?>(a => MatchUnRegister(a, eltRef))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceRegisterAndUnRegisterMutationObserverAsync()
        {
            await using var service = SetupResizeObserverService(out var jsObjectReferenceMock);

            var eltRef = new ElementReference("id");

            var disposable = await service.RegisterMutationObserverAsync(eltRef, CancellationToken.None);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.RegisterMutationObserver,
                    It.Is<object?[]?>(a => MatchMutationObserverRegister(a, eltRef))),
                Times.Once);
            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterMutationObserver,
                    It.IsAny<CancellationToken>(), It.IsAny<object?[]?>()),
                Times.Never);

            await disposable.DisposeAsync();

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterMutationObserver,
                    It.IsAny<CancellationToken>(),
                    It.Is<object?[]?>(a => MatchUnRegister(a, eltRef))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceProcessCallbackAsync()
        {
            await using var service = SetupResizeObserverService(out var jsObjectReferenceMock);

            await service.TriggerCallbackAsync(CancellationToken.None);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.ProcessCallbackReferences,
                    It.Is<object?[]?>(a => MatchProcessCallback(a))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldForwardTheResizeCallAsync()
        {
            var callbackMock = new Mock<IResizeCallback>();

            var proxy = new ResizeCallbackProxy(callbackMock.Object);

            proxy.SizeCallback.Should().BeSameAs(callbackMock.Object);

            await proxy.ResizeAsync(1, 2);

            callbackMock.Verify(
                cb => cb.ResizeAsync(1, 2),
                Times.Once);
        }

        private ResizeObserverService SetupResizeObserverService(
            out Mock<IJSObjectReference> jsObjectReferenceMock)
        {
            var jsRuntimeMock = new Mock<IJSRuntime>();
            jsObjectReferenceMock = new Mock<IJSObjectReference>();

            var optionsMock = new Mock<IOptions<BlazorLayoutOptions>>();

            optionsMock.SetupGet(o => o.Value).Returns(new BlazorLayoutOptions());

            jsRuntimeMock
                .Setup(r => r.InvokeAsync<IJSObjectReference>(
                    ResizeObserverService.Import,
                    new object[] { ResizeObserverService.ResizeObserverJsInteropFile }))
                .ReturnsAsync(jsObjectReferenceMock.Object);

            var service = new ResizeObserverService(
                optionsMock.Object,
                jsRuntimeMock.Object,
                new TestLogger<ResizeObserverService>(this.testOutputHelper));
            return service;
        }

        private static bool MatchResizeCallbackRegister(object?[]? args, ElementReference expectedEltRef, IResizeCallback sizeCallback)
        {
            if (args != null && args.Length == 3
                && args[0] is DotNetObjectReference<ResizeCallbackProxy> callbackRef
                && args[1] is string id
                && args[2] is ElementReference eltRef)
            {
                return expectedEltRef.Id == eltRef.Id
                    && id == expectedEltRef.Id
                    && object.ReferenceEquals(callbackRef.Value.SizeCallback, sizeCallback);
            }

            return false;
        }

        private static bool MatchMutationObserverRegister(object?[]? args, ElementReference expectedEltRef)
        {
            if (args != null && args.Length == 2
                && args[0] is string id
                && args[1] is ElementReference eltRef)
            {
                return expectedEltRef.Id == eltRef.Id
                    && id == expectedEltRef.Id;
            }

            return false;
        }

        private static bool MatchProcessCallback(object?[]? args)
        {
            if (args != null && args.Length == 0)
            {
                return true;
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
