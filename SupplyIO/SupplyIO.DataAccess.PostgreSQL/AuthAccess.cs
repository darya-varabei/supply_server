using Microsoft.EntityFrameworkCore;
using SupplyIO.SupplyIO.Services.Models.Context;
using SupplyIO.SupplyIO.Services.Models.Login;

namespace SupplyIO.SupplyIO.DataAccess.PostgreSQL
{
    public class AuthAccess : IAuthAccess
    {
        private readonly MetalContext _context;

        public AuthAccess(MetalContext metalContext)
            => _context = metalContext;

        public async Task<bool> AddUserAsync(User user)
        {
            var userDb = await _context.User.FirstOrDefaultAsync(us => us.Login == user.Login);

            if (userDb is not null)
                return false;
            else
            {
                user.Created = DateTime.UtcNow;

                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<User> AuthUserAsync(string login, string password)
        {
            login = login is not null ? login : throw new ArgumentNullException(nameof(login));

            var a = await _context.User.ToListAsync();

            return await _context.User.Include(us => us.UserInfo)
                                      .FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
        }

        public async Task<User> GetUserAsync(string login)
        {
            login = login is not null ? login : throw new ArgumentNullException(nameof(login));

            return await _context.User.Include(us => us.UserInfo)
                                      .FirstOrDefaultAsync(user => user.Login == login);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.User.Include(us => us.UserInfo).ToListAsync();
        }

        public async Task<bool> SetNewRefreshKeyAsync(User user)
        {
            user = user is not null ? user : throw new ArgumentNullException(nameof(user));

            var userData = await _context.User.FirstOrDefaultAsync(userData => userData.Login == user.Login);

            userData.RefreshToken = user.RefreshToken;
            userData.RefreshTokenExpiryTime = DateTime.SpecifyKind((DateTime)user.RefreshTokenExpiryTime, DateTimeKind.Utc);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
