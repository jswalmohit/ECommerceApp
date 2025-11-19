using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Repositories.Interface
{
    public interface IRegisterRepo
    {
        Task<UserRequest?> GetByIdAsync(int id);
        Task<UserRequest> CreateAsync(UserRequest user);
        Task<UserRequest?> UpdateAsync(UserRequest user);
    }
}
