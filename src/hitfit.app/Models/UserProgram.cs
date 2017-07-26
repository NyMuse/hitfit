using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.app.Models
{
    public class UserProgram
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProgramId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime FinishedOn { get; set; }
        public string Notes { get; set; }

        public User User { get; set; }
        public Program Program { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
