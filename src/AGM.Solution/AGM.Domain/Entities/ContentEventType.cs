﻿namespace AGM.Domain.Entities
{
    [StronglyTypedId()]
    public partial struct ContentEventTypeId
    {
    }
    public class ContentEventType
    {
        public ContentEventTypeId Id { get; set; }
        public string? Name { get; set; }
        public ICollection<ContentEventSubType> SubTypes { get; set; } = new List<ContentEventSubType>();
    }
}
