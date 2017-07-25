using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.api.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public byte[] Photo { get; set; }
        public string Notes { get; set; }

        public User User { get; set; }
    }
}
