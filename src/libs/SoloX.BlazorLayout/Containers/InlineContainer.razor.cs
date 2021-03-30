// ----------------------------------------------------------------------
// <copyright file="InlineContainer.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.BlazorLayout.Core;

namespace SoloX.BlazorLayout.Containers
{
    /// <summary>
    /// In Line Container.
    /// </summary>
    public partial class InlineContainer
    {
        private string FillClass =>
            Fill switch
            {
                Fill.None => "inline-container container-size-content",
                Fill.Full => "inline-container container-size-100",
                Fill.Vertical => "inline-container container-height-100",
                Fill.Horizontal => "inline-container container-width-100",
                _ => throw new ArgumentOutOfRangeException($"unexpected argument {nameof(Fill)}"),
            };
    }
}
