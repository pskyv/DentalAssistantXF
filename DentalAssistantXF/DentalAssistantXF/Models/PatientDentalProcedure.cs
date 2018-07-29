using SQLite;
using System;

namespace DentalAssistantXF.Models
{
    public class PatientDentalProcedure
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DentalProcedureType DentalProcedure { get; set; }

        public string Description { get; set; }

        public DentalProcedureStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public string Notes { get; set; }

        [Ignore]
        public bool IsLast { get; set; }
    }
}
