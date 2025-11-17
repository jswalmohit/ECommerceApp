using ECommerceApp.Context;
using ECommerceApp.EComm.Repositories.Implementation;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Implementation;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.EntityFrameworkCore.Internal;

namespace ECommerceApp.Extension
{
    public static class IServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<EComDbContext>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IRegisterRepo, RegisterRepo>();
        }
    }
}
