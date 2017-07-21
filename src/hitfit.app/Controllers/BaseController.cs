using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace hitfit.app.Controllers
{
    public class BaseController : Controller
    {
        private static string _apiUrl;

        public BaseController()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            IConfigurationRoot Configuration = builder.Build();

            _apiUrl = Configuration["ApiUrl"];
        }

        public T PostAction<T>(string uri, string content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);

                var stringContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(uri, stringContent).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public List<T> GetAction<T>(string uri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);

                HttpResponseMessage response = client.GetAsync(uri).Result;

                return JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);

            }
        }

        public T GetAction<T>(string uri, string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);

                HttpResponseMessage response = client.GetAsync(string.Format("{0}{1}", uri, id)).Result;

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            }
        }
    }
}
