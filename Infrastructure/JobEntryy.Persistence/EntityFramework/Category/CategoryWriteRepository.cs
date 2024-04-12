using JobEntryy.Application.Abstract.Categories;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class CategoryWriteRepository : WriteRepository<Category>, ICategoryWriteRepository
    {
        public void Activity(Category category)
        {
            using var context = new Context();

            if (category.Status)
                category.Status = false;
            else
                category.Status = true;

            context.Categories.Update(category);
            context.SaveChanges();
        }
    }
}
