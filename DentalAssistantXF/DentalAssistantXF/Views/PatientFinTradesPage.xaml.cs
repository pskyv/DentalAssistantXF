using DentalAssistantXF.Utils;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class PatientFinTradesPage : ContentPage
    {
        public PatientFinTradesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.OnPatientFinTradesPageAppearingMsg);
        }
    }
}
