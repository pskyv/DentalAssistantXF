﻿using DentalAssistantXF.Models;
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
        private PatientDentalProcedure _selectedPatientDentalProcedure;

        public PatientHistoryPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            MessagingCenter.Subscribe<PatientHistoryPage>(this, Constants.OnPatientHistoryPageAppearingMsg, (sender) => { GetPatienDentalOperationsAsync(); }); //{ GetPatienDentalOperationsAsync(); });

            PatientDentalProcedures = new ObservableCollection<PatientDentalProcedure>();
            AddOrEditPatientDentalProcedureCommand = new DelegateCommand<string>(AddOrEditPatientDentalProcedureAsync);
        }        

        public Patient CurrentPatient
        {
            get { return _currentPatient; }
            set { SetProperty(ref _currentPatient, value); }
        }

        public PatientDentalProcedure SelectedPatientDentalProcedure
        {
            get { return _selectedPatientDentalProcedure; }
            set { SetProperty(ref _selectedPatientDentalProcedure, value); }
        }

        public ObservableCollection<PatientDentalProcedure> PatientDentalProcedures { get; }

        public DelegateCommand<string> AddOrEditPatientDentalProcedureCommand { get; }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if(parameters != null)
            {
                CurrentPatient = (Patient)parameters["Patient"];
            }
        }

        private async void GetPatienDentalOperationsAsync()
        {
            var procedures = await _databaseService.DentalAssistantDB.GetPatientDentalProceduresAsync(CurrentPatient.Id);
            if (procedures.Count > 0)
            {
                procedures.Last().IsLast = true;
            }

            PatientDentalProcedures.Clear();
            
            foreach(var procedure in procedures)
            {
                PatientDentalProcedures.Add(procedure);
            }
        }

        private async void AddOrEditPatientDentalProcedureAsync(string mode = "Edit")
        {
            if (string.Equals(mode, "Add"))
            {
                var patientDentalProcedure = new PatientDentalProcedure { StartDate = DateTime.Today, PatientId = CurrentPatient.Id };
                SelectedPatientDentalProcedure = patientDentalProcedure;
            }
            var navParams = new NavigationParameters();
            navParams.Add("PatientDentalProcedure", SelectedPatientDentalProcedure);
            await _navigationService.NavigateAsync("EditPatientHistoryPage", navParams);
        }

        private void GetMockProcedures()
        {
            PatientDentalProcedures.Clear();
            PatientDentalProcedures.Add(new PatientDentalProcedure { PatientId = 1, DentalProcedure = DentalProcedureType.Cleaning, Description = "Normal cleaning", StartDate = DateTime.Today });
            PatientDentalProcedures.Add(new PatientDentalProcedure { PatientId = 1, DentalProcedure = DentalProcedureType.Cap, Description = "A cap for tooth with index 8. Difficult case", StartDate = DateTime.Today });
            PatientDentalProcedures.Add(new PatientDentalProcedure { PatientId = 1, DentalProcedure = DentalProcedureType.Cleaning, Description = "Normal cleaning", StartDate = DateTime.Today });
            PatientDentalProcedures.Add(new PatientDentalProcedure { PatientId = 1, DentalProcedure = DentalProcedureType.Filling, Description = "Normal cleaning", StartDate = DateTime.Today, IsLast = true });
        }
    }
}
