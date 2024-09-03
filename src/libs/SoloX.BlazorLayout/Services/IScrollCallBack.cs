// ----------------------------------------------------------------------
// <copyright file="IScrollCallback.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.BlazorLayout.Core;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Services
{
    /// <summary>
    /// Asynchronous scroll callback interface.
    /// </summary>
    public interface IScrollCallback
    {
        /// <summary>
        /// Callback method if a scroll is detected.
        /// </summary>
        /// <param name="scrollInfo"></param>
        /// <returns>The asynchronous value task.</returns>
        ValueTask ScrollAsync(ScrollInfo scrollInfo);
    }
}
