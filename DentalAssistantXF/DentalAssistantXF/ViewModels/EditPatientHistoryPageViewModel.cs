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
            get { return Enum.GetNames(typeof(DentalProcedureType)).ToList(); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand SavePatientDentalProcedureCommand { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if(parameters != null)
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
                    MessagingCenter.Send(this, Constants.OnDasboardDataChangeMsg);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
