using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PokeDex.models;
using PokeDex.models.Factory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace PokeDex.viewmodels
{
    public class MainPageViewModels : ObservableObject

    {
        private FactoryDB fDB = new FactoryDB();
        private FactoryPokemon fPokemon = new FactoryPokemon();
        private ObservableCollection<Pokemon> pokemons_ = new ObservableCollection<Pokemon>();
        private ObservableCollection<Pokemon> otherListPokemons_ = new ObservableCollection<Pokemon>();
        private List<Pokemon> listPokemon_ = new List<Pokemon>();
        private Visibility visible_ = Visibility.Collapsed;
        private Visibility buttonLoadVisible_ = Visibility.Collapsed;
        private string buscarPokemon_;
        private bool isVisibleButton_ = true;
        private Pokemon poke_;


        public ObservableCollection<Pokemon> Pokemons
        {
            get => pokemons_;
            set => SetProperty(ref pokemons_, value);
        }
        public ObservableCollection<Pokemon> OtherListPokemons
        {
            get => otherListPokemons_;
            set => SetProperty(ref otherListPokemons_, value);
        }
        public List<Pokemon> ListPokemon
        {
            get => listPokemon_;
            set => SetProperty(ref listPokemon_, value);
        }
        public string BuscarPokemon
        {
            get => buscarPokemon_;
            set => SetProperty(ref buscarPokemon_, value);
        }
        public bool IsVisibleButton
        {
            get => isVisibleButton_;
            set => SetProperty(ref isVisibleButton_, value);
        }
        public Visibility IsVisible
        {
            get => visible_;
            set => SetProperty(ref visible_, value);
        }
        public Visibility ButtonLoadVisible
        {
            get => buttonLoadVisible_;
            set => SetProperty(ref buttonLoadVisible_, value);
        }
        public Pokemon Poke
        {
            get => poke_;
            set => SetProperty(ref poke_, value);
        }
        public ICommand SearchPokemon { get; }
        private async void SearchByIdTypeName()
        {
            if (!BuscarPokemon.Equals(""))
            {
                IsVisibleButton = false;
                if (BuscarPokemon.All(char.IsDigit))
                {
                    SearchId();
                }
                else if (BuscarPokemon.All(char.IsLetter)
                       && (BuscarPokemon.Equals("normal") || BuscarPokemon.Equals("fighting") || BuscarPokemon.Equals("flying") || BuscarPokemon.Equals("poison")
                       || BuscarPokemon.Equals("ground") || BuscarPokemon.Equals("rock") || BuscarPokemon.Equals("bug") || BuscarPokemon.Equals("ghost")
                       || BuscarPokemon.Equals("steel") || BuscarPokemon.Equals("fire") || BuscarPokemon.Equals("water") || BuscarPokemon.Equals("grass")
                       || BuscarPokemon.Equals("electric") || BuscarPokemon.Equals("psychic") || BuscarPokemon.Equals("ice") || BuscarPokemon.Equals("dragon")
                       || BuscarPokemon.Equals("dark") || BuscarPokemon.Equals("fairy") || BuscarPokemon.Equals("unknown") || BuscarPokemon.Equals("shadow")))
                {
                    SearchType();
                }
                else if (BuscarPokemon.All(char.IsLetter) || BuscarPokemon.Equals("ho-oh") || BuscarPokemon.Equals("porygon2"))
                {
                    SearchName();
                }
                else
                {
                    BuscarPokemon = "";
                    string msg = "Pokemon inválido! Por favor, digite um ID, NOME ou TIPO do pokemon para localiza-lo! \n Atenção!" +
                                 " \n Para localizar pokemon por ID digite apenas Números; \n Para localizar pokemon por NOME digite " +
                                 "apenas letras ou o nome completo do pokemon; \n Para localizar pokemon por TIPO digite apenas o tipo do pokemon.";
                    ValidationMessege(msg);
                }

            }
            else
            {
                BuscarPokemon = "";
                string msg = "Pokemon inválido! Por favor, digite um ID, NOME ou TIPO do pokemon, para localiza-lo!";
                ValidationMessege(msg);
            }
            IsVisibleButton = true;
        }
        private async void SearchId()
        {
            // Procurar primeiro no banco de dados para depois ir na APIPOKE

            Pokemons.Clear();
            ListPokemon.Clear();
            VisibleGo();
            var auxId = short.Parse(BuscarPokemon);

            await Task.Run(() =>
            {

                if (!fPokemon.ThisPokemonExist(auxId))
                {
                    var pokemonAPI = fPokemon.SearchInApiForPokemonById(auxId);
                    if (pokemonAPI != null && pokemonAPI.Id <= 251)
                    {
                        fDB.AddPokemonToDB(pokemonAPI);
                    }
                }
            });

            var pokemonDB = fPokemon.SearchInDBForPokemonById(auxId);
            foreach (Pokemon p in pokemonDB)
            {
                ListPokemon.Add(p);
            }
            searchForTenPages();
            VisibleGo();
            BuscarPokemon = "";



        }
        private async void SearchType()
        {
            BuscarPokemon = BuscarPokemon.ToLower();

            Pokemons.Clear();
            ListPokemon.Clear();
            VisibleGo();

            await Task.Run(() =>
            {
                var pokemonTypes = fPokemon.SearchInApiForPokemonByType(BuscarPokemon);

                if (pokemonTypes != null)
                {
                    foreach (PokemonElement pokeType in pokemonTypes.Pokemon)
                    {
                        if (!fPokemon.ThisPokemonExistByName(pokeType.Pokemon.Name))
                        {
                            var pokemonApi = fPokemon.SearchInApiForPokemonByName(pokeType.Pokemon.Name);
                            if (pokemonApi.Id <= 251)
                            {
                                fDB.AddPokemonToDB(pokemonApi);
                            }
                            else
                            {
                                break;
                            };
                        };
                    };
                };
            });

            var pokemonDB = fPokemon.SearchInDBForPokemonByType(BuscarPokemon);
            foreach (Pokemon p in pokemonDB)
            {
                ListPokemon.Add(p);
            };

            searchForTenPages();
            VisibleGo();
            BuscarPokemon = "";
        }
        private async void SearchName()
        {
            BuscarPokemon = BuscarPokemon.ToLower();

            Pokemons.Clear();
            ListPokemon.Clear();
            VisibleGo();

            await Task.Run(() =>
            {
                if (!fPokemon.ThisPokemonExistByName(BuscarPokemon))
                {
                    var pokemonAPI = fPokemon.SearchInApiForPokemonByName(BuscarPokemon);

                    if (pokemonAPI != null && pokemonAPI.Id <= 251)
                    {
                        fDB.AddPokemonToDB(pokemonAPI);
                    }

                }
            });

            var pokemonDB = fPokemon.SearchInDBForPokemonByName(BuscarPokemon);

            foreach (Pokemon p in pokemonDB)
            {
                ListPokemon.Add(p);
            };
            searchForTenPages();
            VisibleGo();
            BuscarPokemon = "";


        }
        public ICommand GetAllPokemonDB { get; }
        private async void GetPokemonDB()
        {
            Pokemons.Clear();
            ListPokemon.Clear();
            VisibleGo();

            ObservableCollection<Pokemon> listP = await Task.Run(() =>
                fPokemon.GetAllPokemonMainPage()
            );
            if (listP.Count != 0)
            {
                foreach (var p in listP)
                {
                    ListPokemon.Add(p);
                }
            }
            searchForTenPages();
            VisibleGo();
        }
        public ICommand NavegationPageDetails
        {
            get;
            private set;
        }
        private async void NavegationPDetails(Pokemon p)
        {
            OtherListPokemons.Clear();
            OtherListPokemons.Add(p);
        }
        public ICommand ButtonPagination { get; }
        private void searchForTenPages()
        {
            ButtonLoadVisible = Visibility.Visible;
            var cont = 1;
            foreach (Pokemon p in ListPokemon)
            {
                if (cont <= ListPokemon.Count && cont <= 10)
                {
                    Pokemons.Add(p);
                    cont++;
                }
                else
                {
                    break;
                }
            }
            cont = 1;
            while (cont <= 10)
            {
                if (ListPokemon.Count != 0)
                {
                    ListPokemon.RemoveAt(0);
                }
                else
                {
                    ButtonLoadVisible = Visibility.Collapsed;
                }

                cont++;
            }
        }
        private async void ValidationMessege(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }
        private void VisibleGo()
        {
            if (IsVisible == Visibility.Visible)
            {
                IsVisible = Visibility.Collapsed;
            }
            else
            {
                IsVisible = Visibility.Visible;
            }
        }
        public MainPageViewModels()
        {
            fDB.InitializeDB();
            fDB.InitializationFirstPokemons();
            GetPokemonDB();
            SearchPokemon = new RelayCommand(SearchByIdTypeName);
            GetAllPokemonDB = new RelayCommand(GetPokemonDB);
            NavegationPageDetails = new RelayCommand<Pokemon>(NavegationPDetails);
            ButtonPagination = new RelayCommand(searchForTenPages);

        }
    }
}