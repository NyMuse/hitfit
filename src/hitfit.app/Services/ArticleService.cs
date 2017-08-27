using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using hitfit.app.Models;
using HtmlAgilityPack;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using File = Google.Apis.Drive.v3.Data.File;

namespace hitfit.app.Services
{
    public class ArticleService
    {
        private readonly HitFitDbContext _context;

        public ArticleService(HitFitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetArticlesAsync(int count = 0)
        {
            return await _context.Articles.Where(a => a.Published).OrderBy(a => new Random().Next()).Take(count).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(string documentId)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(a => a.DocumentId == documentId && a.Published);

            if (article == null)
            {
                throw new KeyNotFoundException();
            }

            return article;
        }
    }
}
