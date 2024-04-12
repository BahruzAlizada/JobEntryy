using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface ICategoryReadRepository : IReadRepository<Category>
    {
        Task<List<Category>> GetActiveCategories();
        Task<List<Category>> GetActiveCachingCategories();
    }
}
