using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
	public class PatientProfilePageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private Patient _currentPatient;

        public PatientProfilePageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            EditPatientCommand = new DelegateCommand(EditPatientAsync);
        }        

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public DelegateCommand NavigateBackCommand => new DelegateCommand(async () => { await _navigationService.GoBackAsync(); } );

        public DelegateCommand EditPatientCommand { get; }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                var patientId = (int)parameters["PatientId"];
                CurrentPatient = await _databaseService.DentalAssistantDB.GetPatientAsync(patientId);
            }
        }

        private async void EditPatientAsync()
        {
            var navParams = new NavigationParameters();
            navParams.Add("Patient", CurrentPatient);
            await _navigationService.NavigateAsync("EditPatientPage", navParams);
        }
    }
}
