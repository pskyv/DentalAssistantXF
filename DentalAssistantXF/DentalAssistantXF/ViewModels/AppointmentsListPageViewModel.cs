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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
    public class AppointmentsListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private readonly IPageDialogService _dialogService;
        private DateTime _filterDate;
        private AppointmentDTO _selectedAppointment;

        public AppointmentsListPageViewModel(INavigationService navigationService, IDatabaseService databaseService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;
            _dialogService = dialogService;

            Appointments = new ObservableCollection<AppointmentDTO>();
            MessagingCenter.Subscribe<AppointmentsListPage>(this, Constants.OnAppointmentsListPageAppearingMsg, (sender) => { FilterDate = DateTime.Today; });

            AddAppointmentCommand = new DelegateCommand(AddAppointment);
            ShowActionsCommand = new DelegateCommand<AppointmentDTO>(ShowActions);
        }        

        public DateTime FilterDate
        {
            get { return _filterDate; }
            set
            {
                SetProperty(ref _filterDate, value);
                GetAppointmentsAsync();
            }
        }

        public AppointmentDTO SelectedAppointment
        {
            get { return _selectedAppointment; }
            set { SetProperty(ref _selectedAppointment, value); }
        }

        public ObservableCollection<AppointmentDTO> Appointments { get; }

        public DelegateCommand AddAppointmentCommand { get; }

        public DelegateCommand<AppointmentDTO> ShowActionsCommand { get; }

        private async void GetAppointmentsAsync()
        {
            var appointments = await _databaseService.DentalAssistantDB.GetAppointmentsListAsync(FilterDate);

            Appointments.Clear();
            appointments.ForEach(Appointments.Add);
        }

        private async void AddAppointment()
        {
            var appointment = new Appointment { AppointmentDate = DateTime.Today, AppointmentTime = DateTime.Now.TimeOfDay }; // PatientId = 1    to be deleted......!!!!!!

            var navParams = new NavigationParameters();
            navParams.Add("Appointment", appointment);
            await _navigationService.NavigateAsync("EditAppointmentPage", navParams);
        }        

        private async void ShowActions(AppointmentDTO appointmentDTO)
        {
            IActionSheetButton editAppointmentBtn = ActionSheetButton.CreateButton("Edit appointment", new DelegateCommand(() => { EditAppointment(appointmentDTO); }));
            IActionSheetButton deleteAppointmentBtn = ActionSheetButton.CreateButton("Delete appointment", new DelegateCommand(() => { DeleteAppointment(appointmentDTO.Id); }));
            IActionSheetButton callPatientBtn = ActionSheetButton.CreateButton("Call patient", new DelegateCommand(() => { CallPatient(appointmentDTO.Phone); }));
            await _dialogService.DisplayActionSheetAsync("Appointment actions", editAppointmentBtn, callPatientBtn, deleteAppointmentBtn);
        }        

        private async void EditAppointment(AppointmentDTO appointmentDTO)
        {
            var appointment = await _databaseService.DentalAssistantDB.GetAppointmentAsync(appointmentDTO.Id);
            var patient = await _databaseService.DentalAssistantDB.GetPatientAsync(appointmentDTO.PatientId);

            var navParams = new NavigationParameters();
            navParams.Add("Appointment", appointment);
            navParams.Add("Patient", patient);
            await _navigationService.NavigateAsync("EditAppointmentPage", navParams);
        }

        private void CallPatient(string phone)
        {
            try
            {
                PhoneDialer.Open(phone);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
        }

        private async void DeleteAppointment(int appointmentId)
        {
            try
            {
                if (await _databaseService.DentalAssistantDB.DeleteAppointmentAsync(appointmentId) > 0)
                {
                    GetAppointmentsAsync();
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Appointment deleted successfully");
                }
            }
            catch
            {

            }
        }
    }
}
