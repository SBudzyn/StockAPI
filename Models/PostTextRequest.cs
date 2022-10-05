namespace StockAPI.Models
{
    public class PostTextRequest
    {
        public string Name { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Decimal Cost { get; set; }
        public int AuthorsId { get; set; } 
        public int NumberOfSales { get; set; }
       
    }
}
