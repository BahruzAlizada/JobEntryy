using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface ICityReadRepository : IReadRepository<City>
    {
        Task<List<City>> GetActiveCachingCities();
        Task<List<City>> GetCitiesWithPagedAsync(int take, int page, string search);
    }
}
