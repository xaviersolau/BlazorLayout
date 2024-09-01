// ----------------------------------------------------------------------
// <copyright file="IResponsiveLayoutServiceInternal.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------


/* Unmerged change from project 'SoloX.BlazorLayout (net5.0)'
Before:
using System;

namespace SoloX.BlazorLayout.Services.Impl.ResponsiveLayoutImpl
After:
using System;
using SoloX;
using SoloX.BlazorLayout;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.Services.Impl;
using SoloX.BlazorLayout.Services.Impl.ResponsiveLayoutImpl
*/
using System;

namespace SoloX.BlazorLayout.Services
{
    /// <summary>
    /// Internal interface for the responsive layout service.
    /// </summary>
    public interface IResponsiveLayoutServiceInternal : IResponsiveLayoutService
    {
        /// <summary>
        /// Event raised on responsive layout service request.
        /// </summary>
        event EventHandler<ResponsiveLayoutRequestEventArgs> RequestReceivedEvent;
    }

    /// <summary>
    /// Event args for ResponsiveLayoutRequest.
    /// </summary>
    public class ResponsiveLayoutRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Request type enum.
        /// </summary>
        public enum RequestType
        {
            /// <summary>
            /// Hide header.
            /// </summary>
            HideHeader,
            /// <summary>
            /// Hide footer.
            /// </summary>
            HideFooter,
        }

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="request"></param>
        public ResponsiveLayoutRequestEventArgs(RequestType request)
        {
            Request = request;
        }

        /// <summary>
        /// Request type.
        /// </summary>
        public RequestType Request { get; }

        /// <summary>
        /// Event data as boolean.
        /// </summary>
        public bool BoolEventData { get; init; }
    }
}
