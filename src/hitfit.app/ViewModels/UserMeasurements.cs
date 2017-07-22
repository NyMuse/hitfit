using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.app.ViewModels
{
    public class UserMeasurements
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public short? Growth { get; set; }
        public short? Weight { get; set; }
        public short? Wrist { get; set; }
        public short? Hand { get; set; }
        public short? Breast { get; set; }
        public short? WaistTop { get; set; }
        public short? WaistMiddle { get; set; }
        public short? WaistBottom { get; set; }
        public short? Buttocks { get; set; }
        public short? Things { get; set; }
        public short? Leg { get; set; }
        public short? KneeTop { get; set; }
    }
}
