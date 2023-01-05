using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsumerApp
{
    public class ApiClient
    {
        private readonly HttpClient client;

        public ApiClient(Uri baseUri = null)
        {
            this.client = new HttpClient { BaseAddress = baseUri ?? new Uri("http://my-api") };
        }

        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/");
            request.Headers.Add("Accept", "application/json");

            var response = await this.client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;


            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return response;
            }

            throw new Exception(reasonPhrase);

        }

        public async Task<HttpResponseMessage> GetUser(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id);
            request.Headers.Add("Accept", "application/json");

            var response = await this.client.SendAsync(request);

            var status = response.StatusCode;

            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return response;
            }

            throw new Exception(reasonPhrase);
        }
    }
}
