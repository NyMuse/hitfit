using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace hitfit.app.Models
{
    public class Role : IdentityRole<int>
    {
        [Column("id")]
        public override int Id { get; set; }

        [Column("name")]
        public override string Name { get; set; }

        [Column("normalizedname")]
        public override string NormalizedName { get; set; }

        [Column("concurrencystamp")]
        public override string ConcurrencyStamp { get; set; }
    }
}
