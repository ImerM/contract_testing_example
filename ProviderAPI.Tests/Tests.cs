using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;

namespace Provider.Tests
{
    public class Tests : IClassFixture<ProviderApiFixture>
    {
        private readonly ProviderApiFixture fixture;
        private readonly ITestOutputHelper output;


        public Tests(ProviderApiFixture fixture, ITestOutputHelper output)
        {
            
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public void ProviderAPIHonorsContract()
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    // NOTE: PactNet defaults to a ConsoleOutput, however
                    // xUnit 2 does not capture the console output, so this
                    // sample creates a custom xUnit outputter. You will
                    // have to do the same in xUnit projects.
                    new XUnitOutput(output),
                },
            };

            string pactPath = Path.Combine("..",
                                           "..",
                                           "..",
                                           "..",
                                           "pacts",
                                           "Our API Consumer-Our API Provider.json");

            // Act / Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ServiceProvider("Our API Provider", fixture.ServerUri)
                .WithFileSource(new FileInfo(pactPath))
                .WithProviderStateUrl(new Uri(fixture.ServerUri, "/provider-states"))
                .Verify();
        }


        [Fact]
        public void VerifyRemoteContract()
        {
            // Arrange
            var config = new PactVerifierConfig
            {

                Outputters = new List<IOutput>
                                {
                                    new ConsoleOutput()
                                },

            };

            var uri = new Uri("https://authority.pactflow.io");

            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ServiceProvider("Our API Provider", fixture.ServerUri)
                .WithPactBrokerSource(uri, configure => 
                    configure.TokenAuthentication(Environment.GetEnvironmentVariable("IMER_PACT_TOKEN"))
                    .PublishResults("1")
                )
                .WithProviderStateUrl(new Uri(fixture.ServerUri, "/provider-states"))
                .Verify();
        }
    }
}
