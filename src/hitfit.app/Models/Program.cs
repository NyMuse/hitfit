using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace hitfit.app.Models
{
    public class Program
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedon")]
        public DateTime ModifiedOn { get; set; }

        [Column("startdate")]
        public DateTime? StartDate { get; set; }

        [Column("finishdate")]
        public DateTime? FinishDate { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public ICollection<UserProgram> UserPrograms { get; set; }
    }
}
