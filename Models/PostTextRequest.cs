namespace StockAPI.Models
{
    public class PostTextRequest : PostRequest
    {
        public string Name { get; set; } = null!;
        public string Text { get; set; } = null!;
       
    }
}
