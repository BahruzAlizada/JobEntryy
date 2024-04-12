using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace JobEntryy.Persistence.EntityFramework
{
    public class CityReadRepository : ReadRepository<City>, ICityReadRepository
    {
        private readonly IMemoryCache memoryCache;
        public CityReadRepository(IMemoryCache memoryCache)
        {
            this .memoryCache = memoryCache;
        }
        public async Task<List<City>> GetActiveCachingCities()
        {
            using var context = new Context();

            const string cacheKey = "cities";
            List<City> cities;

            if (!memoryCache.TryGetValue(cacheKey, out cities))
            {
                cities = await context.Cities.Where(x => x.Status).OrderBy(x => x.Name).ToListAsync();

                memoryCache.Set(cacheKey, cities, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(4),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(12),
                    Priority = CacheItemPriority.High
                });
            }
            return cities;
        }
    }
}
