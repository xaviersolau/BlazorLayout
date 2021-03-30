// ----------------------------------------------------------------------
// <copyright file="APanel.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Core
{
    /// <summary>
    /// Base abstract class for all Panel that can be used as container child.
    /// </summary>
    public abstract class APanel : ComponentBase
    {
        /// <summary>
        /// Gets/Sets Panel Id that is mapped to the associated HTML element id.
        /// </summary>
        [Parameter]
        public string? Id { get; set; }

        /// <summary>
        /// Gets/Sets Panel Class that is mapped to the associated HTML element
        /// class (in addition of the current panel implementation classes if any).
        /// </summary>
        [Parameter]
        public string? Class { get; set; }

        /// <summary>
        /// Gets/Sets Panel Child Content.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets/Sets (protected set) the Panel root HTML element reference.
        /// </summary>
        public ElementReference ElementReference { get; protected set; }
    }
}
