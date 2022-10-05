namespace StockAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }

    }
}
