using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AGM.Database.Configurations
{
    public class ContentEventConfiguration : IEntityTypeConfiguration<ContentEvent>
    {
        public void Configure(EntityTypeBuilder<ContentEvent> builder)
        {
            var converter = new ValueConverter<ContentEventId, Guid>(
                                         id => id.Value,
                                         guid => new ContentEventId(guid));

            builder.HasKey(x => x.Id).HasAnnotation("SqlServer:Clustered", false);
            builder.Property(x => x.Id).HasConversion(converter);

            builder.HasOne(x => x.Tenant).WithMany().HasForeignKey(x => x.TenantId);
            builder.HasOne(x => x.AlbionMap).WithMany().HasForeignKey(x => x.AlbionMapId);
            builder.HasOne(x => x.SubType).WithMany().HasForeignKey(x => x.SubTypeId);
            builder.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.TypeId);
        }
    }
}
