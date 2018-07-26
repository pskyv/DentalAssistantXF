using DentalAssistantXF.Utils;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class PatientProfilePage : ContentPage
    {
        public PatientProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.OnPatientProfilePageAppearingMsg);
        }
    }
}
