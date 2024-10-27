using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using AGM.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AGM.Database.Factory
{
    public class AGMDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AGMDBContext>
    {
        private const string ASPCORE_ENV_NAME = "ASPNETCORE_ENVIRONMENT";
        private readonly ITenantProvider _tenantProvider = new MemoryTenantProvider();
        public AGMDBContext CreateDbContext(string[] args)
        {
            _tenantProvider.SetActiveTenant(new Tenant
            {
                Id = TenantId.Empty
            });

            Console.WriteLine("Inputs:");
            Console.WriteLine(string.Join("|", args));
            var environment = Environment.GetEnvironmentVariable(ASPCORE_ENV_NAME);
            var indexEnv = Array.IndexOf(args, "--environment");
            if (indexEnv > -1)
            {
                environment = args[indexEnv + 1];
            }


            return Create(
                Directory.GetCurrentDirectory(),
                environment,
                "DefaultConnection");
        }
        private AGMDBContext Create(string basePath, string environmentName, string connectionStringName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString(connectionStringName);
            Console.WriteLine($"Environment: {environmentName}");

            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException(
                    $"Could not find a connection string named '{connectionStringName}'.");
            }
            else
            {
                var optionsBuilder = new DbContextOptionsBuilder<AGMDBContext>();
                optionsBuilder.UseSqlServer(connstr);
                return new AGMDBContext(optionsBuilder.Options, _tenantProvider);
            }
        }
    }
}
