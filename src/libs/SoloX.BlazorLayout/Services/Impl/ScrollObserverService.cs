// ----------------------------------------------------------------------
// <copyright file="ScrollObserverService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SoloX.BlazorLayout.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services.Impl
{
    /// <summary>
    /// Scroll observer service implementation.
    /// </summary>
    public class ScrollObserverService : IScrollObserverService, IAsyncDisposable
    {
        internal const string RegisterScrollCallback = "scrollManager.registerScrollCallback";
        internal const string UnregisterScrollCallback = "scrollManager.unregisterScrollCallback";
        internal const string ScrollTo = "scrollManager.scrollTo";
        internal const string Ping = "scrollManager.ping";
        internal const string SetupModule = "scrollManager.setupModule";
        internal const string Import = "import";
        internal const string ScrollObserverJsInteropFile = "./_content/SoloX.BlazorLayout/scrollObserverJsInterop.js";

        private readonly BlazorLayoutOptions options;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private readonly Dictionary<string, AsyncDisposable> disposables =
            new Dictionary<string, AsyncDisposable>();

        private readonly ILogger<ScrollObserverService> logger;

        /// <summary>
        /// Setup the service with the given jsRuntime interface.
        /// </summary>
        /// <param name="options">Service options.</param>
        /// <param name="jsRuntime">The JS runtime to interact with JS sandbox.</param>
        /// <param name="logger">The logger where to log messages.</param>
        public ScrollObserverService(IOptions<BlazorLayoutOptions> options, IJSRuntime jsRuntime, ILogger<ScrollObserverService> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options.Value;

            // Setup lazy loading of the JS size observer module.
            this.moduleTask = new(async () =>
            {
                var module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                   Import,
                   ScrollObserverJsInteropFile).ConfigureAwait(false);

                await module.InvokeVoidAsync(
                    SetupModule,
                    this.options.EnableJsModuleLogs,
                    this.options.ScrollCallbackDelay,
                    this.options.EnableScrollEventBurstBoxingCallback).ConfigureAwait(false);

                return module;
            });

            this.logger = logger;
        }

        ///<inheritdoc/>
        public async ValueTask<IAsyncDisposable> RegisterScrollCallbackAsync(IScrollCallback scrollCallback, ElementReference elementReference)
        {
            var module = await this.moduleTask.Value.ConfigureAwait(false);

            var objectRef = DotNetObjectReference.Create(new ScrollCallbackProxy(scrollCallback));

            await module.InvokeVoidAsync(RegisterScrollCallback,
                objectRef, elementReference.Id, elementReference).ConfigureAwait(false);

            var id = $"{nameof(RegisterScrollCallbackAsync)}-{elementReference.Id}";

            var disposable = new AsyncDisposable(
                id,
                async () =>
                {
                    this.disposables.Remove(id);

                    try
                    {
                        await module.InvokeVoidAsync(UnregisterScrollCallback,
                            TimeSpan.FromMilliseconds(500),
                            elementReference.Id).ConfigureAwait(false);
                    }
#if NET6_0_OR_GREATER
                    catch (JSDisconnectedException e)
                    {
                        this.logger.LogDebug(e, e.Message);
                    }
#endif
                    catch (TaskCanceledException e)
                    {
                        this.logger.LogDebug(e, e.Message);
                    }

                    objectRef.Dispose();
                });

            this.disposables.Add(id, disposable);
            return disposable;
        }

        /// <inheritdoc/>
        public async ValueTask ScrollToAsync(ElementReference elementReference, int? scrollLeft, int? scrollTop)
        {
            var module = await this.moduleTask.Value.ConfigureAwait(false);

            await module.InvokeVoidAsync(ScrollTo, elementReference, scrollLeft, scrollTop).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            foreach (var item in this.disposables.Values.ToArray())
            {
                await item.DisposeAsync().ConfigureAwait(false);
            }

            if (this.moduleTask.IsValueCreated)
            {
                var module = await this.moduleTask.Value.ConfigureAwait(false);

                try
                {
                    // make sure JS runtime is steel responding otherwise disposing the module may block forever.
                    await module.InvokeVoidAsync(Ping,
                        TimeSpan.FromMilliseconds(500)).ConfigureAwait(false);

                    await module.DisposeAsync().ConfigureAwait(false);
                }
#if NET6_0_OR_GREATER
                catch (JSDisconnectedException e)
                {
                    this.logger.LogDebug(e, e.Message);
                }
#endif
                catch (TaskCanceledException e)
                {
                    this.logger.LogDebug(e.Message);
                }
            }

#pragma warning disable CA1816 // Les méthodes Dispose doivent appeler SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Les méthodes Dispose doivent appeler SuppressFinalize
        }

        internal class ScrollCallbackProxy
        {
            internal IScrollCallback ScrollCallback { get; }

            public ScrollCallbackProxy(IScrollCallback scrollCallback)
            {
                ScrollCallback = scrollCallback;
            }

            [JSInvokable]
            public ValueTask ScrollAsync(int width, int left, int viewWidth, int height, int top, int viewHeight)
            {
                var scrollInfo = new ScrollInfo()
                {
                    Width = width,
                    Left = left,
                    ViewWidth = viewWidth,
                    Height = height,
                    Top = top,
                    ViewHeight = viewHeight,
                };

                return ScrollCallback.ScrollAsync(scrollInfo);
            }
        }
    }
}
