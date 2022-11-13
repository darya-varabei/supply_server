using System.Text.Json.Serialization;

namespace SupplyIO.SupplyIO.Services.Models.Login
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? NumberOfPhone { get; set; }
        public string? Position { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
