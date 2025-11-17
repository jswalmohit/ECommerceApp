using ECommerceApp.Context;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Repositories.Interface;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class RegisterRepo(EComDbContext context): IRegisterRepo
    {
        private readonly EComDbContext _context = context;

        public async Task<UserRequest?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserRequest> CreateAsync(UserRequest user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserRequest?> UpdateAsync(UserRequest user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                return null;

            existingUser.LoginId = user.LoginId;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;

            await _context.SaveChangesAsync();
            return existingUser;
        }
    }
}

