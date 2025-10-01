// ----------------------------------------------------------------------
// <copyright file="IScrollObserverService.cs" company="Xavier Solau">
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
    /// Scroll observer service.
    /// </summary>
    public interface IScrollObserverService
    {
        /// <summary>
        /// Register a scroll callback for the given element reference.
        /// </summary>
        /// <param name="scrollCallback">The scroll callback to trigger on size changed.</param>
        /// <param name="elementReference">The element reference to look after.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The asynchronous disposable that will unregister the callback once disposed.</returns>
        ValueTask<IAsyncDisposable> RegisterScrollCallbackAsync(
            IScrollCallback scrollCallback, ElementReference elementReference, CancellationToken cancellationToken);

        /// <summary>
        /// Scroll To the given scroll values.
        /// </summary>
        /// <param name="elementReference"></param>
        /// <param name="scrollLeft"></param>
        /// <param name="scrollTop"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        ValueTask ScrollToAsync(ElementReference elementReference, int? scrollLeft, int? scrollTop, CancellationToken cancellationToken);
    }
}
