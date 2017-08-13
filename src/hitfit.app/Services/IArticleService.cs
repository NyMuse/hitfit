using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Drive.v3.Data;
using hitfit.app.Models;

namespace hitfit.app.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetDocsAsync(int pageSize = 4);

        Task<string> GetDocHtmlAsync(string docId);
    }
}
