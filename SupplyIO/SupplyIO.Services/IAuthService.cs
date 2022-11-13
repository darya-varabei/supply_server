using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.Login;
using SupplyIO.SupplyIO.Services.ViewModel;

namespace SupplyIO.SupplyIO.Services
{
    public interface IAuthService
    {
        public Task<TokenApiModel> Login(User loginModel);
        public Task<UserInfo> GetUser(string login);
        public Task<bool> AddUserAsync(User user);
        public Task<List<UserViewModel>> GetUsersAsync();
    }
}
