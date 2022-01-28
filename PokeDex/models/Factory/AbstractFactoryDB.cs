namespace PokeDex.models.Factory
{
    public abstract class AbstractFactoryDB
    {
        public abstract void InitializeDB();
        public abstract void AddPokemonToDB(Pokemon pokemon);
        public abstract void InitializationFirstPokemons();

    }
}
