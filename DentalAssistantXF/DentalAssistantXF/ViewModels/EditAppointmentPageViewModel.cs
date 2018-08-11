using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
    public class EditAppointmentPageViewModel : BindableBase, INavigatingAware
    {
        private readonly IDatabaseService _databaseService;
        private Appointment _appointment;
        private string _title;

        public EditAppointmentPageViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;

            SaveAppointmentCommand = new DelegateCommand(SaveAppointment);
        }        

        public Appointment Appointment
        {
            get { return _appointment; }
            set { SetProperty(ref _appointment, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                Appointment = (Appointment)parameters["Appointment"];
                Title = Appointment.Id < 1 ? "Add new appointment" : "Edit appointment";
            }
        }

        public DelegateCommand SaveAppointmentCommand { get; }

        private async void SaveAppointment()
        {
            try
            {
                if (await _databaseService.DentalAssistantDB.SaveAppointmentAsync(Appointment) > 0)
                {
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Appointment saved successfully");
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
