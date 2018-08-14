using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

namespace DentalAssistantXF.Models
{
    public class Patient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Ignore]
        public string FullName => LastName + " " + FirstName;

        public string Occupation { get; set; }
        
        public DateTime BirthDate { get; set; }

        [Ignore]
        public int Age => DateTime.Today.Year - BirthDate.Year;

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        public byte[] ProfilePhoto { get; set; }

        [Ignore]
        public ImageSource ProfilePhotoSrc
        {
            get { return ProfilePhoto != null ? ImageSource.FromStream(() => new MemoryStream(ProfilePhoto)) : ImageSource.FromFile("avatar"); }
        }

        [Ignore]
        public bool HasOpenCase { get; set; }
    }
}
