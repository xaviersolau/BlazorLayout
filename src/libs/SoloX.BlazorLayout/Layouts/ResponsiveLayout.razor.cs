// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayout.razor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoloX.BlazorLayout.Core;
using SoloX.BlazorLayout.Services;

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

        private IAsyncDisposable? rootCallbackDisposable;
        private IAsyncDisposable? headerCallbackDisposable;
        private IAsyncDisposable? footerCallbackDisposable;
        private IAsyncDisposable? rootScrollCallbackDisposable;

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
        /// Disable ScrollX on Horizontal Navigation Menu.
        /// </summary>
        [Parameter]
        public bool DisableHorizontalNavigationMenuScrollX { get; set; }

        /// <summary>
        /// Use Small navigation.
        /// </summary>
        [Parameter]
        public bool UseSmallNavigation { get; set; }

        /// <summary>
        /// Event triggered when UseSmallNavigation has changed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> UseSmallNavigationChanged { get; set; }

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
        /// ResponsiveLayoutService
        /// </summary>
        [Inject]
        public IResponsiveLayoutServiceInternal ResponsiveLayoutServiceInternal { get; set; } = default!;

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

        private readonly CancellationTokenSource cancelTokenSource = new();

        private class ResizeCallback : IResizeCallback
        {
            private readonly Func<int, int, ValueTask> handler;

            public ResizeCallback(Func<int, int, ValueTask> handler)
            {
                this.handler = handler;
            }

            public ValueTask ResizeAsync(int width, int height)
            {
                return this.handler(width, height);
            }
        }

        private class ScrollCallback : IScrollCallback
        {
            private readonly Func<ScrollInfo, ValueTask> handler;

            public ScrollCallback(Func<ScrollInfo, ValueTask> handler)
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
            var oldScreenSize = ScreenSize;

            ScreenSize = new ScreenSize()
            {
                Width = width,
                Height = height
            };

            if ((oldScreenSize.IsVeryLarge || oldScreenSize.IsVeryVeryLarge)
                && !(ScreenSize.IsVeryLarge || ScreenSize.IsVeryVeryLarge)
                && !UseSmallNavigation)
            {
                UseSmallNavigation = true;

                await UseSmallNavigationChanged.InvokeAsync(true).ConfigureAwait(false);
            }
            else if (!(oldScreenSize.IsVeryLarge || oldScreenSize.IsVeryVeryLarge)
                && (ScreenSize.IsVeryLarge || ScreenSize.IsVeryVeryLarge)
                && UseSmallNavigation)
            {
                UseSmallNavigation = false;

                await UseSmallNavigationChanged.InvokeAsync(false).ConfigureAwait(false);
            }

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
            if (firstRender && !this.cancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    this.rootCallbackDisposable = await ResizeObserverService
                        .RegisterResizeCallbackAsync(new ResizeCallback(SetScreenSizeAsync), ElementReference, this.cancelTokenSource.Token)
                        .ConfigureAwait(false);
                    this.headerCallbackDisposable = await ResizeObserverService
                        .RegisterResizeCallbackAsync(new ResizeCallback(SetHeaderHeightAsync), HeaderElementReference, this.cancelTokenSource.Token)
                        .ConfigureAwait(false);
                    this.footerCallbackDisposable = await ResizeObserverService
                        .RegisterResizeCallbackAsync(new ResizeCallback(SetFooterHeightAsync), FooterElementReference, this.cancelTokenSource.Token)
                        .ConfigureAwait(false);
                    this.rootScrollCallbackDisposable = await ScrollObserverService
                        .RegisterScrollCallbackAsync(new ScrollCallback(SetScrollAsync), ElementReference, this.cancelTokenSource.Token)
                        .ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Expected if component is disposed early
                }
            }

            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
        }

        private async void OnRequestReceivedAsync(object? sender, ResponsiveLayoutRequestEventArgs e)
        {
            switch (e.Request)
            {
                case ResponsiveLayoutRequestEventArgs.RequestType.HideHeader:
                    HideHeader = e.BoolEventData;
                    break;
                case ResponsiveLayoutRequestEventArgs.RequestType.HideFooter:
                    HideFooter = e.BoolEventData;
                    break;
                default:
                    break;
            }

            await InvokeAsync(StateHasChanged).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            ResponsiveLayoutServiceInternal.RequestReceivedEvent += OnRequestReceivedAsync;
        }

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);

#if NET8_0_OR_GREATER
            await this.cancelTokenSource.CancelAsync().ConfigureAwait(false);
#else
            this.cancelTokenSource.Cancel();
#endif

            ResponsiveLayoutServiceInternal.RequestReceivedEvent -= OnRequestReceivedAsync;

            if (this.rootCallbackDisposable != null)
            {
                await this.rootCallbackDisposable.DisposeAsync().ConfigureAwait(false);
                this.rootCallbackDisposable = null;
            }

            if (this.headerCallbackDisposable != null)
            {
                await this.headerCallbackDisposable.DisposeAsync().ConfigureAwait(false);
                this.headerCallbackDisposable = null;
            }

            if (this.footerCallbackDisposable != null)
            {
                await this.footerCallbackDisposable.DisposeAsync().ConfigureAwait(false);
                this.footerCallbackDisposable = null;
            }

            if (this.rootScrollCallbackDisposable != null)
            {
                await this.rootScrollCallbackDisposable.DisposeAsync().ConfigureAwait(false);
                this.rootScrollCallbackDisposable = null;
            }

            this.cancelTokenSource.Dispose();
        }
    }
}
