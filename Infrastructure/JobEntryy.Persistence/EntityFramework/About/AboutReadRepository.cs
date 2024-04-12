using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class AboutReadRepository : ReadRepository<About>, IAboutReadRepository
    {
    }
}
