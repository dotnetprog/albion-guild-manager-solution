using AGM.Application.Features.Caching.Behaviors;
using AGM.Application.Features.Logging.Behaviors;
using AGM.Application.Features.Transactional.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AGM.Application
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAGMApplicationLayer(this IServiceCollection service)
        {
            service.AddMediatR(cfg =>
            {
                cfg.Lifetime = ServiceLifetime.Scoped;
                cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly);
            })
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
            .AddMemoryCache();
            return service;
        }
    }
}
