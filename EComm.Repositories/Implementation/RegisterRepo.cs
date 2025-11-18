using ECommerceApp.Context;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class RegisterRepo(EComDbContext context) : IRegisterRepo
    {
        private readonly EComDbContext _context = context;

        public async Task<UserRequest?> GetByIdAsync(int id)
        {
            var entity = await _context.Users
                .Include(u => u.Credentials)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return entity == null ? null : ToDto(entity);
        }

        public async Task<UserRequest> CreateAsync(UserRequest user)
        {
            var isUserExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.LoginId == user.LoginId);
            if (isUserExists)
                throw new InvalidOperationException("User with the same LoginId already exists.");

            var entity = ToEntity(user);

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<UserRequest?> UpdateAsync(UserRequest user)
        {
            var existingUser = await _context.Users
                .Include(u => u.Credentials)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                return null;

            existingUser.LoginId = user.LoginId;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;

            if (user.Credentials != null)
            {
                existingUser.Credentials ??= new UserCredentialEntity();
                existingUser.Credentials.Password = user.Credentials.Password;
            }

            await _context.SaveChangesAsync();
            return ToDto(existingUser);
        }

        private static UserRequest ToDto(UserEntity entity)
        {
            return new UserRequest
            {
                Id = entity.Id,
                LoginId = entity.LoginId,
                Email = entity.Email,
                FullName = entity.FullName,
                Credentials = entity.Credentials == null
                    ? new LoginModel()
                    : new LoginModel
                    {
                        Password = entity.Credentials.Password
                    }
            };
        }

        private static UserEntity ToEntity(UserRequest user)
        {
            return new UserEntity
            {
                Id = user.Id,
                LoginId = user.LoginId,
                Email = user.Email,
                FullName = user.FullName,
                Credentials = new UserCredentialEntity
                {
                    Password = user.Credentials?.Password ?? string.Empty
                }
            };
        }
    }
}

