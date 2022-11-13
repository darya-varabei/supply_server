using SupplyIO.SupplyIO.Services.Models.Login;

namespace SupplyIO.SupplyIO.DataAccess
{
    public interface IAuthAccess
    {
        public Task<User> AuthUserAsync(string login, string password);
        public Task<User> GetUserAsync(string login);
        public Task<bool> SetNewRefreshKeyAsync(User user);
        public Task<bool> AddUserAsync(User user);
        public Task<List<User>> GetUsersAsync();
    }
}
