using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace AGM.Database.Context
{
    public class AGMDBContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public AGMDBContext(DbContextOptions options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }
        public DbSet<ContentEvent> ContentEvents { get; set; }
        public DbSet<ContentEventType> ContentEventTypes { get; set; }
        public DbSet<ContentEventSubType> ContentEventSubTypes { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<AlbionMap> Maps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tenant = _tenantProvider.GetCurrentTenant();
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            modelBuilder.Entity<ContentEvent>().HasQueryFilter(x => x.Tenant.DiscordServerId == tenant.DiscordServerId);
        }
    }
}
