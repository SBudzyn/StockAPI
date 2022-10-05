using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    public class Text
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TextInfo { get; set; } = null!;
        public DateTime DateOfCreation { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal Cost { get; set; }
        public Author Author { get; set; } = null!;
        public int NumberOfSales { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal SumOfRatings { get; set; }
        public int NumOfReviews { get; set; }

    }
}
