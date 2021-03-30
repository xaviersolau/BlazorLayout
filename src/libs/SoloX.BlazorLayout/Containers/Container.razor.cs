// ----------------------------------------------------------------------
// <copyright file="Container.razor.cs" company="Xavier Solau">
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
    /// Simple container component.
    /// </summary>
    public partial class Container
    {
        private string FillClass =>
            Fill switch
            {
                Fill.None => "container-size-content",
                Fill.Full => "container-size-100",
                Fill.Vertical => "container-height-100",
                Fill.Horizontal => "container-width-100",
                _ => throw new ArgumentOutOfRangeException($"unexpected argument {nameof(Fill)}"),
            };
    }
}
