using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse?> GenerateJwtTokenAsync(LoginRequest loginRequest);
    }
}

