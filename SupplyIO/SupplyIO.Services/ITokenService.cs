using SupplyIO.SupplyIO.Services.Models;

namespace SupplyIO.SupplyIO.Services
{
    public interface ITokenService
    {
        public Task<TokenApiModel> Refresh(TokenApiModel tokenApiModel);
        public Task<bool> CheckAccessKey(string token);
    }
}
