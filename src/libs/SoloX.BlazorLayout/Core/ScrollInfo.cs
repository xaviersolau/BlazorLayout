// ----------------------------------------------------------------------
// <copyright file="ScrollInfo.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.BlazorLayout.Core
{
    /// <summary>
    /// Scroll info object.
    /// </summary>
    public class ScrollInfo
    {
        /// <summary>
        /// Scroll total width.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Scroll value from the left.
        /// </summary>
        public int Left { get; internal set; }

        /// <summary>
        /// Scroll view width.
        /// </summary>
        public int ViewWidth { get; internal set; }

        /// <summary>
        /// Scroll total height.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Scroll value from the top.
        /// </summary>
        public int Top { get; internal set; }

        /// <summary>
        /// Scroll view height.
        /// </summary>
        public int ViewHeight { get; internal set; }
    }
}
