using AGM.Domain.Enums;

namespace AGM.Domain.Entities
{

    [StronglyTypedId()]
    public partial struct AlbionMapId
    {
    }
    public class AlbionMap
    {
        public AlbionMapId Id { get; set; }

        public string? Name { get; set; }

        public AlbionMapType? Type { get; set; }
    }
}
