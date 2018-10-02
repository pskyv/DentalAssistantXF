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
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class PatientProfilePageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IDatabaseService _databaseService;
        private Patient _currentPatient;
        private int _patientId;
        private int _cases;
        private decimal _balance;

        public PatientProfilePageViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _databaseService = databaseService;

            ShowPatientActionsCommand = new DelegateCommand(ShowPatientActionsAsync);
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

        public DelegateCommand ShowPatientActionsCommand { get; }

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

        private async void ShowPatientActionsAsync()
        {
            IActionSheetButton editPatientBtn = ActionSheetButton.CreateButton("Edit patient", new DelegateCommand(() => { NavigateToPage("EditPatientPage"); }));
            IActionSheetButton callPatientBtn = ActionSheetButton.CreateButton("Call patient", new DelegateCommand(() => { CallPatient(); }));
            IActionSheetButton scheduleAppointmentBtn = ActionSheetButton.CreateButton("Schedule appointment", new DelegateCommand(() => { ScheduleAppointmentAsync(); }));            

            await _dialogService.DisplayActionSheetAsync("Patient actions", editPatientBtn, callPatientBtn, scheduleAppointmentBtn);
        }        

        private async void NavigateToPage(string page)
        {
            var navParams = new NavigationParameters();
            navParams.Add("Patient", CurrentPatient);
            await _navigationService.NavigateAsync(page, navParams);
        }

        private void CallPatient()
        {
            if(string.IsNullOrEmpty(CurrentPatient.Phone))
            {
                HelperFunctions.ShowToastMessage(ToastMessageType.Error, "There's no registered phone number");
                return;
            }

            try
            {
                PhoneDialer.Open(CurrentPatient.Phone);
            }

            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
        }

        private async void ScheduleAppointmentAsync()
        {
            var appointment = new Appointment { AppointmentDate = DateTime.Today, AppointmentTime = DateTime.Now.TimeOfDay, PatientId = CurrentPatient.Id };

            var navParams = new NavigationParameters();
            navParams.Add("Appointment", appointment);
            navParams.Add("Patient", CurrentPatient);
            await _navigationService.NavigateAsync("EditAppointmentPage", navParams);
        }
    }
}
