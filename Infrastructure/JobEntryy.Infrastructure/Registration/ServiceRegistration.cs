using JobEntryy.Infrastructure.Abstract;
using JobEntryy.Infrastructure.Concrete;
using JobEntryy.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JobEntryy.Infrastructure.Registration
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<LanguageService>();
            services.AddScoped<IPhotoService,PhotoService>();
        }
    }
}
