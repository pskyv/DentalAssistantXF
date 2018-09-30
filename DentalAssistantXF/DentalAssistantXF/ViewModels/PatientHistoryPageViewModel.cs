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
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class PatientHistoryPageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IDatabaseService _databaseService;
        private Patient _currentPatient;
        private string _title;
        private PatientDentalProcedure _selectedPatientDentalProcedure;

        public PatientHistoryPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
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

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public PatientDentalProcedure SelectedPatientDentalProcedure
        {
            get { return _selectedPatientDentalProcedure; }
            set { SetProperty(ref _selectedPatientDentalProcedure, value); }
        }

        public ObservableCollection<PatientDentalProcedure> PatientDentalProcedures { get; }

        public DelegateCommand<string> AddOrEditPatientDentalProcedureCommand { get; }

        public DelegateCommand<PatientDentalProcedure> DeleteProcedureCommand => new DelegateCommand<PatientDentalProcedure>(async (args) =>
        {
            if (await _pageDialogService.DisplayAlertAsync("Alert", "Are you sure you want to delete this procedure?", "Yes", "Cancel"))
            {
                if (await _databaseService.DentalAssistantDB.DeleteProcedureAsync(args) > 0)
                {
                    GetPatienDentalOperationsAsync();
                    HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Dental procedure deleted successfully");
                    MessagingCenter.Send(this, Constants.OnDashboardDataChangeMsg);
                    MessagingCenter.Send(this, Constants.OnAddOrEditPatientMsg);
                }
            }
        });

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if(parameters != null)
            {
                CurrentPatient = (Patient)parameters["Patient"];
                Title = CurrentPatient.FullName + " timeline";
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
            procedures.ForEach(PatientDentalProcedures.Add);
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
