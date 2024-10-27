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
                cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly);
            })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            return service;
        }
    }
}
