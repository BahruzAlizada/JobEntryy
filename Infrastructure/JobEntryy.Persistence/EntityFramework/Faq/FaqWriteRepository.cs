using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;

namespace JobEntryy.Persistence.EntityFramework
{
    public class FaqWriteRepository : WriteRepository<Faq>, IFaqWriteRepository
    {
        public void Activity(Faq faq)
        {
            using var context = new Context();

            if (faq.Status)
                faq.Status = false;
            else
                faq.Status = true;

            context.Faqs.Update(faq);
            context.SaveChanges();
        }
    }
}
