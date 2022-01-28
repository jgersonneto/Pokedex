using Newtonsoft.Json;
using System.Collections.Generic;

namespace PokeDex.models
{
    public class Types
    {
        [JsonProperty("pokemon")]
        public List<PokemonElement> Pokemon { get; set; }
    }
}
