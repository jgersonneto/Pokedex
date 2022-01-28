using Microsoft.Toolkit.Mvvm.ComponentModel;
using PokeDex.models;
using PokeDex.models.Factory;
using System.Collections.ObjectModel;

namespace PokeDex.viewmodels
{
    public class PokemonDetails : ObservableObject
    {
        private Pokemon poke_ = new Pokemon();
        private ObservableCollection<Pokemon> pokemons_ = new ObservableCollection<Pokemon>();

        private FactoryPokemon fPokemon = new FactoryPokemon();
        public PokemonDetails()
        {
        }

        public Pokemon Poke
        {
            get => poke_;
            set => SetProperty(ref poke_, value);
        }
        public ObservableCollection<Pokemon> Pokemons
        {
            get => pokemons_;
            set => SetProperty(ref pokemons_, value);
        }

    }
}
