using DentalAssistantXF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
	public class EditPatientPageViewModel : BindableBase, INavigationAware
	{
        private readonly INavigationService _navigationService;
        private Patient _patient;
        private string _title;

        public EditPatientPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SavePatientCommand = new DelegateCommand(SavePatient);
            TakePhotoCommand = new DelegateCommand(TakePhoto);
        }        

        public Patient Patient
        {
            get { return _patient; }
            set { SetProperty(ref _patient, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand SavePatientCommand { get; }

        public DelegateCommand TakePhotoCommand { get; }

        private void SavePatient()
        {
            
        }

        private void TakePhoto()
        {
           
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                Patient = (Patient)parameters["Patient"];
                Title = Patient.PatientId < 1 ? "Add patient" : "Edit patient";
            }
        }
    }
}
