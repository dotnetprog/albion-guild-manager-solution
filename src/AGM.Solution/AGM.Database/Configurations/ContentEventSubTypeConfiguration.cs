using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AGM.Database.Configurations
{
    public class ContentEventSubTypeConfiguration : IEntityTypeConfiguration<ContentEventSubType>
    {
        public void Configure(EntityTypeBuilder<ContentEventSubType> builder)
        {
            var converter = new ValueConverter<ContentEventSubTypeId, Guid>(
                                           id => id.Value,
                                           guid => new ContentEventSubTypeId(guid));

            builder.HasKey(x => x.Id).HasAnnotation("SqlServer:Clustered", false);
            builder.Property(x => x.Id).HasConversion(converter);
            builder.HasOne(x => x.ContentEventType).WithMany(e => e.SubTypes).HasForeignKey(x => x.ContentEventTypeId);

        }
    }
}
