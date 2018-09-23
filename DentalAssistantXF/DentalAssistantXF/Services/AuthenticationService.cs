using DentalAssistantXF.Utils;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DentalAssistantXF.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationService _navigationService;        

        public AuthenticationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ConfigureAuthenticator();
        }

        private void ConfigureAuthenticator()
        {
            Authenticator = new OAuth2Authenticator(
                Constants.clientId,
                null,
                Constants.scope,
                new Uri(Constants.authorizeUrl),
                new Uri(Constants.redirectUri),
                new Uri(Constants.accesstokenUrl),
                null,
                true
                );

            Authenticator.Completed += OnAuthenticatorCompleted;
            Authenticator.Error += OnAuthenticatorError;

            AuthenticationState.Authenticator = Authenticator;
        }

        private async void OnAuthenticatorCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;

            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthenticatorCompleted;
                authenticator.Error -= OnAuthenticatorError;
            }

            if (e.IsAuthenticated)
            {
                await AccountStore.Create().SaveAsync(e.Account, Constants.AccessTokenKey); //Account properties: access_token, expires_in, refresh_token, token_type
                Preferences.Set("IsLoggedIn", true);
                double expiresIn = 0;
                Double.TryParse(e.Account.Properties["expires_in"], out expiresIn);
                Preferences.Set("ExpiryDate", DateTime.Now.AddSeconds(expiresIn));

                await NavigateToPageAsync();
                //DependencyService.Get<IActivityService>().StartActivity();
            }
        }

        private async Task NavigateToPageAsync()
        {
            await _navigationService.NavigateAsync("NavigationMenuPage/NavigationPage/MainPage");
        }

        private void OnAuthenticatorError(object sender, AuthenticatorErrorEventArgs e)
        {

        }

        public static OAuth2Authenticator Authenticator;

        public void AuthenticateUser()
        {
            try
            {
                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(Authenticator);
            }
            catch (Exception e)
            {

            }
        }
    }
}
