using AGM.DiscordBot.Interactions.AutoCompletions;
using AGM.Domain.Entities;
using Discord.Interactions;

namespace AGM.DiscordBot.Interactions.Module.Roads.Input
{
    public class RoadPost
    {
        [ComplexParameterCtor]
        public RoadPost(
            [Autocomplete(typeof(UtcTimeAutoCompleteHandler))] string expiresIn,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string startingMap,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string map2,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string map3 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map4 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map5 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map6 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map7 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map8 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map9 = null,
            [Autocomplete(typeof(AlbionMapAutoCompleteHandler))] string? map10 = null
            )
        {
            this.ExpiresIn = expiresIn;
            this.StartingMap = startingMap;
            this.Map2 = map2;
            this.Map3 = map3;
            this.Map4 = map4;
            this.Map5 = map5;
            this.Map6 = map6;
            this.Map7 = map7;
            this.Map8 = map8;
            this.Map9 = map9;
            this.Map10 = map10;

        }
        [RequiredInput(true)]
        public string ExpiresIn { get; set; }
        [RequiredInput(true)]
        public string StartingMap { get; set; }
        [RequiredInput(true)]
        public string Map1 { get; set; }
        [RequiredInput(true)]
        public string Map2 { get; set; }
        [RequiredInput(false)]
        public string? Map3 { get; set; }
        [RequiredInput(false)]
        public string? Map4 { get; set; }
        [RequiredInput(false)]
        public string? Map5 { get; set; }
        [RequiredInput(false)]
        public string? Map6 { get; set; }
        [RequiredInput(false)]
        public string? Map7 { get; set; }
        [RequiredInput(false)]
        public string? Map8 { get; set; }
        [RequiredInput(false)]
        public string? Map9 { get; set; }
        [RequiredInput(false)]
        public string? Map10 { get; set; }


        public CustomResult<List<AlbionMapId>> GetMaps()
        {
            var result = new CustomResult<List<AlbionMapId>>()
            {
                Value = new List<AlbionMapId>()
            };
            var stringMaps = new string?[] { StartingMap, Map2, Map3, Map4, Map5, Map6, Map7, Map8, Map9, Map10 };


            for (int i = 0; i < stringMaps.Length; i++)
            {
                var map = stringMaps[i];
                if (string.IsNullOrWhiteSpace(map)) { continue; }
                if (Guid.TryParse(map, out Guid MapGuid))
                {
                    result.Value.Add(new AlbionMapId(MapGuid));
                }
                else
                {
                    var paramName = i == 0 ? nameof(StartingMap) : $"Map{i + 1}";
                    result.Errors.Add($"{paramName} must be selected from the autocomplete.");
                }
            }
            return result;


        }

    }

    public class CustomResult<T>
    {
        public CustomResult()
        {
            this.Errors = new List<string>();
        }
        public T Value { get; set; }

        public List<string> Errors { get; set; }
    }
}
