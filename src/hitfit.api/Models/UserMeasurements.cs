using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using hitfit.api.Dto;

namespace hitfit.api.Models
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

        public User User { get; set; }

        public UserMeasurementsDto ToDto()
        {
            return new UserMeasurementsDto
            {
                Id = Id,
                UserId = UserId,
                Growth = Growth,
                Weight = Weight,
                Wrist = Wrist,
                Hand = Hand,
                Breast = Breast,
                WaistTop = WaistTop,
                WaistMiddle = WaistMiddle,
                WaistBottom = WaistBottom,
                Buttocks = Buttocks,
                Things = Things,
                Leg = Leg,
                KneeTop = KneeTop
            };
        }
    }
}
