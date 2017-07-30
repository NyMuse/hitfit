namespace hitfit.app.Models
{
    public class ImageObject
    {
        public int Id { get; set; }
        public string RelationType { get; set; }
        public int OwnerId { get; set; }
        public byte[] Image { get; set; }
    }
}
