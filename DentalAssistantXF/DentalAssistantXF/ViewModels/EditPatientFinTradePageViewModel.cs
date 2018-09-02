using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class EditPatientFinTradePageViewModel : BindableBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private FinTrade _patientFinTrade;
        private FinTradeType _selectedFinTradeType;
        private string _title;

        public EditPatientFinTradePageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            SavePatientFinTradeCommand = new DelegateCommand(SavePatientFinTradeAsync);
        }        

        public FinTrade PatientFinTrade
        {
            get { return _patientFinTrade; }
            set { SetProperty(ref _patientFinTrade, value); }
        }

        public FinTradeType SelectedFinTradeType
        {
            get { return _selectedFinTradeType; }
            set { SetProperty(ref _selectedFinTradeType, value); }
        }

        public List<string> FinTradeTypes
        {
            get { return Enum.GetNames(typeof(FinTradeType)).ToList(); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand SavePatientFinTradeCommand { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                PatientFinTrade = (FinTrade)parameters["PatientFinTrade"];
                Title = PatientFinTrade.Id < 1 ? "Add financial trade" : "Edit financial trade";
                SelectedFinTradeType = PatientFinTrade.TradeType;
            }
        }

        private async void SavePatientFinTradeAsync()
        {
            try
            {
                PatientFinTrade.TradeType = SelectedFinTradeType;
                if (await _databaseService.DentalAssistantDB.SavePatientFinTradeAsync(PatientFinTrade) > 0)
                {
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Financial trade saved successfully");
                    MessagingCenter.Send(this, Constants.OnDashboardDataChangeMsg);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
