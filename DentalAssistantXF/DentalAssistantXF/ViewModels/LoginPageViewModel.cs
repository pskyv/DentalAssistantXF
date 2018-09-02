using DentalAssistantXF.Models;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace DentalAssistantXF.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private string _password;
        private bool _canLogin;
        private bool _isChecking;

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                CanLogin = !string.IsNullOrEmpty(Password);
            }
        }

        public bool CanLogin
        {
            get { return _canLogin; }
            set { SetProperty(ref _canLogin, value); }
        }

        public bool IsChecking
        {
            get { return _isChecking; }
            set { SetProperty(ref _isChecking, value); }
        }

        public DelegateCommand LoginCommand => new DelegateCommand(LoginAsync).ObservesCanExecute(() => CanLogin);

        public DelegateCommand ForgotPasswordCommand => new DelegateCommand(async () =>
        {
            var answer = await _pageDialogService.DisplayAlertAsync("Confirm", $"Send password to: {Preferences.Get("Email", "")}?", "Ok", "Cancel");
            if(answer)
            {
                SendEmailWithPassword();
            }
        });
    

        private async void LoginAsync()
        {
            IsChecking = true;
            //System.Threading.Thread.Sleep(5000);

            if (string.Equals(Password, Preferences.Get("Password", "")))
            {
                await _navigationService.NavigateAsync("NavigationMenuPage/NavigationPage/MainPage");
            }
            else
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, "Invalid password");
            }

            IsChecking = false;
        }

        private async void SendEmailWithPassword()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Dentist Assist password reminder",
                    Body = $"Password: {Preferences.Get("Password", "")}",
                    To = new List<string> { Preferences.Get("Email", "") }
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
                HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Password sent successfully");
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, "Email is not supported on this device");
            }
            catch (Exception ex)
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, "Failed sending email");
            }            
        }

    }
}
