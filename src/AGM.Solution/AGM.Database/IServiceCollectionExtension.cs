using AGM.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AGM.Database
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAGMDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AGMDBContext>(
                options =>
                {
                    options
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                         (so) => so.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null));

                }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            return services;
        }
    }
}
