using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;

namespace hitfit.app.Models
{
    public class User : IdentityUser<int>
    {
        //[NotMapped]
        //public bool IsAuthenticated { get; set; }
        //[NotMapped]
        //public string AuthenticationType { get; set; }

        [JsonProperty(Order = 1)]
        public override int Id { get; set; }

        [JsonProperty(Order = 2)]
        public bool IsAdministrator { get; set; }

        [JsonProperty(Order = 3)]
        public bool IsActive { get; set; }

        [JsonProperty(Order = 4)]
        public override string UserName { get; set; }

        [JsonProperty(Order = 5)]
        public override string Email { get; set; }

        [JsonProperty(Order = 6)]
        public override bool EmailConfirmed { get; set; }

        [JsonProperty(Order = 7)]
        public override string PhoneNumber { get; set; }

        [JsonProperty(Order = 8)]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonProperty(Order = 9)]
        public string FirstName { get; set; }
        
        [JsonProperty(Order = 11)]
        public string LastName { get; set; }

        [JsonProperty(Order = 12)]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(Order = 13)]
        public DateTime ModifiedOn { get; set; }

        [JsonProperty(Order = 14)]
        public UserDetails Details { get; set; }

        [JsonProperty(Order = 15)]
        public ICollection<UserProgram> UserPrograms { get; set; }

        [JsonProperty(Order = 16)]
        public ICollection<UserMeasurements> UserMeasurements { get; set; }

        [JsonProperty(Order = 17)]
        public ICollection<Report> Reports { get; set; }

        [JsonProperty(Order = 18)]
        public ICollection<Article> Articles { get; set; }

        [JsonIgnore]
        public override string NormalizedUserName { get; set; }

        [JsonIgnore]
        public override string NormalizedEmail { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }
        
        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        [JsonIgnore]
        public override ICollection<IdentityUserRole<int>> Roles { get; }

        [JsonIgnore]
        public override ICollection<IdentityUserClaim<int>> Claims { get; }

        [JsonIgnore]
        public override ICollection<IdentityUserLogin<int>> Logins { get; }
    }
}
