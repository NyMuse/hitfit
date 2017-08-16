using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;

namespace hitfit.app.Models
{
    public class User : IdentityUser<int>
    {
        [JsonProperty(Order = 1)]
        [Column("id")]
        public override int Id { get; set; }

        [JsonProperty(Order = 3)]
        [Column("isactive")]
        public bool IsActive { get; set; }

        [JsonProperty(Order = 4)]
        [Column("username")]
        public override string UserName { get; set; }

        [JsonProperty(Order = 5)]
        [Column("email")]
        public override string Email { get; set; }

        [JsonProperty(Order = 6)]
        [Column("emailconfirmed")]
        public override bool EmailConfirmed { get; set; }

        [JsonProperty(Order = 7)]
        [Column("phonenumber")]
        public override string PhoneNumber { get; set; }

        [JsonProperty(Order = 8)]
        [Column("phonenumberconfirmed")]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonProperty(Order = 9)]
        [Column("firstname")]
        public string FirstName { get; set; }
        
        [JsonProperty(Order = 11)]
        [Column("lastname")]
        public string LastName { get; set; }

        [JsonProperty(Order = 12)]
        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(Order = 13)]
        [Column("modifiedon")]
        public DateTime ModifiedOn { get; set; }

        [Column("accessfailedcount")]
        public override int AccessFailedCount { get; set; }

        [Column("birthday")]
        public DateTime Birthday { get; set; }

        [Column("sex")]
        public string Sex { get; set; }

        [Column("photo")]
        public byte[] Photo { get; set; }

        [Column("notes")]
        public string Notes { get; set; }
        
        [JsonProperty(Order = 15)]
        public ICollection<UserProgram> UserPrograms { get; set; }

        [JsonProperty(Order = 16)]
        public ICollection<UserMeasurements> UserMeasurements { get; set; }

        [JsonProperty(Order = 17)]
        public ICollection<Report> Reports { get; set; }
        
        [JsonIgnore]
        [Column("normalizedusername")]
        public override string NormalizedUserName { get; set; }

        [JsonIgnore]
        [Column("normalizedemail")]
        public override string NormalizedEmail { get; set; }

        [JsonIgnore]
        [Column("passwordhash")]
        public override string PasswordHash { get; set; }
        
        [JsonIgnore]
        [Column("concurrencystamp")]
        public override string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        [Column("twofactorenabled")]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        [Column("lockoutenabled")]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        [Column("lockoutend")]
        public override DateTimeOffset? LockoutEnd { get; set; }

        //[JsonIgnore]
        //public override ICollection<IdentityUserRole<int>> Roles { get; }

        //[JsonIgnore]
        //public override ICollection<IdentityUserClaim<int>> Claims { get; }

        //[JsonIgnore]
        //public override ICollection<IdentityUserLogin<int>> Logins { get; }
    }
}
