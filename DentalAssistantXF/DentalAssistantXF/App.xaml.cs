using Prism;
using Prism.Ioc;
using DentalAssistantXF.ViewModels;
using DentalAssistantXF.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using DentalAssistantXF.Services;
using Xamarin.Essentials;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DentalAssistantXF
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
#if DEBUG
            LiveReload.Init();
#endif

            InitializeComponent();

            if (!Preferences.Get("LoginWithPass", false))
            {
                await NavigationService.NavigateAsync("NavigationMenuPage/NavigationPage/MainPage");
            }
            else
            {
                if (Preferences.Get("IsLoggedIn", false) && DateTime.Now < Preferences.Get("ExpiryDate", DateTime.Now.AddDays(-1)))
                {
                    await NavigationService.NavigateAsync("NavigationMenuPage/NavigationPage/MainPage");
                }
                else
                {
                    await NavigationService.NavigateAsync("NavigationMenuPage/NavigationPage/LoginPage");
                }
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<NavigationMenuPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<PatientProfilePage>();
            containerRegistry.RegisterForNavigation<PatientsListPage>();
            containerRegistry.RegisterForNavigation<EditPatientPage>();            
            containerRegistry.RegisterForNavigation<PatientHistoryPage>();
            containerRegistry.RegisterForNavigation<EditPatientHistoryPage>();
            containerRegistry.RegisterForNavigation<PatientFinTradesPage>();
            containerRegistry.RegisterForNavigation<EditPatientFinTradePage>();
            containerRegistry.RegisterForNavigation<DashboardPage>();
            containerRegistry.RegisterForNavigation<AppointmentsListPage>();
            containerRegistry.RegisterForNavigation<EditAppointmentPage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<AboutPage>();

            containerRegistry.RegisterSingleton(typeof(IDatabaseService), typeof(DatabaseService));
            containerRegistry.RegisterSingleton(typeof(IAuthenticationService), typeof(AuthenticationService));
            containerRegistry.RegisterForNavigation<DenturePage>();
        }
    }
}
