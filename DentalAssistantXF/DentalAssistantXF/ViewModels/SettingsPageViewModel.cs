using DentalAssistantXF.Models;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace DentalAssistantXF.ViewModels
{
    public class SettingsPageViewModel : BindableBase
    {
        private bool _loginWithPass;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _canSave;
        private bool _isEmailValid;

        public SettingsPageViewModel()
        {
            Initialization();
        }

        public bool LoginWithPass
        {
            get { return _loginWithPass; }
            set
            {
                SetProperty(ref _loginWithPass, value);
                if (!LoginWithPass)
                {
                    ClearSettings();
                }
            }
        }

        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set { SetProperty(ref _isEmailValid, value); }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
                IsEmailValid = ValidateEmail(Email);
                CanSave = IsEmailValid && string.Equals(Password, ConfirmPassword) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                CanSave = IsEmailValid && string.Equals(Password, ConfirmPassword) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                SetProperty(ref _confirmPassword, value);
                CanSave = IsEmailValid && string.Equals(Password, ConfirmPassword) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword);
            }
        }

        public DelegateCommand SaveSettingsCommand => new DelegateCommand(SaveSettings).ObservesCanExecute(() => CanSave);

        private void SaveSettings()
        {
            Preferences.Set("LoginWithPass", LoginWithPass);
            Preferences.Set("Email", Email);
            Preferences.Set("Password", Password);
            HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Settings saved successfully");
            //try
            //{
            //    await SecureStorage.SetAsync("password", Password);
            //}
            //catch (Exception ex)
            //{
            //    // Possible that device doesn't support secure storage on device.
            //}
        }

        private void Initialization()
        {
            LoginWithPass = Preferences.Get("LoginWithPass", false);
            if (LoginWithPass)
            {
                Email = Preferences.Get("Email", "");
                Password = Preferences.Get("Password", "");
                ConfirmPassword = Preferences.Get("Password", "");
                //try
                //{
                //    Password = await SecureStorage.GetAsync("Password");
                //    ConfirmPassword = await SecureStorage.GetAsync("Password");
                //}
                //catch (Exception ex)
                //{

                //}
            }
        }

        private void ClearSettings()
        {
            Preferences.Clear();
            //SecureStorage.Remove("Password");
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private bool ValidateEmail(string emailAddress)
        {
            string regexPattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            Match matches = Regex.Match(emailAddress, regexPattern);
            return matches.Success;
        }
    }
}
