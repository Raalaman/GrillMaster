using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// Configures basic data for the HttpClient
        /// </summary>
        /// <param name="client">entity that is going to be configured</param>
        /// <param name="baseUrl">baseURL that all the clients will have </param>
        public static void AddHttpBaseClientConfig(HttpClient client, string baseUrl)
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}
