using Prism.Navigation;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class LoginPage : ContentPage, INavigatedAware
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            Navigation.RemovePage(this);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }
    }
}
