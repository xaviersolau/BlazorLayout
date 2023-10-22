// ----------------------------------------------------------------------
// <copyright file="ResizeObserverServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.AspNetCore.Components;
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

namespace SoloX.BlazorLayout.UTest.Services
{
    public class ResizeObserverServiceTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ResizeObserverServiceTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceRegisterAndUnRegisterResizeCallBackAsync()
        {
            var callbackMock = new Mock<IResizeCallBack>();

            var service = SetupResizeObserverService(out var jsObjectReferenceMock);
            await using var _ = service.ConfigureAwait(false);

            var eltRef = new ElementReference("id");

            var disposable = await service.RegisterResizeCallBackAsync(callbackMock.Object, eltRef).ConfigureAwait(false);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.RegisterResizeCallBack,
                    It.Is<object?[]?>(a => MatchResizeCallBackRegister(a, eltRef, callbackMock.Object))),
                Times.Once);
            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterResizeCallBack,
                    It.IsAny<CancellationToken>(), It.IsAny<object?[]?>()),
                Times.Never);

            await disposable.DisposeAsync().ConfigureAwait(false);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterResizeCallBack,
                    It.IsAny<CancellationToken>(),
                    It.Is<object?[]?>(a => MatchUnRegister(a, eltRef))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceRegisterAndUnRegisterMutationObserverAsync()
        {
            var service = SetupResizeObserverService(out var jsObjectReferenceMock);
            await using var _ = service.ConfigureAwait(false);

            var eltRef = new ElementReference("id");

            var disposable = await service.RegisterMutationObserverAsync(eltRef).ConfigureAwait(false);

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

            await disposable.DisposeAsync().ConfigureAwait(false);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.UnregisterMutationObserver,
                    It.IsAny<CancellationToken>(),
                    It.Is<object?[]?>(a => MatchUnRegister(a, eltRef))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldCallJSObjectReferenceProcessCallBackAsync()
        {
            var service = SetupResizeObserverService(out var jsObjectReferenceMock);
            await using var _ = service.ConfigureAwait(false);

            await service.TriggerCallBackAsync().ConfigureAwait(false);

            jsObjectReferenceMock.Verify(
                r => r.InvokeAsync<object>(
                    ResizeObserverService.ProcessCallbackReferences,
                    It.Is<object?[]?>(a => MatchProcessCallback(a))),
                Times.Once);
        }

        [Fact]
        public async Task ItShouldForwardTheResizeCallAsync()
        {
            var callBackMock = new Mock<IResizeCallBack>();

            var proxy = new SizeCallBackProxy(callBackMock.Object);

            proxy.SizeCallBack.Should().BeSameAs(callBackMock.Object);

            await proxy.ResizeAsync(1, 2).ConfigureAwait(false);

            callBackMock.Verify(
                cb => cb.ResizeAsync(1, 2),
                Times.Once);
        }

        private ResizeObserverService SetupResizeObserverService(
            out Mock<IJSObjectReference> jsObjectReferenceMock)
        {
            var jsRuntimeMock = new Mock<IJSRuntime>();
            jsObjectReferenceMock = new Mock<IJSObjectReference>();

            jsRuntimeMock
                .Setup(r => r.InvokeAsync<IJSObjectReference>(
                    ResizeObserverService.Import,
                    new object[] { ResizeObserverService.SizeObserverJsInteropFile }))
                .ReturnsAsync(jsObjectReferenceMock.Object);

            var service = new ResizeObserverService(
                            jsRuntimeMock.Object,
                            new TestLogger<ResizeObserverService>(this.testOutputHelper));
            return service;
        }

        private static bool MatchResizeCallBackRegister(object?[]? args, ElementReference expectedEltRef, IResizeCallBack sizeCallBack)
        {
            if (args != null && args.Length == 3
                && args[0] is DotNetObjectReference<SizeCallBackProxy> callBackRef
                && args[1] is string id
                && args[2] is ElementReference eltRef)
            {
                return expectedEltRef.Id == eltRef.Id
                    && id == expectedEltRef.Id
                    && object.ReferenceEquals(callBackRef.Value.SizeCallBack, sizeCallBack);
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
