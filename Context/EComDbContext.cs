using ECommerceApp.EComm.Commons.Modals;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Context
{
    public class EComDbContext:DbContext
    {
        public EComDbContext(DbContextOptions<EComDbContext> options) :base(options)
        {
            
        }
        public DbSet<UserRequest> Users { get; set; }
    }
}
