using Newtonsoft.Json;

namespace PokeDex.models
{
    public class StatElement
    {
        [JsonProperty("base_stat")]
        public int Base_Stat { get; set; }

        [JsonProperty("stat")]
        public StatStat Stat { get; set; }
    }
}
