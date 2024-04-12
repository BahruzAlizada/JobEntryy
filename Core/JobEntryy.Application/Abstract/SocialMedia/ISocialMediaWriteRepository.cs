using JobEntryy.Application.Repositories;
using JobEntryy.Domain.Entities;

namespace JobEntryy.Application.Abstract
{
    public interface ISocialMediaWriteRepository : IWriteRepository<SocialMedia>
    {
        void Activity(SocialMedia socialMedia);
    }
}
