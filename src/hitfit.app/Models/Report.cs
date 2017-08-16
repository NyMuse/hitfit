using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hitfit.app.Models
{
    public class Report
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("userprogramid")]
        public int UserProgramId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("photo")]
        public byte[] Photo { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public User User { get; set; }
        public UserProgram UserProgram { get; set; }
    }
}
