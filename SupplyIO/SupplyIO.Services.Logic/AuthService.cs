using Microsoft.IdentityModel.Tokens;
using SupplyIO.SupplyIO.DataAccess;
using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.Login;
using SupplyIO.SupplyIO.Services.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SupplyIO.SupplyIO.Services.Logic
{
    public class AuthService : IAuthService
    {
        private readonly IAuthAccess _access;
        private readonly int _liveTimeAccessTokenMinutes;
        private readonly int _liveTimeRefreshTokenHours;
        private readonly string _key;

        public AuthService(IAuthAccess access, IConfiguration config)
        {
            _access = access;
            _key = config.GetSection("SecreteKey").Value;
            _liveTimeAccessTokenMinutes = int.Parse(config.GetSection("LiveTimeAccessTokenMinutes").Value);
            _liveTimeRefreshTokenHours = int.Parse(config.GetSection("LiveTimeRefreshTokenHours").Value);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            return await _access.AddUserAsync(user);
        }

        public async Task<UserInfo> GetUser(string login)
        {
            var user = await _access.GetUserAsync(login);

            return user.UserInfo;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            return (await _access.GetUsersAsync()).Select(us => MapUser(us)).ToList();
        }

        public async Task<TokenApiModel> Login(User user)
        {
            var userData = await _access.AuthUserAsync(user.Login, user.Password);

            if (userData == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userData.Login),
                new Claim(ClaimTypes.Role, userData.UserInfo.Position)
            };

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();

            userData.RefreshToken = refreshToken;
            userData.RefreshTokenExpiryTime = DateTime.Now.AddHours(_liveTimeRefreshTokenHours);

            await _access.SetNewRefreshKeyAsync(userData);

            return new TokenApiModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(_liveTimeAccessTokenMinutes),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private UserViewModel MapUser(User user)
            => new()
            {
                Login = user.Login,
                Created = user.Created,
                FirstName = user.UserInfo.FirstName,
                LastName = user.UserInfo.LastName,
                Position = user.UserInfo.Position,
            };
    }
}
