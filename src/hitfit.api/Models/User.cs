using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using hitfit.api.Dto;

namespace hitfit.api.Models
{
    public class User
    {
        [NotMapped]
        public bool IsAuthenticated { get; set; }
        [NotMapped]
        public string AuthenticationType { get; set; }

        public int Id { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsActive { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public UserDetails Details { get; set; }
        public ICollection<UserProgram> UserPrograms { get; set; }
        public ICollection<UserMeasurements> UserMeasurements { get; set; }
        public ICollection<Report> Reports { get; set; }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id,
                Login = Login,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                //UserMeasurements = UserMeasurements.Select(e => e.ToDto()).ToList()
            };
        }
    }
}
