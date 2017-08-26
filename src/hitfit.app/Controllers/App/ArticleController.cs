using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using hitfit.app.Models;
using hitfit.app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using HtmlAgilityPack;

namespace hitfit.app.Controllers.App
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly GoogleDriveArticleService _articleService;
        private readonly UserManager<User> _userManager;

        public ArticleController(UserManager<User> userManager)
        {
            //_articleService = new GoogleDriveArticleService(new CancellationToken(), this);
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetArticlesAsync();

            ViewBag.Articles = articles;
            
            return View();
        }

        [HttpGet("[controller]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewArticle(string id)
        {
            var article = await _articleService.GetArticleAsync(id);

            ViewBag.Article = article;

            return View();
        }

        //[HttpGet("[controller]/add")]
        //public async Task<IActionResult> AddArticle()
        //{
        //    //ViewBag.ArticleCategories = await _articleService.GetArticleCategoriesAsync();

        //    return View();
        //}

        //[HttpPost("[controller]/add")]
        //public async Task<IActionResult> AddArticle(IFormFile file, string articleTitle, string articleSubTitle, string articleContent, int category, bool publish = false)
        //{
        //    byte[] imageBytes;

        //    using (var ms = new MemoryStream())
        //    {
        //        await file.CopyToAsync(ms);
        //        imageBytes = ms.ToArray();
        //    }

        //    var author = await _userManager.FindByNameAsync(this.User.Identity.Name);
        //    var creator = await _userManager.FindByNameAsync(this.User.Identity.Name);

        //    var article = new Article
        //    {
        //        AuthorId = author.Id,
        //        CreatorId = creator.Id,
        //        CategoryId = category,
        //        Title = articleTitle,
        //        Content = articleContent,
        //        Image = imageBytes,
        //        IsPublished = publish
        //    };

        //    await _articleService.AddArticle(article);

        //    return RedirectToAction("Index");
        //}

        //[HttpGet("[controller]/{id}/edit")]
        //public async Task<IActionResult> EditArticle(int id)
        //{
        //    if (id == 0)
        //    {
        //        RedirectToAction("AddArticle");
        //    }

        //    var article = await _articleService.GetArticleAsync(id);
        //    article.ImageBase64 = Convert.ToBase64String(article.Image);

        //    ViewBag.ArticleCategories = await _articleService.GetArticleCategoriesAsync();
        //    ViewBag.Article = article;

        //    return View();
        //}

        //[HttpPost("[controller]/{id}/edit")]
        //public async Task<IActionResult> EditArticle(int id, int categoryId, IFormFile file, string articleTitle,
        //    string articleSubTitle, string articleContent, int category, bool publish = false)
        //{
        //    byte[] imageBytes;

        //    using (var ms = new MemoryStream())
        //    {
        //        await file.CopyToAsync(ms);
        //        imageBytes = ms.ToArray();
        //    }

        //    var article = new Article
        //    {
        //        Id = id,
        //        CategoryId = category,
        //        Title = articleTitle,
        //        Content = articleContent,
        //        Image = imageBytes,
        //        IsPublished = publish
        //    };

        //    await _articleService.EditArticle(article);

        //    return RedirectToAction("ViewArticle", id);
        //}
    }
}