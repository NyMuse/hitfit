using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using hitfit.app.Models.Dictionaries;

namespace hitfit.app.Models
{
    public class Article
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CreatorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        [NotMapped]
        public string ImageBase64 { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsPublished { get; set; }

        public User Author { get; set; }
        public ArticleCategory Category { get; set; }
        //public User Creator { get; set; }
    }
}
