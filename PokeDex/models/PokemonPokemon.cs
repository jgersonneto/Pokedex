using Newtonsoft.Json;

namespace PokeDex.models
{
    public class PokemonPokemon
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
