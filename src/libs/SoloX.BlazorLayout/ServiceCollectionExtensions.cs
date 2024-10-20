// ----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SoloX.BlazorLayout.Services;
using SoloX.BlazorLayout.Services.Impl;
using System;

namespace SoloX.BlazorLayout
{
    /// <summary>
    /// Extension methods to setup the BlazorLayout package.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add BlazorLayout services.
        /// </summary>
        /// <param name="services">The service collection to setup.</param>
        /// <returns>The given service collection updated with the BlazorLayout services.</returns>
        public static IServiceCollection AddBlazorLayout(this IServiceCollection services)
            => AddBlazorLayout(services, opt => { });

        /// <summary>
        /// Add BlazorLayout services.
        /// </summary>
        /// <param name="services">The service collection to setup.</param>
        /// <param name="optionsBuilder">BlazorLayout options builder.</param>
        /// <returns>The given service collection updated with the BlazorLayout services.</returns>
        public static IServiceCollection AddBlazorLayout(this IServiceCollection services, Action<BlazorLayoutOptions> optionsBuilder)
        {
            services
                .AddScoped<IResizeObserverService, ResizeObserverService>()
                .AddScoped<IScrollObserverService, ScrollObserverService>()
                .AddScoped<IResponsiveLayoutServiceInternal, ResponsiveLayoutService>()
                .AddScoped<IResponsiveLayoutService>(r => r.GetRequiredService<IResponsiveLayoutServiceInternal>());

            services.Configure(optionsBuilder);

            return services;
        }
    }

    /// <summary>
    /// BlazorLayout options.
    /// </summary>
    public class BlazorLayoutOptions
    {
        /// <summary>
        /// Enable Js module to log traces in browser console.
        /// </summary>
        public bool EnableJsModuleLogs { get; set; }

        /// <summary>
        /// Resize Callback delay (in millisecond) or null if delay is disabled
        /// </summary>
        public int? ResizeCallbackDelay { get; set; } = 250;

        /// <summary>
        /// Enable the first synchronous callback on ResizeEvent burst in order to get 2 callback at burst start and at burst end. Otherwise only one asynchronous callback will be processed at burst end.
        /// It won't have any effect if ResizeCallbackDelay is not used.
        /// </summary>
        public bool EnableResizeEventBurstBoxingCallback { get; set; }

        /// <summary>
        /// Scroll Callback delay (in millisecond) or null if delay is disabled
        /// </summary>
        public int? ScrollCallbackDelay { get; set; } = 250;

        /// <summary>
        /// Enable the first synchronous callback on ScrollEvent burst in order to get 2 callback at burst start and at burst end. Otherwise only one asynchronous callback will be processed at burst end.
        /// It won't have any effect if ScrollCallbackDelay is not used.
        /// </summary>
        public bool EnableScrollEventBurstBoxingCallback { get; set; }
    }
}
