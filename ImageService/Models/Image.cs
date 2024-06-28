namespace ImageService.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public byte[]? ImageData { get; set; }
      
    }
}
