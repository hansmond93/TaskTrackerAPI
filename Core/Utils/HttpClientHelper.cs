using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class HttpClientHelper
    {
        public static async Task<TResult> PostAsJsonAsync<TModel, TResult>(this HttpClient client, string requestUri, TModel model)
        {
            var Content = new StringContent(JsonConvert.SerializeObject(model), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, Content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResult>();
        }

        public static async Task<TResult> DeleteAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.DeleteAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TResult>();
        }

        public static async Task<TResult> PutAsync<TModel, TResult>(this HttpClient client, string requestUri, TModel model)
        {
            var response = await client.PutAsJsonAsync(requestUri, model);
            return await response.Content.ReadAsAsync<TResult>();
        }

        public static async Task<TResult> GetAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var str = response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsAsync<TResult>();
        }
    }
}