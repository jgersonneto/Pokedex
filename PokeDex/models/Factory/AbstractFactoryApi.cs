namespace PokeDex.models.Factory
{
    public abstract class AbstractFactoryApi
    {
        public abstract Pokemon SearchPokemonApiById(int id);

        public abstract Types SearchTypesApiByType(string type);

        public abstract Pokemon SearchPokemonApiByName(string name);

    }
}
