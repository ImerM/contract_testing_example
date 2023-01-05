using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ConsumerApp;
using PactNet;
using PactNet.Matchers;
using Xunit;

namespace ConsumerTests
{
    public class Tests
    {
        private readonly IPactBuilderV3 pactBuilder;
        private readonly List<object> users;

        public Tests()
        {
            // Use default pact directory ..\..\pacts and default log
            // directory ..\..\logs
            var pact = Pact.V3("Our API Consumer", "Our API Provider", new PactConfig
            {
                PactDir = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts"
            });

            // Initialize Rust backend
            this.pactBuilder = pact.WithHttpInteractions();
            users = new List<object>()
            {
               new {id = 3, name = "Jack Smith", type= "Admin", version= "V2" },
              new {id = 8, name = "John Doe", type= "Moderator", version= "V1" }
            };

        }

        [Fact]
        public async Task GetAllUsers()
        {
            // Arrange
            this.pactBuilder
                .UponReceiving("A GET request to retrieve all users")
                    .Given("users exist")
                    .WithRequest(HttpMethod.Get, "/api/users/")
                    .WithHeader("Accept", "application/json")
                .WillRespond()
                    .WithStatus(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new TypeMatcher(users));

            await this.pactBuilder.VerifyAsync(async ctx =>
            {
                var client = new ApiClient(ctx.MockServerUri);
                var userResponse = await client.GetAllUsers();

                Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
            }); 
        }


        [Fact]
        public async Task GetUserWithId3()
        {
            this.pactBuilder
                .UponReceiving("A GET request to retrieve a user")
                    .Given("user with ID 3 exists")
                    .WithRequest(HttpMethod.Get, "/api/users/3")
                    .WithHeader("Accept", "application/json")
                .WillRespond()
                    .WithStatus(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(
                new TypeMatcher(users[0]));

            await this.pactBuilder.VerifyAsync(async ctx =>
            {
                var client = new ApiClient(ctx.MockServerUri);
                var userResponse = await client.GetUser(3);

                Assert.Equal(HttpStatusCode.OK, userResponse.StatusCode);
            });

        }
    }
}
