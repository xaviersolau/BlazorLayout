// ----------------------------------------------------------------------
// <copyright file="IResponsiveLayoutService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.BlazorLayout.Services
{
    /// <summary>
    /// Responsive layout service.
    /// </summary>
    public interface IResponsiveLayoutService
    {
        /// <summary>
        /// Hide header panel.
        /// </summary>
        /// <param name="hide"></param>
        void HideHeader(bool hide);

        /// <summary>
        /// Hide footer panel.
        /// </summary>
        /// <param name="hide"></param>
        void HideFooter(bool hide);
    }
}
