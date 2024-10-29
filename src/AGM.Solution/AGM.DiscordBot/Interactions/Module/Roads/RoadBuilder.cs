using AGM.Domain.Entities;
using AGM.Domain.Enums;
using Discord;

namespace AGM.DiscordBot.Interactions.Module.Roads
{
    public class RoadBuilder
    {
        private static Dictionary<AlbionMapType, string> Icons = new Dictionary<AlbionMapType, string>()
        {
            { AlbionMapType.Bluezone, "<:bluezone:1300874181105418260>"},
            { AlbionMapType.RedZone, "<:redzone:1300874164487585883>"},
            { AlbionMapType.YellowZone, "<:yellowzone:1300874149819846808>"},
            { AlbionMapType.Road, "<:avaroad:1300874131834671116>"},
            { AlbionMapType.Blackzone, "<:blackzone:1300874116282323026>"},
            { AlbionMapType.City, "<:bluezone:1300874181105418260>"},
        };
        private string Separator = "<a:rightarrow:1300887699951063132>";
        private List<AlbionMap> Maps = new List<AlbionMap>();
        private DateTime ExpiresAt { get; set; }
        public RoadBuilder AddMap(AlbionMap Map)
        {
            Maps.Add(Map);
            return this;
        }

        public RoadBuilder WithExpiresAt(DateTime expiresAt)
        {
            this.ExpiresAt = expiresAt;
            return this;
        }

        public string BuildString()
        {
            DateTimeOffset timestamp = DateTime.SpecifyKind(ExpiresAt, DateTimeKind.Utc);
            var timeTag = TimestampTag.FormatFromDateTimeOffset(timestamp, TimestampTagStyles.Relative);
            var roadStrings = Maps.Select(m => $"{Icons[m.Type.Value]} {m.Name}").ToList();
            var roadFormatted = string.Join(Separator, roadStrings);
            return $"Expires In: {timeTag} | {roadFormatted}";

        }



    }
}
