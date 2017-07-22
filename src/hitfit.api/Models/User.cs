using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using hitfit.api.Dto;

namespace hitfit.api.Models
{
    public class User : IIdentity
    {
        [NotMapped]
        public bool IsAuthenticated { get; set; }
        [NotMapped]
        public string AuthenticationType { get; set; }

        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<UserMeasurements> UserMeasurements { get; set; }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id,
                IsAdministrator = IsAdministrator,
                Name = Name,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                Birthday = Birthday,
                UserMeasurements = UserMeasurements.Select(e => e.ToDto()).ToList()
            };
        }
    }
}
