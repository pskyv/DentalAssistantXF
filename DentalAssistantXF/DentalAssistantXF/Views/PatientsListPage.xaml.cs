using DentalAssistantXF.Utils;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class PatientsListPage : ContentPage
    {
        public PatientsListPage()
        {
            InitializeComponent();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.OnPatientsListPageAppearingMsg);
        }
    }
}
