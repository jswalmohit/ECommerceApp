using ECommerceApp.EComm.Repositories.Specifications;

namespace ECommerceApp.EComm.Repositories.Interface
{
    public interface ISpecificationRepository<T> where T : class
    {
        Task<T?> GetBySpecificationAsync(ISpecification<T> specification);
        Task<IEnumerable<T>> GetAllBySpecificationAsync(ISpecification<T> specification);
        Task<int> CountBySpecificationAsync(ISpecification<T> specification);
    }
}

