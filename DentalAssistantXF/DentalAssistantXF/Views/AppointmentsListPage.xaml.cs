using DentalAssistantXF.Utils;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
    public partial class AppointmentsListPage : ContentPage
    {
        public AppointmentsListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, Constants.OnAppointmentsListPageAppearingMsg);
        }
    }
}
