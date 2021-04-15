// ----------------------------------------------------------------------
// <copyright file="ResponsiveMainPage.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Pages
{
    /// <summary>
    /// Responsive main page implementation.
    /// </summary>
    public partial class ResponsiveMainPage
    {
        /// <summary>
        /// Max proportion of the page side elements.
        /// </summary>
        [Parameter]
        public int MaxProportion { get; set; } = 25;

        /// <summary>
        /// Class to use in the navigation panel.
        /// </summary>
        [Parameter]
        public string? NavigationClass { get; set; }

        /// <summary>
        /// Navigation panel header.
        /// </summary>
        [Parameter]
        public RenderFragment? NavigationHeader { get; set; }

        /// <summary>
        /// Navigation panel body.
        /// </summary>
        [Parameter]
        public RenderFragment? Navigation { get; set; }

        /// <summary>
        /// Header child.
        /// </summary>
        [Parameter]
        public RenderFragment? Header { get; set; }

        /// <summary>
        /// Outline child.
        /// </summary>
        /// <remarks>
        /// The outline panel may be hidden depending on the view port size.
        /// </remarks>
        [Parameter]
        public RenderFragment? Outline { get; set; }

        /// <summary>
        /// Footer child.
        /// </summary>
        [Parameter]
        public RenderFragment? Footer { get; set; }

        /// <summary>
        /// Body page child content
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Enable/disable content scrolling:
        /// * if enabled the scroll bar will be in the child panels and the page will fill the view port.
        /// * if disabled the scroll bar will be on the full page.
        /// </summary>
        [Parameter]
        public bool EnableContentScroll { get; set; }

        /// <summary>
        /// Disable Outline (default is true).
        /// </summary>
        [Parameter]
        public bool EnableOutline { get; set; }

        private string ClassContentOverflow
            => EnableContentScroll ? "content-overflow" : string.Empty;
    }
}
