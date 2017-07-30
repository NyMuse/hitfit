﻿using System;

namespace hitfit.app.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserProgramId { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }

        public User User { get; set; }
        public UserProgram UserProgram { get; set; }
    }
}
