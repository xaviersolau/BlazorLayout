﻿// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayout.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace SoloX.BlazorLayout.Layouts
{
    /// <summary>
    /// Responsive main page implementation.
    /// </summary>
    public partial class ResponsiveLayout
    {
        internal const string NavigationPanelId = "navigation-panel-id";
        internal const string HeaderPanelId = "header-panel-id";
        internal const string OutlinePanelId = "outline-panel-id";
        internal const string ChildContentPanelId = "child-content-panel-id";
        internal const string FooterPanelId = "footer-panel-id";

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
        /// Style to use in the navigation panel.
        /// </summary>
        [Parameter]
        public string? NavigationStyle { get; set; }

        /// <summary>
        /// Navigation panel header.
        /// </summary>
        [Parameter]
        public RenderFragment? NavigationHeader { get; set; }

        /// <summary>
        /// Navigation panel body.
        /// </summary>
        [Parameter]
        public RenderFragment? NavigationMenu { get; set; }

        /// <summary>
        /// Small navigation panel header.
        /// </summary>
        [Parameter]
        public RenderFragment? SmallNavigationHeader { get; set; }

        /// <summary>
        /// Small navigation panel body.
        /// </summary>
        [Parameter]
        public RenderFragment? SmallNavigationMenu { get; set; }

        /// <summary>
        /// Use Small navigation.
        /// </summary>
        [Parameter]
        public bool UseSmallNavigation { get; set; }

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
