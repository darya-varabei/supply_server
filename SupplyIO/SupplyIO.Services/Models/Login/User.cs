namespace SupplyIO.SupplyIO.Services.Models.Login
{
    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public UserInfo? UserInfo { get; set; }
    }
}
