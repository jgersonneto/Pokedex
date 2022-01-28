using Newtonsoft.Json;

namespace PokeDex.models
{
    public class Other
    {
        [JsonProperty("dream_world")]
        public DreamWorld Dream_World { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }

        [JsonProperty("official-artwork")]
        public OfficialArtwork OfficialArtwork { get; set; }
    }
}
