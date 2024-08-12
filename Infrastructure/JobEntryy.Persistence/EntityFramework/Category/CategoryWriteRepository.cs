using JobEntryy.Application.Abstract.Categories;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class CategoryWriteRepository : WriteRepository<Category>, ICategoryWriteRepository
    {
      
    }
}
