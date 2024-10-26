using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace AGM.Database.Context
{
    public class AGMDBContext : DbContext
    {
        public AGMDBContext(DbContextOptions options) : base(options) { }
        public DbSet<ContentEvent> ContentEvents { get; set; }
        public DbSet<ContentEventType> ContentEventTypes { get; set; }
        public DbSet<ContentEventSubType> ContentContentEventSubTypes { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<AlbionMap> Maps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
           modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
