using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface IFaqWriteRepository : IWriteRepository<Faq>
    {
        void Activity(Faq faq);
    }
}
