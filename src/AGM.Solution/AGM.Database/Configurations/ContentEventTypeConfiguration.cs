using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AGM.Database.Configurations
{
    public class ContentEventTypeConfiguration : IEntityTypeConfiguration<ContentEventType>
    {
        public void Configure(EntityTypeBuilder<ContentEventType> builder)
        {
            var converter = new ValueConverter<ContentEventTypeId, Guid>(
                                           id => id.Value,
                                           guid => new ContentEventTypeId(guid));

            builder.HasKey(x => x.Id).HasAnnotation("SqlServer:Clustered", false);
            builder.Property(x => x.Id).HasConversion(converter);
            builder.HasMany(x => x.SubTypes)
                .WithOne(e => e.ContentEventType)
                .HasForeignKey(e => e.ContentEventTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
