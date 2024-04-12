namespace Server.Shared.Models
{
    public class ItemResponseModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte[] FileContent { get; set; }
    }
}
