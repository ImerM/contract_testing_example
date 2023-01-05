using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Provider.Tests
{
    public class ProviderApiFixture : IDisposable
    {
        private readonly IHost server;
        public Uri ServerUri { get; }

        public ProviderApiFixture()
        {
            ServerUri = new Uri("http://localhost:9223");
            server = Host.CreateDefaultBuilder()
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseUrls(ServerUri.ToString());
                                webBuilder.UseStartup<TestStartup>();
                            })
                            .Build();
            server.Start();
        }

        public void Dispose()
        {
            server.Dispose();

            // Clean out leftover data
            foreach (var dataFile in Directory.GetFiles(Path.Combine("..", "..", "..", "data")))
            {
                File.Delete(dataFile);
            }
        }
    }
}
