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
        private int _cases;
        private decimal _balance;

        public PatientProfilePageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            EditPatientCommand = new DelegateCommand<string>(NavigateToPage);
            NavigateToPatientDetailsCommand = new DelegateCommand<string>(NavigateToPage);

            MessagingCenter.Subscribe<PatientProfilePage>(this, Constants.OnPatientProfilePageAppearingMsg, (sender) => { GetPatientAsync(); });
        }        

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public int Cases
        {
            get { return _cases; }
            set { SetProperty(ref _cases, value); }
        }

        public decimal Balance
        {
            get { return _balance; }
            set { SetProperty(ref _balance, value); }
        }

        public DelegateCommand NavigateBackCommand => new DelegateCommand(async () => { await _navigationService.GoBackAsync(); } );

        public DelegateCommand<string> EditPatientCommand { get; }

        public DelegateCommand<string> NavigateToPatientDetailsCommand { get; }

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
            Cases = await _databaseService.DentalAssistantDB.GetPatientDentalProceduresCountAsync(_patientId);
            Balance = await _databaseService.DentalAssistantDB.GetPatientBalanceAsync(_patientId);
        }

        private async void NavigateToPage(string page)
        {
            var navParams = new NavigationParameters();
            navParams.Add("Patient", CurrentPatient);
            await _navigationService.NavigateAsync(page, navParams);
        }
    }
}
