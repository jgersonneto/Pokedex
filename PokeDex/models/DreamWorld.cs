using Newtonsoft.Json;

namespace PokeDex.models
{
    public class DreamWorld
    {
        [JsonProperty("front_default")]
        public string Front_Default { get; set; }

        [JsonProperty("front_female")]
        public object Front_Female { get; set; }
    }
}
