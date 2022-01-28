using Newtonsoft.Json;

namespace PokeDex.models
{
    public class TypeClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
