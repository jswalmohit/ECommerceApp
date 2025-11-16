using ECommerceApp.Context;

namespace ECommerceApp.Extension
{
    public static class IServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<EComDbContext>();
        }
    }
}
