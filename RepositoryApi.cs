using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MVCAPI.Helpers
{
    public class RepositoryApi<T>
    {
        static HttpClient client = new HttpClient();

        public async Task<HttpResponseMessage> PostCreateAsync(string pathUrl, T entity)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(pathUrl, entity);            
            response.EnsureSuccessStatusCode();
            
            return response;
        }

        public async Task<T> PostAsync(string pathUrl, T entity)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(pathUrl, entity);
            T obj = default(T);
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            return obj;            
        }

        public async Task<T> GeTAsync(string pathUrl)
        {
            T obj = default(T);
            HttpResponseMessage response = await client.GetAsync(pathUrl);
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            return obj;
        }

        public async Task<T> PutAsync(string pathUrl, T entity)
        {
            T obj = default(T);
            HttpResponseMessage response = await client.PutAsJsonAsync(pathUrl, entity);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            
            return obj;
        }

        public async Task<HttpStatusCode> DeleteAsync(string pathUrl)
        {
            HttpResponseMessage response = await client.DeleteAsync(pathUrl);            
            return response.StatusCode;
        }




    }
}