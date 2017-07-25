using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hitfit.api.Models.Dictionaries
{
    public class ProgramType
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
