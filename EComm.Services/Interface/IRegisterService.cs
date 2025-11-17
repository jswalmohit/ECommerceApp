using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface IRegisterService
    {
        Task<UserRequest?> GetUserByIdAsync(int id);
        Task<UserRequest> CreateUserAsync(UserRequest user);
        Task<UserRequest?> UpdateUserAsync(int id, UserRequest user);
    }
}
