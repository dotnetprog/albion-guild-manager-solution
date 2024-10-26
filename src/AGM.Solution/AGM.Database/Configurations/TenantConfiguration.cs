using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AGM.Database.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            var converter = new ValueConverter<TenantId, Guid>(
                                            id => id.Value,
                                            guid => new TenantId(guid));
            builder.HasKey(x => x.Id).HasAnnotation("SqlServer:Clustered", false);
            builder.Property(x => x.Id).HasConversion(converter);
        }
    }
}
