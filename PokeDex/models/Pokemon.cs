using Newtonsoft.Json;
using System.Collections.Generic;

namespace PokeDex.models
{
    public class Pokemon
    {
        public Pokemon()
        {
            List<StatElement> stats = new List<StatElement>();
            List<AbilityElement> abilities = new List<AbilityElement>();
            List<TypeElement> types = new List<TypeElement>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("base_experience")]
        public int Base_Experience { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("stats")]
        public List<StatElement> Stats { get; set; }

        [JsonProperty("abilities")]
        public List<AbilityElement> Abilities { get; set; }

        [JsonProperty("types")]
        public List<TypeElement> Types { get; set; }

        [JsonProperty("sprites")]
        public Sprites Sprites { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
