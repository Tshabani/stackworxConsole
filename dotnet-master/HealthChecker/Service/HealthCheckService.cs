using HealthChecker.GraphQL;
using HealthChecker.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthChecker.Service
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IHttpClientFactory _clientFactory;
        public HealthCheckService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<Response> GetStatusAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
             "https://dev.vantage.run/health");

            var client = _clientFactory.CreateClient();
            client.Timeout = new TimeSpan(TimeSpan.FromMinutes(5).Ticks);

            var response = await client.SendAsync(request);

            var server = new Response();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                server = await JsonSerializer.DeserializeAsync
                    <Response>(responseStream);
            }

            return server;
        }

        public Task<Server> GetStatusWithRestsharpAsync()
        {
            var client = new RestClient("https://dev.vantage.run/health");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            return null;
        }
    }
}
