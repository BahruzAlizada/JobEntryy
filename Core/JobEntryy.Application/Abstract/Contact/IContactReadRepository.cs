using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IContactReadRepository : IReadRepository<Contact>
    {
        Task<List<Contact>> GetContactsWithPagedAsync(int take, int page);
        Task<double> ContactPageCountAsync(double take);
    }
}
