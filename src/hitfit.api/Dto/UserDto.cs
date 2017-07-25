using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.api.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        
        public ICollection<UserMeasurementsDto> UserMeasurements { get; set; }
    }
}
