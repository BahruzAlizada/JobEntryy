using JobEntryy.Application.Abstract;
using JobEntryy.Domain.Entities;
using JobEntryy.Persistence.Concrete;
using JobEntryy.Persistence.Repositories;


namespace JobEntryy.Persistence.EntityFramework
{
    public class SocialMediaWriteRepository : WriteRepository<SocialMedia>, ISocialMediaWriteRepository
    {
        public void Activity(SocialMedia socialMedia)
        {
            using var context = new Context();

            if (socialMedia.Status)
                socialMedia.Status = false;
            else
                socialMedia.Status = true;  

            context.SocialMedias.Update(socialMedia);
            context.SaveChanges();
        }
    }
}
