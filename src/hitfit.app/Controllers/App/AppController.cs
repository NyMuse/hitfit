using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace hitfit.app.Controllers.App
{
    public class AppController : Controller
    {
        //private static string _apiUrl;

        //public AppController()
        //{
        //    var builder = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json");

        //    IConfigurationRoot configuration = builder.Build();

        //    _apiUrl = configuration["ApiUrl"];
        //}

        //protected T PostAction<T>(string uri, string content)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var token = Request.Cookies["token"];
        //        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        //        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        request.Content = new StringContent(content, Encoding.UTF8, "application/json");

        //        client.BaseAddress = new Uri(_apiUrl);

        //        var response = client.SendAsync(request).Result;

        //        return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        //    }
        //}

        //protected List<T> GetAction<T>(string uri)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var token = Request.Cookies["token"];
        //        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        //        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        // request.Content = new StringContent(content, Encoding.UTF8, "application/json");

        //        client.BaseAddress = new Uri(_apiUrl);

        //        HttpResponseMessage response = client.SendAsync(request).Result;

        //        return JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);

        //    }
        //}

        //protected T GetAction<T>(string uri, string id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var token = Request.Cookies["token"];
        //        var request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}{1}", uri, id));
        //        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        client.BaseAddress = new Uri(_apiUrl);

        //        HttpResponseMessage response = client.SendAsync(request).Result;
        //        var result = response.Content.ReadAsStringAsync().Result;
        //        return JsonConvert.DeserializeObject<T>(result);
        //    }
        //}
    }
}
