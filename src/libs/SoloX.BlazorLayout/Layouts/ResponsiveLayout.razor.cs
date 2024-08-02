﻿// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayout.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Services;
using System;
using System.Threading.Tasks;

namespace SoloX.BlazorLayout.Layouts
{
    /// <summary>
    /// Responsive main page implementation.
    /// </summary>
    public partial class ResponsiveLayout : IAsyncDisposable
    {
        internal const string NavigationPanelId = "navigation-panel-id";
        internal const string HeaderPanelId = "header-panel-id";
        internal const string OutlinePanelId = "outline-panel-id";
        internal const string ChildContentPanelId = "child-content-panel-id";
        internal const string FooterPanelId = "footer-panel-id";

        private IAsyncDisposable? rootCallBackDisposable;
        private IAsyncDisposable? headerCallBackDisposable;
        private IAsyncDisposable? footerCallBackDisposable;
        private IAsyncDisposable? rootScrollCallBackDisposable;

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
        /// Hide/Show Header.
        /// </summary>
        [Parameter]
        public bool HideHeader { get; set; }

        /// <summary>
        /// Hide/Show Footer.
        /// </summary>
        [Parameter]
        public bool HideFooter { get; set; }

        /// <summary>
        /// ResizeObserverService to handle root footer and header size observation.
        /// </summary>
        [Inject]
        public IResizeObserverService ResizeObserverService { get; set; } = default!;

        /// <summary>
        /// ScrollObserverService to handle main view scroll.
        /// </summary>
        [Inject]
        public IScrollObserverService ScrollObserverService { get; set; } = default!;

        /// <summary>
        /// Gets/Sets (protected set) the Panel root HTML element reference.
        /// </summary>
        public ElementReference ElementReference { get; private set; }

        /// <summary>
        /// Gets/Sets (protected set) the Panel root HTML element reference.
        /// </summary>
        public ElementReference HeaderElementReference { get; private set; }

        /// <summary>
        /// Gets/Sets (protected set) the Panel root HTML element reference.
        /// </summary>
        public ElementReference FooterElementReference { get; private set; }

        /// <summary>
        /// Disable Outline (default is true).
        /// </summary>
        [Parameter]
        public bool EnableOutline { get; set; }

        /// <summary>
        /// Current screen size.
        /// </summary>
        public ScreenSize ScreenSize { get; private set; } = new ScreenSize();

        /// <summary>
        /// Current scroll info.
        /// </summary>
        public ScrollInfo ScrollInfo { get; private set; } = new ScrollInfo();

        private int headerHeight;
        private int footerHeight;

        private string ClassContentOverflow
            => EnableContentScroll ? "content-overflow" : string.Empty;

        private string ClassHideHeader
            => HideHeader ? "header hide-header" : "header";

        private string ClassHideFooter
            => HideFooter ? "footer hide-footer" : "footer";

        private class ResizeCallBack : IResizeCallBack
        {
            private readonly Func<int, int, ValueTask> handler;

            public ResizeCallBack(Func<int, int, ValueTask> handler)
            {
                this.handler = handler;
            }

            public ValueTask ResizeAsync(int width, int height)
            {
                return this.handler(width, height);
            }
        }

        private class ScrollCallBack : IScrollCallBack
        {
            private readonly Func<ScrollInfo, ValueTask> handler;

            public ScrollCallBack(Func<ScrollInfo, ValueTask> handler)
            {
                this.handler = handler;
            }

            public ValueTask ScrollAsync(ScrollInfo scrollInfo)
            {
                return this.handler(scrollInfo);
            }
        }

        private async ValueTask SetScreenSizeAsync(int width, int height)
        {
            ScreenSize = new ScreenSize()
            {
                Width = width,
                Height = height
            };

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        private ValueTask SetHeaderHeightAsync(int width, int height)
        {
            if (!HideHeader)
            {
                this.headerHeight = height;
            }

            return ValueTask.CompletedTask;
        }

        private ValueTask SetFooterHeightAsync(int width, int height)
        {
            if (!HideFooter)
            {
                this.footerHeight = height;
            }

            return ValueTask.CompletedTask;
        }

        private async ValueTask SetScrollAsync(ScrollInfo scrollInfo)
        {
            ScrollInfo = scrollInfo;

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                this.rootCallBackDisposable = await ResizeObserverService.RegisterResizeCallBackAsync(new ResizeCallBack(SetScreenSizeAsync), ElementReference).ConfigureAwait(false);
                this.headerCallBackDisposable = await ResizeObserverService.RegisterResizeCallBackAsync(new ResizeCallBack(SetHeaderHeightAsync), HeaderElementReference).ConfigureAwait(false);
                this.footerCallBackDisposable = await ResizeObserverService.RegisterResizeCallBackAsync(new ResizeCallBack(SetFooterHeightAsync), FooterElementReference).ConfigureAwait(false);
                this.rootScrollCallBackDisposable = await ScrollObserverService.RegisterScrollCallBackAsync(new ScrollCallBack(SetScrollAsync), ElementReference).ConfigureAwait(false);
            }

            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
        }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

            if (this.rootCallBackDisposable != null)
            {
                await this.rootCallBackDisposable.DisposeAsync().ConfigureAwait(false);
                this.rootCallBackDisposable = null;
            }
        }
    }
}
