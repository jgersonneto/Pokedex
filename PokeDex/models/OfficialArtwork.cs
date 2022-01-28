using Newtonsoft.Json;

namespace PokeDex.models
{
    public class OfficialArtwork
    {
        [JsonProperty("front_default")]
        public string Front_Default { get; set; }
    }
}
