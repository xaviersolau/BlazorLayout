// ----------------------------------------------------------------------
// <copyright file="IResizeCallBack.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services
{
    /// <summary>
    /// Asynchronous resize callback interface.
    /// </summary>
    public interface IResizeCallBack
    {
        /// <summary>
        /// Callback method if a resize is detected.
        /// </summary>
        /// <param name="width">Detected width.</param>
        /// <param name="height">Detected height.</param>
        /// <returns>The asynchronous value task.</returns>
        ValueTask ResizeAsync(int width, int height);
    }
}
