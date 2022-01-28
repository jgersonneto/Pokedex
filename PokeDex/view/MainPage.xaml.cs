using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace PokeDex.view
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void ButaoCadastro_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ViewCadastro), null, new DrillInNavigationTransitionInfo());
        }
        private void ButtonPageNavigation(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PokemonDetails), viewModels.OtherListPokemons, new DrillInNavigationTransitionInfo());
        }
    }
}
