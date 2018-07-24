using System;
using System.IO;
using Xamarin.Forms;

namespace DentalAssistantXF.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => LastName + " " + FirstName;
        
        public string BirthDate { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        //public byte[] ProfilePhoto { get; set; }

        //public ImageSource ProfilePhotoSrc
        //{
        //    get { return ProfilePhoto == null? ImageSource.FromStream(() => new MemoryStream(ProfilePhoto)): null; } 
        //}

        public bool HasOpenCase { get; }
    }
}
