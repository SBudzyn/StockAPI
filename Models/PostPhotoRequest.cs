using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    public class PostPhotoRequest : PostRequest
    {
        public IFormFile Photo { get; set; } = null!;
    }
}
