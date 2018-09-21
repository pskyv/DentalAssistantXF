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
    public class EditPatientHistoryPageViewModel : BindableBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private PatientDentalProcedure _patientDentalProcedure;
        private DentalProcedureType _selectedDentalProcedureType;
        private string _title;

        public EditPatientHistoryPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            SavePatientDentalProcedureCommand = new DelegateCommand(SavePatientDentalProcedureAsync);
            CreateFinTradeCommand = new DelegateCommand(CreateFinTradeAsync);
        }

        public PatientDentalProcedure PatientDentalProcedure
        {
            get { return _patientDentalProcedure; }
            set { SetProperty(ref _patientDentalProcedure, value); }
        }

        public DentalProcedureType SelectedDentalProcedureType
        {
            get { return _selectedDentalProcedureType; }
            set { SetProperty(ref _selectedDentalProcedureType, value); }
        }

        public List<string> DentalProcedureTypes
        {
            get { return Enum.GetNames(typeof(DentalProcedureType)).Select(d => d.SplitCamelCase()).ToList(); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand SavePatientDentalProcedureCommand { get; }

        public DelegateCommand CreateFinTradeCommand { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                PatientDentalProcedure = (PatientDentalProcedure)parameters["PatientDentalProcedure"];
                Title = PatientDentalProcedure.Id < 1 ? "Add procedure" : "Edit procedure";
                SelectedDentalProcedureType = PatientDentalProcedure.DentalProcedure;
            }
        }

        private async void SavePatientDentalProcedureAsync()
        {
            try
            {
                PatientDentalProcedure.DentalProcedure = SelectedDentalProcedureType;
                if (await _databaseService.DentalAssistantDB.SavePatientDentalprocedureAsync(PatientDentalProcedure) > 0)
                {
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Dental procedure saved successfully");
                    MessagingCenter.Send(this, Constants.OnDashboardDataChangeMsg);
                    MessagingCenter.Send(this, Constants.OnAddOrEditPatientMsg);
                }
            }
            catch (Exception e)
            {

            }
        }

        private async void CreateFinTradeAsync()
        {
            var patient = await _databaseService.DentalAssistantDB.GetPatientAsync(PatientDentalProcedure.PatientId);
            var finTrade = new FinTrade { TradeDate = DateTime.Today, PatientId = patient.Id };

            var navParams = new NavigationParameters();
            navParams.Add("PatientFinTrade", finTrade);
            await _navigationService.NavigateAsync("EditPatientFinTradePage", navParams);
        }
    }
}
