using Prism;
using Prism.Ioc;
using DentalAssistantXF.ViewModels;
using DentalAssistantXF.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using DentalAssistantXF.Services;

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
            //#if DEBUG
            //LiveReload.Init();
            //#endif

            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationMenuPage/NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<NavigationMenuPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<PatientProfilePage>();
            containerRegistry.RegisterForNavigation<PatientsListPage>();
            containerRegistry.RegisterForNavigation<EditPatientPage>();

            containerRegistry.RegisterSingleton(typeof(IDatabaseService), typeof(DatabaseService));
            containerRegistry.RegisterForNavigation<PatientHistoryPage>();
            containerRegistry.RegisterForNavigation<EditPatientHistoryPage>();
            containerRegistry.RegisterForNavigation<PatientFinTradesPage>();
            containerRegistry.RegisterForNavigation<EditPatientFinTradePage>();
        }
    }
}
