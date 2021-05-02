// ----------------------------------------------------------------------
// <copyright file="ResizeContainer.razor.cs" company="Xavier Solau">
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
    /// Re-sizable Container.
    /// </summary>
    public partial class ResizeContainer
    {
        /// <summary>
        /// Gets/Sets optional re-sizable container min width.
        /// </summary>
        [Parameter]
        public string? MinWidth { get; set; }

        /// <summary>
        /// Gets/Sets optional re-sizable container min width.
        /// </summary>
        [Parameter]
        public string? MaxWidth { get; set; }

        /// <summary>
        /// Gets/Sets optional re-sizable container min width.
        /// </summary>
        [Parameter]
        public string? MinHeight { get; set; }

        /// <summary>
        /// Gets/Sets optional re-sizable container min width.
        /// </summary>
        [Parameter]
        public string? MaxHeight { get; set; }

        private string ComputeStyle()
        {
            return $"{StyleMinMaxSize} {Style}";
        }

        private string FillClass =>
            Fill switch
            {
                Fill.None => "container-size-content",
                Fill.Full => "container-size-100",
                Fill.Vertical => "container-height-100",
                Fill.Horizontal => "container-width-100",
                _ => throw new ArgumentOutOfRangeException($"unexpected argument {nameof(Fill)}"),
            };

        private string StyleMinMaxSize
        {
            get
            {
                if (string.IsNullOrEmpty(MinWidth)
                    || string.IsNullOrEmpty(MaxWidth)
                    || string.IsNullOrEmpty(MinHeight)
                    || string.IsNullOrEmpty(MaxHeight))
                {
                    var minWidth = string.IsNullOrEmpty(MinWidth) ? string.Empty : $"min-width:{MinWidth};";
                    var maxWidth = string.IsNullOrEmpty(MaxWidth) ? string.Empty : $"max-width:{MaxWidth};";
                    var minHeight = string.IsNullOrEmpty(MinWidth) ? string.Empty : $"min-width:{MinHeight};";
                    var maxHeight = string.IsNullOrEmpty(MaxWidth) ? string.Empty : $"max-width:{MaxHeight};";
                    return $"{minWidth}{maxWidth}{minHeight}{maxHeight}";
                }

                return string.Empty;
            }
        }
    }
}
