using System;
using System.Collections.Generic;
using System.Text;

namespace DentalAssistantXF.Models
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => LastName + " " + FirstName;

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public DateTime AppointmentDateAndTime => AppointmentDate.Date.Add(AppointmentTime);

        public string Subject { get; set; }

        public string Color
        {
            get { return GetColor(); }
        }

        private string GetColor()
        {
            TimeSpan MorningTime = new TimeSpan(8, 0, 0);
            TimeSpan NoonTime = new TimeSpan(12, 0, 0);
            TimeSpan AfternoonTime = new TimeSpan(17, 0, 0);
            TimeSpan EveningTime = new TimeSpan(21, 0, 0);

            if (AppointmentTime >= MorningTime && AppointmentTime < NoonTime)
            {
                return "yellow";
            }
            else if (AppointmentTime > NoonTime && AppointmentTime < AfternoonTime)
            {
                return "red";
            }
            else
            {
                return "green";
            }
        }
    }
}
