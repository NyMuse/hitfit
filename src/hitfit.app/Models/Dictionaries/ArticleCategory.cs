namespace hitfit.app.Models.Dictionaries
{
    using System.Collections.Generic;

    public class ArticleCategory : RootDictionary
    {
        public List<Article> Articles { get; set; }
    }
}
