using Newtonsoft.Json;

namespace PokeDex.models
{
    public class Sprites
    {
        [JsonProperty("back_default")]
        public string Back_Default { get; set; }

        [JsonProperty("back_female")]
        public object Back_Female { get; set; }

        [JsonProperty("back_shiny")]
        public string Back_Shiny { get; set; }

        [JsonProperty("back_shiny_female")]
        public object Back_Shiny_Female { get; set; }

        [JsonProperty("front_default")]
        public string Front_Default { get; set; }

        [JsonProperty("front_female")]
        public object Front_Female { get; set; }

        [JsonProperty("front_shiny")]
        public string Front_Shiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public object Front_Shiny_Female { get; set; }

        [JsonProperty("other")]
        public Other Other { get; set; }
    }
}
