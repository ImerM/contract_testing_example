using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProviderAPI.Model;
using ProviderAPI.Repositories;

namespace Provider.Tests
{
    public class ProviderStateMiddleware
    {
        private readonly IDictionary<string, Action> providerStates;
        private readonly RequestDelegate next;
        private readonly IUserRepository _repository;


        public ProviderStateMiddleware(RequestDelegate next, IUserRepository repository)
        {
            this.next = next;
            _repository = repository;

            providerStates = new Dictionary<string, Action>
            {
                {   "users exist", UsersExist },
                {   "user with ID 3 exists",    User3Exists  }
            };
        }

        private void UsersExist()
        {
            List<User> users = new List<User>()
            {
                new User(3, "Jack Smith", "Admin", "V2"),
                new User(8, "John Doe", "Moderator", "V1")
            };
            _repository.SetState(users);
        }
        private void User3Exists()
        {
            List<User> users = new List<User>()
            {
                new User(3, "Jack Smith", "Admin", "V2"),
            };
            _repository.SetState(users);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path
                .Value?.StartsWith("/provider-states") ?? false)
            {
                await next.Invoke(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method == HttpMethod.Post.ToString()
                && context.Request.Body != null)
            {
                string jsonRequestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (!string.IsNullOrEmpty(providerState?.State))
                {
                    providerStates[providerState.State].Invoke();
                }

                await context.Response.WriteAsync(string.Empty);
            }
        }
    }
}
