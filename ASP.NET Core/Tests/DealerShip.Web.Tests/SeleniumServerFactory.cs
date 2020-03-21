namespace DealerShip.Web.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;

<<<<<<< HEAD
    using Microsoft.AspNetCore;
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

<<<<<<< HEAD
    public sealed class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
=======
    public class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
        where TStartup : class
    {
        private readonly Process process;

        private IWebHost host;

        public SeleniumServerFactory()
        {
            this.ClientOptions.BaseAddress = new Uri("https://localhost"); // will follow redirects by default
<<<<<<< HEAD
            this.CreateServer(this.CreateWebHostBuilder());
=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5

            this.process = new Process
                       {
                           StartInfo = new ProcessStartInfo
                                       {
                                           FileName = "selenium-standalone",
                                           Arguments = "start",
                                           UseShellExecute = true,
                                       },
                       };
            this.process.Start();
        }

<<<<<<< HEAD
        public string RootUri { get; set; }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
=======
        public string RootUri { get; set; } // Save this use by tests

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            // Real TCP port
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
            this.host = builder.Build();
            this.host.Start();
            this.RootUri = this.host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); // Last is https://localhost:5001!

            // Fake Server we won't use...this is lame. Should be cleaner, or a utility class
            return new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
        }

<<<<<<< HEAD
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost.CreateDefaultBuilder(Array.Empty<string>());
            builder.UseStartup<TStartup>();
            return builder;
        }

=======
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.host.Dispose();
                this.process.CloseMainWindow(); // Be sure to stop Selenium Standalone
            }
        }

        public class FakeStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure()
            {
            }
        }
    }
}
