using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace hitfit.app.Models
{
    public class UserRole : IdentityUserRole<int>
    {
        [Column("userid")]
        public override int UserId { get; set; }

        [Column("roleid")]
        public override int RoleId { get; set; }
    }
}