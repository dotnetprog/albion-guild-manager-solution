using AGM.Database.Console.Seeder;
using AGM.Database.Context;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AGM.Database.Console.Commands
{
    public class DatabaseCommands
    {
        private const string BaseFolder = "Data";
        private const string BaseMapFolder = $"{BaseFolder}/Maps";
        private const string BaseContentSubTypeFolder = $"{BaseFolder}/ContentEventSubTypes";
        private readonly IServiceProvider RootserviceProvider;
        private readonly ILogger _logger;
        private IServiceProvider ScopedserviceProvider
        {
            get
            {
                var scope = RootserviceProvider.CreateScope();
                return scope.ServiceProvider;
            }
        }
        public DatabaseCommands(IServiceProvider serviceProvider, ILogger<DatabaseCommands> logger)
        {
            RootserviceProvider = serviceProvider;
            _logger = logger;
        }
        [Command("seed")]
        public async Task SeedDatabase()
        {
            var context = ScopedserviceProvider.GetService<AGMDBContext>();
            await AlbionMapSeeder.Seed(context, _logger, BaseMapFolder);
            await ContentEventTypeSeeder.Seed(context, _logger, BaseFolder);
            await ContentEventSubTypeSeeder.Seed(context, _logger, BaseContentSubTypeFolder);
        }


    }
}
