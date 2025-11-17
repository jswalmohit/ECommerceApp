using ECommerceApp.EComm.Data.Entities;

namespace ECommerceApp.EComm.Repositories.Interface
{
    public interface IAuthRepo
    {
        Task<UserEntity?> ValidateUserAsync(string loginId, string password);
    }
}

