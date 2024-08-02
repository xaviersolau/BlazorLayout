// ----------------------------------------------------------------------
// <copyright file="ScreenSize.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.BlazorLayout.Layouts
{
    /// <summary>
    /// ScreenSize
    /// </summary>
    public class ScreenSize
    {
        /// <summary>
        /// Screen width.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Screen height.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Is Small size.
        /// </summary>
        public bool IsSmall => Width <= 650;

        /// <summary>
        /// Is Large size.
        /// </summary>
        public bool IsLarge => Width > 650 && !IsVeryLarge;

        /// <summary>
        /// Is Very Large size.
        /// </summary>
        public bool IsVeryLarge => Width > 900;
    }
}
