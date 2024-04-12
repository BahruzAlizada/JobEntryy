using JobEntryy.Application.Abstract;
using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class FaqReadRepository : ReadRepository<Faq>,IFaqReadRepository 
    {
    }
}
