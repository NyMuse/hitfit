using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Discovery;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using hitfit.app.Models;
using HtmlAgilityPack;
using File = Google.Apis.Drive.v3.Data.File;

namespace hitfit.app.Services
{
    public class GoogleDriveArticleService
    {
        private DriveService _driveService;

        public GoogleDriveArticleService()
        {
            GetDriveService();
        }

        private async void GetDriveService()
        {
            string[] Scopes = new[] { DriveService.Scope.DriveReadonly };
            UserCredential credential;

            using (var stream = new FileStream("google_credentials.json",
                FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None);
            }

            // Create the service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "hitfit.app",
            });

            _driveService = service;
        }

        public async Task<List<Article>> GetArticlesAsync(int pageSize = 4)
        {
            FilesResource.ListRequest request = _driveService.Files.List();
            request.PageSize = pageSize;
            request.Q = "starred = true and mimeType = 'application/vnd.google-apps.document'";
            request.Fields = "nextPageToken, files(id, name, mimeType)";
            
            var result = await request.ExecuteAsync();

            var artickes = new List<Article>();
            foreach (var file in result.Files)
            {
                var docHtml = await _driveService.Files.Export(file.Id, "text/html").ExecuteAsync();
                var article = ParseGoogleDoc(docHtml);

                article.DocumentId = file.Id;
                article.Title = file.Name;

                artickes.Add(article);
            }

            return artickes;
        }

        public async Task<Article> GetArticleAsync(string docId)
        {
            var file = await _driveService.Files.Get(docId).ExecuteAsync();
            var docHtml = await _driveService.Files.Export(docId, "text/html").ExecuteAsync();
            var article = ParseGoogleDoc(docHtml, false);

            article.DocumentId = file.Id;
            article.Title = file.Name;

            return article;
        }

        private Article ParseGoogleDoc(string docHtml, bool preview = true)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(docHtml);
            string text = WebUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("//body").InnerText);
            string image = doc.DocumentNode.Descendants("img").FirstOrDefault().GetAttributeValue("src", null);

            Article article = new Article
            {
                ImageBase64 = image,
                Content = preview ? text.Length > 200 ? text.Substring(0, 200) + "..." : text : docHtml
            };

            return article;
        }
    }
}
