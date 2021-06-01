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
        /// <param name="serviceLifetime">Service Lifetime to use to register the IStringLocalizerFactory. (Default is Scoped)</param>
        /// <returns>The given service collection updated with the BlazorLayout services.</returns>
        public static IServiceCollection AddBlazorLayout(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<IResizeObserverService, ResizeObserverService>();
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped<IResizeObserverService, ResizeObserverService>();
                    break;
                case ServiceLifetime.Transient:
                default:
                    services.AddTransient<IResizeObserverService, ResizeObserverService>();
                    break;
            }

            return services;
        }
    }
}
