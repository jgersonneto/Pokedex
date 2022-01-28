
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace PokeDex.view
{

    public sealed partial class ViewCadastro : Page
    {
        public ViewCadastro()
        {
            this.InitializeComponent();
        }
        private void ButaoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }

        

        
    }
}
