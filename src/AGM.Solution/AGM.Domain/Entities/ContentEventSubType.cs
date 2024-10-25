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
        public ContentEventType? ContentEventType { get; set; }

    }
}
