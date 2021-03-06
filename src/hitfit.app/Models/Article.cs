﻿using System.ComponentModel.DataAnnotations.Schema;

namespace hitfit.app.Models
{
    public class Article
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("documentid")]
        public string DocumentId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("headerimage")]
        public string HeaderImage { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("published")]
        public bool Published { get; set; }
    }
}
