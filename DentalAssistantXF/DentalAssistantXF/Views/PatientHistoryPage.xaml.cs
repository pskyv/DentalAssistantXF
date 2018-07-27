using DentalAssistantXF.Utils;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class PatientHistoryPage : ContentPage
    {
        public PatientHistoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.OnPatientHistoryPageAppearingMsg);
        }
    }
}
