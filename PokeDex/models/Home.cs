using Newtonsoft.Json;

namespace PokeDex.models
{
    public class Home
    {
        [JsonProperty("front_default")]
        public string Front_Default { get; set; }

        [JsonProperty("front_female")]
        public object Front_Female { get; set; }

        [JsonProperty("front_shiny")]
        public string Front_Shiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public object Front_Shiny_Female { get; set; }
    }
}
