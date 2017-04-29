using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace AdminUI.Helpers
{
    public class RepositoryApi
    {
        static HttpClient client = new HttpClient();

        //public RepositoryApi()
        //{
        //    //client = new HttpClient();
        //    //client.DefaultRequestHeaders.Accept.Clear();
        //    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}

        public async Task<HttpResponseMessage> PostCreateAsync<T>(string pathUrl, T entity)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync(pathUrl, entity);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<T> PostAsync<T>(string pathUrl, T entity)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(pathUrl, entity);
            T obj = default(T);
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            return obj;
        }

        public async Task<T> GeTAsync<T>(string pathUrl)
        {
            T obj = default(T);
            HttpResponseMessage response = await client.GetAsync(pathUrl);
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<T>();
            }
            return obj;
        }

        public async Task<T> PutAsync<T>(string pathUrl, T entity)
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