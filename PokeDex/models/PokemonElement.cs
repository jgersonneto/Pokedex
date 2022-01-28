using Newtonsoft.Json;

namespace PokeDex.models
{
    public class PokemonElement
    {
        [JsonProperty("pokemon")]
        public PokemonPokemon Pokemon { get; set; }
    }
}
