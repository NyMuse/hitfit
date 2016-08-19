namespace HitFit.Api.Models
{
    using System;

    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public short? Growth { get; set; }
        public short? Weight { get; set; }
    }
}
