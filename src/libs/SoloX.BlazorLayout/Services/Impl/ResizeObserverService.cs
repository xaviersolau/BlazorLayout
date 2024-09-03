// ----------------------------------------------------------------------
// <copyright file="ResizeObserverService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services.Impl
{
    /// <summary>
    /// Resize observer service implementation.
    /// </summary>
    public class ResizeObserverService : IResizeObserverService, IAsyncDisposable
    {
        internal const string RegisterResizeCallback = "resizeManager.registerResizeCallback";
        internal const string UnregisterResizeCallback = "resizeManager.unregisterResizeCallback";
        internal const string RegisterMutationObserver = "resizeManager.registerMutationObserver";
        internal const string UnregisterMutationObserver = "resizeManager.unregisterMutationObserver";
        internal const string ProcessCallbackReferences = "resizeManager.processCallbackReferences";
        internal const string Ping = "resizeManager.ping";
        internal const string SetupModule = "resizeManager.setupModule";
        internal const string Import = "import";
        internal const string ResizeObserverJsInteropFile = "./_content/SoloX.BlazorLayout/resizeObserverJsInterop.js";

        private readonly BlazorLayoutOptions options;
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private readonly Dictionary<string, AsyncDisposable> disposables =
            new Dictionary<string, AsyncDisposable>();

        private readonly ILogger<ResizeObserverService> logger;

        /// <summary>
        /// Setup the service with the given jsRuntime interface.
        /// </summary>
        /// <param name="options">Service options.</param>
        /// <param name="jsRuntime">The JS runtime to interact with JS sandbox.</param>
        /// <param name="logger">The logger where to log messages.</param>
        public ResizeObserverService(IOptions<BlazorLayoutOptions> options, IJSRuntime jsRuntime, ILogger<ResizeObserverService> logger)
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
                   ResizeObserverJsInteropFile).ConfigureAwait(false);

                await module.InvokeVoidAsync(
                    SetupModule,
                    this.options.EnableJsModuleLogs,
                    this.options.ResizeCallbackDelay,
                    this.options.EnableResizeEventBurstBoxingCallback).ConfigureAwait(false);

                return module;
            });

            this.logger = logger;
        }

        ///<inheritdoc/>
        public async ValueTask<IAsyncDisposable> RegisterResizeCallbackAsync(
            IResizeCallback sizeCallback, ElementReference elementReference)
        {
            var module = await this.moduleTask.Value.ConfigureAwait(false);

            var objectRef = DotNetObjectReference.Create(new ResizeCallbackProxy(sizeCallback));

            await module.InvokeVoidAsync(RegisterResizeCallback,
                objectRef, elementReference.Id, elementReference).ConfigureAwait(false);

            var id = $"{nameof(RegisterResizeCallbackAsync)}-{elementReference.Id}";

            var disposable = new AsyncDisposable(
                id,
                async () =>
                {
                    this.disposables.Remove(id);

                    try
                    {
                        await module.InvokeVoidAsync(UnregisterResizeCallback,
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

        ///<inheritdoc/>
        public async ValueTask<IAsyncDisposable> RegisterMutationObserverAsync(
            ElementReference elementReference)
        {
            var module = await this.moduleTask.Value.ConfigureAwait(false);

            await module.InvokeVoidAsync(RegisterMutationObserver,
                elementReference.Id, elementReference).ConfigureAwait(false);

            var id = $"{nameof(RegisterMutationObserverAsync)}-{elementReference.Id}";

            var disposable = new AsyncDisposable(
                id,
                async () =>
                {
                    this.disposables.Remove(id);

                    try
                    {
                        await module.InvokeVoidAsync(UnregisterMutationObserver,
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
                        this.logger.LogDebug(e.Message);
                    }
                });

            this.disposables.Add(id, disposable);
            return disposable;
        }

        ///<inheritdoc/>
        public async ValueTask TriggerCallbackAsync()
        {
            var module = await this.moduleTask.Value.ConfigureAwait(false);

            await module.InvokeVoidAsync(ProcessCallbackReferences).ConfigureAwait(false);
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

        internal class ResizeCallbackProxy : IResizeCallback
        {
            internal IResizeCallback SizeCallback { get; }

            public ResizeCallbackProxy(IResizeCallback sizeCallback)
            {
                SizeCallback = sizeCallback;
            }

            [JSInvokable]
            public ValueTask ResizeAsync(int width, int height)
            {
                return SizeCallback.ResizeAsync(width, height);
            }
        }
    }
}
