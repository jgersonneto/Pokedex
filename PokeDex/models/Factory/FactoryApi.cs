using PokeDex.models.repository.api;
using System.Threading.Tasks;

namespace PokeDex.models.Factory
{
    public class FactoryApi : AbstractFactoryApi
    {
        public override Pokemon SearchPokemonApiById(int id)
        {
            var pokemon = Task.Run(async () => await ApiService.ApiPokeById(id)).Result;
            return pokemon;
        }


        public override Types SearchTypesApiByType(string type)
        {
            Types typePoke = Task.Run(async () => await ApiService.ApiPokeByTypes(type)).Result;
            return typePoke;
        }

        public override Pokemon SearchPokemonApiByName(string name)
        {

            var pokemon = Task.Run(async () => await ApiService.ApiPokeByName(name)).Result;
            return pokemon;

        }
    }
}
