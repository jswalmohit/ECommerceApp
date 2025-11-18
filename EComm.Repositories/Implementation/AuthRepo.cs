using ECommerceApp.Context;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class AuthRepo(EComDbContext context) : IAuthRepo
    {
        private readonly EComDbContext _context = context;

        public async Task<UserEntity?> ValidateUserAsync(string loginId, string password)
        {
            var user = await _context.Users
                .Include(u => u.Credentials)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.LoginId == loginId);

            if (user == null || user.Credentials == null)
                return null;

            // Simple password comparison (in production, use hashed passwords)
            if (user.Credentials.Password != password)
                return null;

            return user;
        }
    }
}

