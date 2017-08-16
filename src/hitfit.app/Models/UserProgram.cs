using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace hitfit.app.Models
{
    public class UserProgram
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("programid")]
        public int ProgramId { get; set; }

        [Column("isactive")]
        public bool IsActive { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedon")]
        public DateTime ModifiedOn { get; set; }

        [Column("startedon")]
        public DateTime? StartedOn { get; set; }

        [Column("finishedon")]
        public DateTime? FinishedOn { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        public User User { get; set; }
        public Program Program { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
