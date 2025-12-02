using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.EComm.Data.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}

