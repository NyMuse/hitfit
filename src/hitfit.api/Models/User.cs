using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public UserType UserType { get; set; }
        

        // public short? Growth { get; set; }
        // public short? Weight { get; set; }
    }
}
