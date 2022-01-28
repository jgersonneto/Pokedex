using Newtonsoft.Json;

namespace PokeDex.models
{
    public class TypeElement
    {
        [JsonProperty("type")]
        public TypeClass Type { get; set; }
    }
}
