using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    public class PostPhotoRequest
    {
        public int AuthorsId { get; set; }
        public IFormFile Photo { get; set; } = null!;
        [Column(TypeName = "decimal(10,2)")]
        public Decimal Cost { get; set; }
        public int NumberOfSales { get; set; }
    }
}
