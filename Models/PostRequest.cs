using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    public abstract class PostRequest
    {
        public int AuthorsId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Decimal Cost { get; set; }
        public int NumberOfSales { get; set; }

    }
}
