using ApplicationCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public abstract class BaseApiService<T> : IAsyncOnlyGetRepository<T>
    {
        protected abstract string RouteName { get; }

        private readonly IHttpClientFactory _clientFactory;

        protected BaseApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await GetAllEntitiesAsync<IReadOnlyList<T>>(RouteName);
        }

        private async Task<U> GetAllEntitiesAsync<U>(string route) where U : IReadOnlyList<T>
        {
            HttpClient client;
            client = _clientFactory.CreateClient(Client.DEFAULT);
            HttpResponseMessage httpResponse = await client.GetAsync(route);
            if (httpResponse.IsSuccessStatusCode)
            {
                return await CustomJsonSerializer.DeserializeHttpContent<U>(httpResponse.Content);
            }
            throw new HttpRequestException($"{httpResponse.StatusCode} {httpResponse.ReasonPhrase}");
        }
    }
}
