using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApplicationCore
{
    public static class CustomJsonSerializer
    {
        private static JsonSerializerOptions GetCustomJsonSerializerOptions(JsonSerializerOptions options = null)
        {
            if (options == null)
                options = new JsonSerializerOptions();

            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            return options;
        }
        public static async Task<T> DeserializeHttpContent<T>(HttpContent httpContent)
        {
            string strResult = await httpContent.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(strResult, GetCustomJsonSerializerOptions());
        }
    }
}
