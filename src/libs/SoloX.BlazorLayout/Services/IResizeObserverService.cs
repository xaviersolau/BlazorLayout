// ----------------------------------------------------------------------
// <copyright file="IResizeObserverService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services
{
    /// <summary>
    /// Resize observer service.
    /// </summary>
    public interface IResizeObserverService
    {
        /// <summary>
        /// Register a resize callback for the given element reference.
        /// </summary>
        /// <param name="sizeCallback">The resize callback to trigger on size changed.</param>
        /// <param name="elementReference">The element reference to look after.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The asynchronous disposable that will unregister the callback once disposed.</returns>
        ValueTask<IAsyncDisposable> RegisterResizeCallbackAsync(
            IResizeCallback sizeCallback,
            ElementReference elementReference,
            CancellationToken cancellationToken);

        /// <summary>
        /// Register a mutation observer on the given mutable element reference.
        /// </summary>
        /// <param name="elementReference">The reference of the element subject to change layout sizing.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The asynchronous disposable that will unregister the observer once disposed.</returns>
        ValueTask<IAsyncDisposable> RegisterMutationObserverAsync(
            ElementReference elementReference,
            CancellationToken cancellationToken);

        /// <summary>
        /// Manually trigger the registered callbacks.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The asynchronous value task.</returns>
        ValueTask TriggerCallbackAsync(CancellationToken cancellationToken);
    }
}
