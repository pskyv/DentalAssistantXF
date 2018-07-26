using DentalAssistantXF.Models;
using DentalAssistantXF.Services;
using DentalAssistantXF.Utils;
using Plugin.Media;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace DentalAssistantXF.ViewModels
{
	public class EditPatientPageViewModel : BindableBase, INavigatingAware
	{
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _databaseService;
        private Patient _patient;
        private string _title;
        private ImageSource _profilePhotoSrc;

        public EditPatientPageViewModel(INavigationService navigationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _databaseService = databaseService;

            SavePatientCommand = new DelegateCommand(SavePatientAsync);
            TakePhotoCommand = new DelegateCommand(TakePhotoAsync);
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

        public ImageSource ProfilePhotoSrc
        {
            get { return _profilePhotoSrc; }
            set { SetProperty(ref _profilePhotoSrc, value); }
        }

        public DelegateCommand SavePatientCommand { get; }

        public DelegateCommand TakePhotoCommand { get; }

        private async void SavePatientAsync()
        {
            try
            {
                if (Patient.Id > 0)
                {
                    if (await _databaseService.DentalAssistantDB.UpdatePatientAsync(Patient) > 0)
                    {
                        HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Patient saved successfully");
                    }
                }
                else
                {
                    if (await _databaseService.DentalAssistantDB.SavePatientAsync(Patient) > 0)
                    {
                        HelperFunctions.ShowToastMessage(ToastMessageType.Success, "Patient saved successfully");
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private async void TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }

            var mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 92
            });

            if (mediaFile == null)
                return;

            ProfilePhotoSrc = ImageSource.FromStream(() => mediaFile.GetStream());
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                Patient = (Patient)parameters["Patient"];
                Title = Patient.Id < 1 ? "Add patient" : "Edit patient";
            }
        }
    }
}
