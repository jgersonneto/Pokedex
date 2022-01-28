using System.Collections.ObjectModel;

namespace PokeDex.models.Factory
{
    public abstract class AbstractFactoryPokemon
    {
        public abstract ObservableCollection<Pokemon> GetAllPokemonMainPage();
        public abstract Pokemon SearchInApiForPokemonById(int id);
        public abstract Pokemon SearchInApiForPokemonByName(string name);
        public abstract Types SearchInApiForPokemonByType(string type);
        public abstract ObservableCollection<Pokemon> SearchInDBForPokemonById(int id);
        public abstract ObservableCollection<Pokemon> SearchInDBForPokemonByName(string name);
        public abstract ObservableCollection<Pokemon> SearchInDBForPokemonByType(string type);
        public abstract bool ThisPokemonExist(int id);
        public abstract bool ThisPokemonExistByName(string name);






    }
}
