using Newtonsoft.Json;

namespace PokeDex.models
{
    public class AbilityElement
    {
        [JsonProperty("ability")]
        public TypeClass Ability { get; set; }
    }
}
