using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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
        private readonly IDatabaseService _databaseService;
        private FinTrade _selectedFinTrade;
        private Patient _currentPatient;
        private decimal _patientBalance;

        public PatientFinTradesPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
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
            foreach(var finTrade in finTrades)
            {
                FinTrades.Add(finTrade);
            }

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
