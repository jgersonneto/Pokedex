using PokeDex.models.repository.db;
using System.Collections.ObjectModel;

namespace PokeDex.models.Factory
{
    public class FactoryPokemon : AbstractFactoryPokemon
    {
        public override ObservableCollection<Pokemon> GetAllPokemonMainPage()
        {
            ObservableCollection<Pokemon> pokemons = new ObservableCollection<Pokemon>();
            pokemons = DBPokemonTable.GetAllAttributesForMainPage();
            pokemons = DBTypesTable.GetAllAttributesForMainPage(pokemons);
            return pokemons;
        }

        public override Pokemon SearchInApiForPokemonById(int id)
        {
            FactoryApi fApi = new FactoryApi();
            var pokemon = fApi.SearchPokemonApiById(id);

            return pokemon;
        }

        public override Types SearchInApiForPokemonByType(string type)
        {
            FactoryApi fApi = new FactoryApi();
            Types typePoke = fApi.SearchTypesApiByType(type);

            return typePoke;
        }
        public override Pokemon SearchInApiForPokemonByName(string name)
        {
            FactoryApi fApi = new FactoryApi();
            var pokemon = fApi.SearchPokemonApiByName(name);

            return pokemon;
        }
        public override ObservableCollection<Pokemon> SearchInDBForPokemonByName(string name)
        {
            ObservableCollection<Pokemon> pokemons = new ObservableCollection<Pokemon>();
            pokemons = DBPokemonTable.SearchPokemonByNameForMainPage(name);
            pokemons = DBTypesTable.SearchInTypesByNameForMainPage(pokemons);

            return pokemons;

        }
        public override ObservableCollection<Pokemon> SearchInDBForPokemonById(int id)
        {
            ObservableCollection<Pokemon> pokemons = new ObservableCollection<Pokemon>();
            pokemons = DBPokemonTable.SearchPokemonByIdForMainPage(id);
            pokemons = DBTypesTable.SearchInTypesByIdForMainPage(pokemons);

            return pokemons;
        }
        public override ObservableCollection<Pokemon> SearchInDBForPokemonByType(string type)
        {
            ObservableCollection<Pokemon> pokemons = new ObservableCollection<Pokemon>();
            pokemons = DBPokemonTable.SearchPokemonByType(type);
            pokemons = DBTypesTable.SearchInTypesByNameForMainPage(pokemons);

            return pokemons;
        }
        public override bool ThisPokemonExist(int id)
        {
            bool result = false;
            var pokemons = DBPokemonTable.SearchOnePokemonById(id);
            if (pokemons.Count != 0)
            {
                result = true;
            }
            return result;
        }
        public override bool ThisPokemonExistByName(string name)
        {
            bool result = false;
            var pokemons = DBPokemonTable.SearchOnePokemonByName(name);
            if (pokemons.Count != 0)
            {
                result = true;
            }
            return result;
        }
    }
}
