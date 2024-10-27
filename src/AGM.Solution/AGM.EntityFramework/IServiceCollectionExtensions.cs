using AGM.Domain.Abstractions;
using AGM.EntityFramework.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AGM.EntityFramework
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {

            return services
                .AddTransient<ITenantRepository, EFTenantRepository>()
                .AddTransient<IContentEventRepository, EFContentEventRepository>()
                .AddTransient<IContentEventTypeRepository, EFContentEventTypeRepository>()
                .AddTransient<IUnitOfWork, EFUnitOfWork>()
                .AddTransient<IAlbionMapRepository, EFAlbionMapRepository>();

        }
    }
}
