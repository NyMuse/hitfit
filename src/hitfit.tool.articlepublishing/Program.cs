using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Npgsql;
using HtmlAgilityPack;

namespace hitfit.tool.articlepublishing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().Run().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("ERROR: " + e.Message);
                    Console.ReadKey();
                }
            }
        }

        private async Task Run()
        {
            string[] Scopes = new[] { DriveService.Scope.DriveReadonly };
            UserCredential credential;

            ClientSecrets secrets = new ClientSecrets
            {
                ClientId = "703137161297-kcdmjcjk5caa9ampgvraqa4ktt5nsqit.apps.googleusercontent.com",
                ClientSecret = "2XXbusAQkVUsY7MvoxXQRlIi"
            };

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, Scopes, "user", CancellationToken.None);

            // Create the service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "hitfit.app",
            });

            FilesResource.ListRequest request = service.Files.List();
            //request.PageSize = pageSize;
            request.Q = "mimeType = 'application/vnd.google-apps.document'";
            request.Fields = "nextPageToken, files(id, name, mimeType, starred)";

            var result = await request.ExecuteAsync();

            var articles = new List<Article>();
            foreach (var file in result.Files)
            {
                var docHtml = await service.Files.Export(file.Id, "text/html").ExecuteAsync();
                var article = ParseGoogleDoc(docHtml);

                article.DocumentId = file.Id;
                article.Title = file.Name;
                if (file.Starred.HasValue)
                {
                    article.Published = file.Starred.Value;
                }

                articles.Add(article);
            }

            //NpgsqlConnection connection = new NpgsqlConnection("Server=hitfitdbserver.postgres.database.azure.com;Database=hitfitdb;Port=5432;User Id=postgres@hitfitdbserver;Password=Tsunami9;");
            NpgsqlConnection connection = new NpgsqlConnection("User ID=postgres;Password=Tsunami9;Host=localhost;Port=5432;Database=hitfitdb;Pooling=true;");
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            foreach (var article in articles)
            {
                command.Parameters.Clear();

                command.CommandText = string.Format("SELECT content FROM articles WHERE documentid = '{0}'",
                    article.DocumentId);

                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    await reader.CloseAsync();

                    command.CommandText =
                        string.Format(
                            "UPDATE articles SET title = @title, headerimage = @headerimage, content = @content, published = @published WHERE documentid = @documentid",
                            article.Title, article.HeaderImage, article.Content, article.Published, article.DocumentId);
                    command.Parameters.AddWithValue("@documentid", article.DocumentId);
                    command.Parameters.AddWithValue("@title", article.Title);
                    command.Parameters.AddWithValue("@headerimage", article.HeaderImage);
                    command.Parameters.AddWithValue("@content", article.Content);
                    command.Parameters.AddWithValue("@published", article.Published);
                    await command.ExecuteNonQueryAsync();
                }
                else
                {
                    await reader.CloseAsync();

                    command.CommandText =
                    string.Format(
                        "INSERT INTO articles (documentid, title, headerimage, content, published) SELECT @documentid, @title, @headerimage, @content, @published",
                        article.DocumentId, article.Title, article.HeaderImage, article.Content, article.Published ? "true" : "false");
                    command.Parameters.AddWithValue("@documentid", article.DocumentId);
                    command.Parameters.AddWithValue("@title", article.Title);
                    command.Parameters.AddWithValue("@headerimage", article.HeaderImage);
                    command.Parameters.AddWithValue("@content", article.Content);
                    command.Parameters.AddWithValue("@published", article.Published);
                    await command.ExecuteNonQueryAsync();
                }
            }

            connection.Close();
        }

        private static Article ParseGoogleDoc(string docHtml)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(docHtml);

            var images = doc.DocumentNode.Descendants("img");
            foreach (var img in images)
            {
                img.SetAttributeValue("style", "max-width: 500px;");
                img.ParentNode.SetAttributeValue("style", "text-align: -webkit-center;");
                img.ParentNode.Attributes.Add("class", "image fit");
            }
            
            using (StringWriter writer = new StringWriter())
            {
                doc.Save(writer);
                docHtml = writer.ToString();
            }

            string image = doc.DocumentNode.Descendants("img").FirstOrDefault().GetAttributeValue("src", null);

            Article article = new Article
            {
                HeaderImage = image,
                Content = docHtml
            };

            return article;
        }
    }
}
