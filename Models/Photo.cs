using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Link { get; set; } = null!;
        public long OriginalSize { get; set; }
        public DateTime DateOfCreation { get; set; }
        public Author Author { get; set; } = null!;
        [Column(TypeName = "decimal(10,2)")]
        public Decimal Cost { get; set; }
        public int NumberOfSales { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal SumOfRatings { get; set; }
        public int NumOfReviews { get; set; }
    }
}
