using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract.Categories
{
    public interface ICategoryWriteRepository : IWriteRepository<Category>
    {
        void Activity(Category category);
    }
}
