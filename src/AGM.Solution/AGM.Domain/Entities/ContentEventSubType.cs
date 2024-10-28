using Newtonsoft.Json;

namespace AGM.Domain.Entities
{
    [StronglyTypedId()]
    public partial struct ContentEventSubTypeId
    {
    }
    public class ContentEventSubType
    {
        public ContentEventSubTypeId Id { get; set; }
        public string? Name { get; set; }
        public string? Emoji { get; set; }

        public ContentEventTypeId? ContentEventTypeId { get; set; }
        [JsonIgnore]
        public ContentEventType? ContentEventType { get; set; }

    }
}
