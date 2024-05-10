using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PotterKata.Interfaces;
using PotterKata.Services;

namespace PotterKata.Tests
{
    public class Setup
    {
        private readonly IHostBuilder hostBuilder;
        private IServiceProvider serviceProvider;
        private bool _built = false;

        public Setup() => hostBuilder = Host.CreateDefaultBuilder();

        public IServiceProvider Services => serviceProvider ?? Build();

        private IServiceProvider Build()
        {
            if (_built)
                throw new InvalidOperationException("Loaded already.");
            _built = true;

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IDemoDataService, DemoDataService>();
                services.AddSingleton<IDiscountSchemeService, DiscountSchemeService>();
            });

            serviceProvider = hostBuilder.Build().Services;
            return serviceProvider;
        }
    }
}