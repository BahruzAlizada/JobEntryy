

using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class CityWriteRepository : WriteRepository<City>, ICityWriteRepository
    {
        public void Activity(City city)
        {
            using var context = new Context();

            if (city.Status)
                city.Status = false;
            else
                city.Status = true;

            context.Cities.Update(city);
            context.SaveChanges();
        }
    }
}
