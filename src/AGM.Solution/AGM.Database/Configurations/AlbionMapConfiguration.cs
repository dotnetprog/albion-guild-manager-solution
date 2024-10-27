using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AGM.Database.Configurations
{
    public class AlbionMapConfiguration : IEntityTypeConfiguration<AlbionMap>
    {
        public void Configure(EntityTypeBuilder<AlbionMap> builder)
        {
            var converter = new ValueConverter<AlbionMapId, Guid>(
                                           id => id.Value,
                                           guid => new AlbionMapId(guid));

            builder.HasKey(x => x.Id).HasAnnotation("SqlServer:Clustered", false);
            builder.Property(x => x.Id).HasConversion(converter);
            builder.Property(x => x.Type).HasConversion<string>();



        }
    }
}
