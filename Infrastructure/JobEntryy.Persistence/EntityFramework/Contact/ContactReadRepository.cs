using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobEntryy.Persistence.EntityFramework
{
    public class ContactReadRepository : ReadRepository<Contact>, IContactReadRepository
    {
        public async Task<double> ContactPageCountAsync(double take)
        {
            using var context = new Context();

            double pageCount = Math.Ceiling(await context.Contacts.CountAsync() / take);
            return pageCount;
        }

        public async Task<List<Contact>> GetContactsWithPagedAsync(int take, int page)
        {
            using var context = new Context();

            List<Contact> contacts = await context.Contacts.OrderByDescending(x => x.Id).Skip((take - 1) * page).
                Take(take).ToListAsync();
            return contacts;
        }
    }
}
