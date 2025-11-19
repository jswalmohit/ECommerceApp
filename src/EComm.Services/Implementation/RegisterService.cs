using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Interface;

namespace ECommerceApp.EComm.Services.Implementation
{
    public class RegisterService: IRegisterService
    {
        private readonly IRegisterRepo _registerRepository;

        public RegisterService(IRegisterRepo userRepository)
        {
            _registerRepository = userRepository;
        }

        public async Task<UserRequest?> GetUserByIdAsync(int id)
        {
            return await _registerRepository.GetByIdAsync(id);
        }

        public async Task<UserRequest> CreateUserAsync(UserRequest user)
        {
            return await _registerRepository.CreateAsync(user);
        }

        public async Task<UserRequest?> UpdateUserAsync(int id, UserRequest user)
        {
            user.Id = id;
            return await _registerRepository.UpdateAsync(user);
        }
    }
}
