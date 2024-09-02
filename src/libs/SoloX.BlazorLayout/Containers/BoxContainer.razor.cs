// ----------------------------------------------------------------------
// <copyright file="BoxContainer.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;

namespace SoloX.BlazorLayout.Containers
{
    /// <summary>
    /// Simple container component.
    /// </summary>
    public partial class BoxContainer
    {
        /// <summary>
        /// Display the box container on the given DisplaySize.
        /// </summary>
        [Parameter]
        public DisplaySize DisplayOn { get; set; } = DisplaySize.All;

        private string FillClass =>
            Fill switch
            {
                Fill.None => "container-size-content",
                Fill.Full => "container-size-100",
                Fill.Vertical => "container-height-100",
                Fill.Horizontal => "container-width-100",
                _ => throw new ArgumentOutOfRangeException($"unexpected argument {nameof(Fill)}"),
            };

        private string DisplayClass =>
            DisplayOn switch
            {
                DisplaySize.None => "no-display",
                DisplaySize.All => string.Empty,
                DisplaySize.Small => "display-on-small",
                DisplaySize.Large => "display-on-large",
                DisplaySize.VeryLarge => "display-on-very-large",
                DisplaySize.VeryVeryLarge => "display-on-very-very-large",

                DisplaySize.Small | DisplaySize.Large => "display-from-small-to-large",
                DisplaySize.Small | DisplaySize.Large | DisplaySize.VeryLarge => "display-from-small-to-very-large",
                DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge => "display-from-very-large-to-very-very-large",
                DisplaySize.Large | DisplaySize.VeryLarge | DisplaySize.VeryVeryLarge => "display-from-large-to-very-very-large",

                _ => throw new NotSupportedException("DisplaySize must be single or must be consecutive values starting by Small or ending by VeryVeryLarge"),
            };
    }

    /// <summary>
    /// Display size.
    /// </summary>
    [Flags]
    public enum DisplaySize
    {
        /// <summary>
        /// No display.
        /// </summary>
        None = 0,
        /// <summary>
        /// Small sizes.
        /// </summary>
        Small = 1,
        /// <summary>
        /// Large sizes.
        /// </summary>
        Large = 2,
        /// <summary>
        /// Very large sizes.
        /// </summary>
        VeryLarge = 4,
        /// <summary>
        /// Very very large sizes.
        /// </summary>
        VeryVeryLarge = 8,
        /// <summary>
        /// All sizes.
        /// </summary>
        All = Small | Large | VeryLarge | VeryVeryLarge,
    }
}
