// ----------------------------------------------------------------------
// <copyright file="Fill.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.BlazorLayout.Core
{
    /// <summary>
    /// Flags to define how a container must fill its parent space.
    /// </summary>
    [Flags]
    public enum Fill
    {
        /// <summary>
        /// Fill according to the content size.
        /// </summary>
        None = 0,

        /// <summary>
        /// Fill horizontally the parent space.
        /// </summary>
        Horizontal = 1,

        /// <summary>
        /// Fill vertically the parent space.
        /// </summary>
        Vertical = 2,

        /// <summary>
        /// Fill both horizontally and vertically the parent space.
        /// </summary>
        Full = Horizontal | Vertical,
    }
}
