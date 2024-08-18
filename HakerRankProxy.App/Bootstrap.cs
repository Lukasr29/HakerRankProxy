using HakerRankProxy.App.Models;
using HakerRankProxy.App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HakerRankProxy.App
{
    public static class Bootstrap
    {
        public static void BootstrapHackerRankApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EndpointsConfiguration>(configuration.GetSection(nameof(EndpointsConfiguration)));

            services.AddDistributedMemoryCache();
            services.AddLogging();
            services.AddSingleton<Storage.IStorageContainer, Storage.StorageContainer>();
            services.AddScoped<IHackerRankDataUpdater, HackerRankDataUpdater>();

            services.AddHttpClient<Clients.HackerRankHttpClient>((sp, c) =>
            {
                var endpoints = sp.GetRequiredService<IOptions<EndpointsConfiguration>>();
                c.BaseAddress = new Uri(endpoints.Value.HackerRank);
            });

            services.AddHostedService<Background.HackerRankBackgroundUpdater>();
        }

    }
}
