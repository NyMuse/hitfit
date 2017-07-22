using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.api.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<UserMeasurementsDto> UserMeasurements { get; set; }
    }
}
