using System;

namespace hitfit.app.Models
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
