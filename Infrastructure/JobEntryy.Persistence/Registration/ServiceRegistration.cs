
using JobEntryy.Application.Abstract;
using JobEntryy.Application.Abstract.Categories;
using JobEntryy.Persistence.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace JobEntryy.Persistence.Registration
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddScoped<IJobReadRepository, JobReadRepository>();
            services.AddScoped<IJobWriteRepository, JobWriteRepository>();

            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<ICityReadRepository, CityReadRepository>();
            services.AddScoped<ICityWriteRepository, CityWriteRepository>();

            services.AddScoped<IJobTypeReadRepository, JobTypeReadRepository>();
            services.AddScoped<IJobTypeWriteRepository, JobTypeWriteRepository>();

            services.AddScoped<IExperienceReadRepository, ExperienceReadRepository>();
            services.AddScoped<IExperienceWriteRepository, ExperienceWriteRepository>();

            services.AddScoped<IFaqReadRepository, FaqReadRepository>();
            services.AddScoped<IFaqWriteRepository, FaqWriteRepository>();

            services.AddScoped<ISocialMediaReadRepository, SocialMediaReadRepository>();
            services.AddScoped<ISocialMediaWriteRepository, SocialMediaWriteRepository>();

            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();

            services.AddScoped<IAboutReadRepository, AboutReadRepository>();
            services.AddScoped<IAboutWriteRepository, AboutWriteRepository>();
        }
    }
}
