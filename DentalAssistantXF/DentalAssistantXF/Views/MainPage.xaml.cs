using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace DentalAssistantXF.Views
{
	public partial class MainPage : Xamarin.Forms.TabbedPage
    {
		public MainPage ()
		{
			InitializeComponent ();

            //if (DeviceInfo.Platform == "Android")
            //{
            //    if (DeviceInfo.Version.Major >= 7 && DeviceInfo.Version.Minor >= 1)
            //    {
            //        On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);                    
            //    }
            //}

        }
    }
}