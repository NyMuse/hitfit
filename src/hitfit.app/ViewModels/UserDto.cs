using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.app.ViewModels
{
    public class UserDto
    {
        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public List<UserMeasurementsDto> UserMeasurements { get; set; }
    }
}
