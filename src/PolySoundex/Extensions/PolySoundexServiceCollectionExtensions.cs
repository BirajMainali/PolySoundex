using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PolySoundex.Builders;
using PolySoundex.Models;
using PolySoundex.Services;

namespace PolySoundex.Extensions
{
    public static class PolySoundexServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the PolySoundex service to the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the service to.</param>
        /// <param name="configure">Action to configure the PolySoundexConfig list.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddPolySoundex(this IServiceCollection services, Action<List<PolySoundexConfig>> configure)
        {
            var builder = new PolySoundexBuilder();
            builder.UsePolySoundex(configure);
            var polySoundex = builder.Build();
            services.AddSingleton<IPolySoundex>(polySoundex);
            return services;
        }
    }
}