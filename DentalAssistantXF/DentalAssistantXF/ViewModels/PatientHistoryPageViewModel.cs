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
	public class PatientHistoryPageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private Patient _currentPatient;

        public PatientHistoryPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            MessagingCenter.Subscribe<PatientHistoryPage>(this, Constants.OnPatientHistoryPageAppearingMsg, (sender) => { GetPatienDentalOperationsAsync(); });

            PatientDentalProcedures = new ObservableCollection<PatientDentalProcedure>();
        }        

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public ObservableCollection<PatientDentalProcedure> PatientDentalProcedures { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if(parameters != null)
            {
                CurrentPatient = (Patient)parameters["patient"];
            }
        }

        private async void GetPatienDentalOperationsAsync()
        {
            var procedures = await _databaseService.DentalAssistantDB.GetPatientDentalProcedures(CurrentPatient.Id);

            PatientDentalProcedures.Clear();
            foreach(var procedure in procedures)
            {
                PatientDentalProcedures.Add(procedure);
            }
        }
    }
}
