using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using hitfit.app.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace hitfit.app.Controllers.App
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly GoogleDriveArticleService _articleService;

        public HomeController(GoogleDriveArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetArticlesAsync(4);
            
            //foreach (var article in articles)
            //{
            //    article.ImageBase64 = Convert.ToBase64String(article.Image);
            //}

            ViewBag.Articles = articles;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Elements()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
