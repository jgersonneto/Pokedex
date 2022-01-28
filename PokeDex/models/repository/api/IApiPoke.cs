using Refit;
using System.Threading.Tasks;

namespace PokeDex.models.repository.api
{
    public interface IApiPoke
    {
        [Get("/pokemon/{id}")]
        Task<Pokemon> GetPokemonAsyncById(int id);

        [Get("/pokemon/{name}")]
        Task<Pokemon> GetPokemonAsyncByName(string name);

        [Get("/type/{name}")]
        Task<Types> GetTypesAsyncByType(string name);
    }
}