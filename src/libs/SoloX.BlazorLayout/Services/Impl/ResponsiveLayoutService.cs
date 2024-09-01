// ----------------------------------------------------------------------
// <copyright file="ResponsiveLayoutService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.BlazorLayout.Services.Impl
{
    /// <summary>
    /// Responsive layout service.
    /// </summary>
    public class ResponsiveLayoutService : IResponsiveLayoutServiceInternal
    {
        /// <inheritdoc/>
        public event EventHandler<ResponsiveLayoutRequestEventArgs> RequestReceivedEvent;

        /// <inheritdoc/>
        public void HideHeader(bool hide)
        {
            RequestReceivedEvent.Invoke(
                this,
                new ResponsiveLayoutRequestEventArgs(ResponsiveLayoutRequestEventArgs.RequestType.HideHeader)
                {
                    BoolEventData = hide,
                });
        }

        /// <inheritdoc/>
        public void HideFooter(bool hide)
        {
            RequestReceivedEvent.Invoke(
                this,
                new ResponsiveLayoutRequestEventArgs(ResponsiveLayoutRequestEventArgs.RequestType.HideFooter)
                {
                    BoolEventData = hide,
                });
        }
    }
}
