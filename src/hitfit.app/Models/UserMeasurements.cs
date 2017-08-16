using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace hitfit.app.Models
{
    public class UserMeasurements
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("userprogramid")]
        public int UserProgramId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("createdon")]
        public DateTime CreatedOn { get; set; }

        [Column("modifiedon")]
        public DateTime ModifiedOn { get; set; }

        [Column("growth")]
        public short? Growth { get; set; }

        [Column("weight")]
        public short? Weight { get; set; }

        [Column("wrist")]
        public short? Wrist { get; set; }

        [Column("hand")]
        public short? Hand { get; set; }

        [Column("breast")]
        public short? Breast { get; set; }

        [Column("waisttop")]
        public short? WaistTop { get; set; }

        [Column("waistmiddle")]
        public short? WaistMiddle { get; set; }

        [Column("waistbottom")]
        public short? WaistBottom { get; set; }

        [Column("buttocks")]
        public short? Buttocks { get; set; }

        [Column("thighs")]
        public short? Thighs { get; set; }

        [Column("leg")]
        public short? Leg { get; set; }

        [Column("kneetop")]
        public short? KneeTop { get; set; }

        public User User { get; set; }
    }
}
