using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DentalAssistantXF.Views
{
	public partial class MainPage : Xamarin.Forms.TabbedPage
    {
		public MainPage ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}