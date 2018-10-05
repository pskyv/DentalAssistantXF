using DentalAssistantXF.Models;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace DentalAssistantXF.ViewModels
{
	public class NavigationMenuPageViewModel : BindableBase
	{
        private readonly INavigationService _navigationService;
        private NavigationMenuItem _selectedNavItem;

        public NavigationMenuPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            InitNavItems();
        }

        public NavigationMenuItem SelectedNaviItem
        {
            get { return _selectedNavItem; }
            set { SetProperty(ref _selectedNavItem, value); }
        }

        public bool IsLoggedIn
        {
            get { return Preferences.Get("LoginWithPass", false); }
        }

        public ObservableCollection<NavigationMenuItem> NavItems { get; set; }

        public DelegateCommand NavigateCommand => new DelegateCommand(NavigateAsync);

        public DelegateCommand LogoutCommand => new DelegateCommand(LogoutAsync);        

        private async void NavigateAsync()
        {
            await _navigationService.NavigateAsync("NavigationPage/" + SelectedNaviItem.Page);
        }

        private async void LogoutAsync()
        {
            Preferences.Set("IsLoggedIn", false);
            Preferences.Remove("ExpiryDate");
            await _navigationService.NavigateAsync("myApp:///NavigationMenuPage/NavigationPage/LoginPage");
        }

        private void InitNavItems()
        {
            NavItems = new ObservableCollection<NavigationMenuItem>()
            {
                new NavigationMenuItem {Caption = Constants.MainPageCaption, Page = "MainPage", IconSource = "ic_home"},
                new NavigationMenuItem {Caption = Constants.SettingsPageCaption, Page = "SettingsPage", IconSource = "ic_settings"},
                new NavigationMenuItem {Caption = Constants.AboutPageCaption, Page = "AboutPage", IconSource = "ic_information_black_24dp"},
            };
        }
    }
}
