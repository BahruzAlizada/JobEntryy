
using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace JobEntryy.Persistence.EntityFramework
{
    public class CategoryReadRepository : ReadRepository<Category>, ICategoryReadRepository
    {
        private readonly IMemoryCache memoryCache;
        public CategoryReadRepository(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
     

        public async Task<List<Category>> GetActiveCachingCategories()
        {
            using var context = new Context();

            const string cachedKey = "categories";
            List<Category> categories;

            if(!memoryCache.TryGetValue(cachedKey, out categories))
            {
                categories = await context.Categories.Where(x => x.Status).ToListAsync();

                memoryCache.Set(cachedKey, cachedKey, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(9),
                    Priority = CacheItemPriority.Normal
                });
            }
            return categories;
        }

        public async Task<List<Category>> GetActiveCategories()
        {
            using var context = new Context();

            List<Category> categories = await context.Categories.Where(x => x.Status).ToListAsync();
            return categories;
        }

    }
}
