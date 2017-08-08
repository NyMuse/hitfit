using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models;
using hitfit.app.Models.Dictionaries;
using Microsoft.EntityFrameworkCore;

namespace hitfit.app.Services
{
    public class ArticleService
    {
        private readonly HitFitDbContext _context;

        public ArticleService(HitFitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(c => c.Category).Where(c => c.IsPublished == true).ToListAsync();
        }

        public async Task<List<Article>> GetRandomAsync(int count)
        {
            return await _context.Articles.Include(c => c.Category).Where(c => c.IsPublished == true).OrderBy(r => Guid.NewGuid()).Take(count).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(int articleId)
        {
            return await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id.Equals(articleId));
        }

        public async Task<List<Article>> GetArticles(int? authorId = null, int ? creatorId = null)
        {
            IQueryable<Article> query = _context.Articles;

            if (authorId.HasValue)
            {
                query = query.Where(a => a.AuthorId.Equals(authorId.Value));
            }

            if (creatorId.HasValue)
            {
                query = query.Where(a => a.CreatorId.Equals(creatorId.Value));
            }

            return await query.ToListAsync();
        }

        public async Task<int> AddArticle(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();

            return article.Id;
        }

        public async Task<int> EditArticle(Article article)
        {
            var entity = await _context.Articles.FirstOrDefaultAsync(a => a.Id.Equals(article.Id));

            entity.CategoryId = article.CategoryId;
            entity.Title = article.Title;
            entity.Content = article.Content;
            entity.Image = article.Image;

            await _context.SaveChangesAsync();

            return article.Id;
        }

        public async Task<List<ArticleCategory>> GetArticleCategoriesAsync()
        {
            return await _context.ArticleCategories.Where(c => c.IsActive == true).ToListAsync();
        }
    }
}
