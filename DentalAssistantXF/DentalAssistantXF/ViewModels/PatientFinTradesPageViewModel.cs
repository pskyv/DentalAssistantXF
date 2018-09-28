using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class PatientFinTradesPageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IDatabaseService _databaseService;
        private FinTrade _selectedFinTrade;
        private Patient _currentPatient;
        private decimal _patientBalance;

        public PatientFinTradesPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _databaseService = databaseService;

            MessagingCenter.Subscribe<PatientFinTradesPage>(this, Constants.OnPatientFinTradesPageAppearingMsg, (sender) => { GetPatientFinTradesAsync(); }); 

            FinTrades = new ObservableCollection<FinTrade>();
            AddOrEditFinTradeCommand = new DelegateCommand<string>(AddOrEditFinTradeAsync);
        }        

        public FinTrade SelectedFinTrade
        {
            get { return _selectedFinTrade; }
            set { SetProperty(ref _selectedFinTrade, value); }
        }

        public ObservableCollection<FinTrade> FinTrades { get; }

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public Decimal PatientBalance
        {
            get { return _patientBalance; }
            set { SetProperty(ref _patientBalance, value); }
        }

        public DelegateCommand<string> AddOrEditFinTradeCommand { get; }

        public DelegateCommand<FinTrade> DeleteFinTradeCommand => new DelegateCommand<FinTrade>(async (args) => 
        {
            if (await _pageDialogService.DisplayAlertAsync("Alert", "Are you sure you want to delete this financial trade?", "Yes", "Cancel"))
            {
                if (await _databaseService.DentalAssistantDB.DeleteFinTradeAsync(args) > 0)
                {
                    FinTrades.Remove(args);
                    PatientBalance = FinTrades.Sum(f => f.AmmountForSum);
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Financial trade deleted successfully");
                    MessagingCenter.Send(this, Constants.OnDashboardDataChangeMsg);
                }
            }
        });

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                CurrentPatient = (Patient)parameters["Patient"];
            }
        }

        private async void GetPatientFinTradesAsync()
        {
            var finTrades = await _databaseService.DentalAssistantDB.GetPatientFinTradesAsync(CurrentPatient.Id);

            FinTrades.Clear();
            finTrades.ForEach(FinTrades.Add);

            PatientBalance = FinTrades.Sum(f => f.AmmountForSum);
        }
        

        private async void AddOrEditFinTradeAsync(string mode)
        {
            if (string.Equals(mode, "Add"))
            {
                var finTrade = new FinTrade { TradeDate = DateTime.Today, PatientId = CurrentPatient.Id };
                SelectedFinTrade = finTrade;
            }
            var navParams = new NavigationParameters();
            navParams.Add("PatientFinTrade", SelectedFinTrade);
            await _navigationService.NavigateAsync("EditPatientFinTradePage", navParams);
        }
    }
}
