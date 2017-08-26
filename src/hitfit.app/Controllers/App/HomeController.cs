using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using hitfit.app.Services;
using hitfit.google.auth.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace hitfit.app.Controllers.App
{
    //[Authorize]
    public class HomeController : Controller
    {
        //private readonly GoogleDriveArticleService _articleService;

        public HomeController()
        {
            
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            GoogleDriveArticleService articleService = null;

            var result = await new AuthorizationCodeMvcApp(this, new Google.AppFlowMetadata()).AuthorizeAsync(cancellationToken);
            //https://accounts.google.com/o/oauth2/v2/auth?access_type=offline&response_type=code&client_id=703137161297-kcdmjcjk5caa9ampgvraqa4ktt5nsqit.apps.googleusercontent.com&redirect_uri=http:%2F%2Flocalhost:51134%2FAuthCallback%2FIndexAsync&scope=https:%2F%2Fwww.googleapis.com%2Fauth%2Fdrive&state=63161155
            if (result.Credential == null)
            {
                return Redirect(result.RedirectUri);
            }
            else
            {
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "hitfit.app"
                });

                articleService = new GoogleDriveArticleService(service);

                //_driveService = service;
            }

            //var articleService = new GoogleDriveArticleService(new CancellationToken(), this);

            var articles = await articleService.GetArticlesAsync(4);
            
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
