using PokeDex.models.repository.db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeDex.models.Factory
{
    public class FactoryDB : AbstractFactoryDB
    {
        public override void AddPokemonToDB(Pokemon pokemon)
        {
            DBPokemonTable.AddPokemonToDB(pokemon);
            DBAbilitiesTable.AddAbilitiesToDB(pokemon);
            DBTypesTable.AddTypesToDB(pokemon);
        }

        public override void InitializationFirstPokemons()
        {
            int pikachu = 25, bulbasaur = 1, charmander = 4, squirtle = 7;
            List<int> pokemons = new List<int>
            {
                pikachu,
                bulbasaur,
                squirtle,
                charmander
            };

            pokemons.ForEach((value) =>
            {
                FactoryApi fApi = new FactoryApi();
                var pokemon = fApi.SearchPokemonApiById(value);
                AddPokemonToDB(pokemon);
            });
        }

        public override void InitializeDB()
        {
            var result1 = Task.Run(async () => await DBPokemonTable.InitializeTablePokemonDB()).Result;
            var result2 = Task.Run(async () => await DBAbilitiesTable.InitializeTableAbilitiesDB()).Result;
            var result3 = Task.Run(async () => await DBTypesTable.InitializeTableTypesDB()).Result;
        }

    }
}
