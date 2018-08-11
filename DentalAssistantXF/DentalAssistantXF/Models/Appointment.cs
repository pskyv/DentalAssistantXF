using SQLite;
using System;

namespace DentalAssistantXF.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public string Subject { get; set; }
    }
}
