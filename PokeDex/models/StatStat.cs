using Newtonsoft.Json;

namespace PokeDex.models
{
    public class StatStat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
