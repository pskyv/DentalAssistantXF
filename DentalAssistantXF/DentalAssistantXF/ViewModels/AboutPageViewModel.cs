using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace DentalAssistantXF.ViewModels
{
	public class AboutPageViewModel : BindableBase
	{
        public AboutPageViewModel()
        {

        }

        public string Version { get { return AppInfo.VersionString; } }

        public DateTime AccessTokenExpires { get { return Preferences.Get("ExpiryDate", DateTime.Now); } }
    }
}
