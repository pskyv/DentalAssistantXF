using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using DentalAssistantXF.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class PatientProfilePageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private Patient _currentPatient;
        private int _patientId;

        public PatientProfilePageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            EditPatientCommand = new DelegateCommand(EditPatientAsync);

            MessagingCenter.Subscribe<PatientProfilePage>(this, Constants.OnPatientProfilePageAppearingMsg, (sender) => { GetPatientAsync(); });
        }        

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public DelegateCommand NavigateBackCommand => new DelegateCommand(async () => { await _navigationService.GoBackAsync(); } );

        public DelegateCommand EditPatientCommand { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                _patientId = (int)parameters["PatientId"];                
            }
        }

        private async void GetPatientAsync()
        {
            CurrentPatient = await _databaseService.DentalAssistantDB.GetPatientAsync(_patientId);
        }

        private async void EditPatientAsync()
        {
            var navParams = new NavigationParameters();
            navParams.Add("Patient", CurrentPatient);
            await _navigationService.NavigateAsync("EditPatientPage", navParams);
        }
    }
}
