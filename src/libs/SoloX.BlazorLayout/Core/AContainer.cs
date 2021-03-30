// ----------------------------------------------------------------------
// <copyright file="AContainer.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Core
{
    /// <summary>
    /// Base abstract class for a Container.
    /// </summary>
    public abstract class AContainer : APanel
    {
        /// <summary>
        /// Gets/Sets how the container is supposed to fill its parent area.
        /// </summary>
        [Parameter]
        public Fill Fill { get; set; }
    }
}
