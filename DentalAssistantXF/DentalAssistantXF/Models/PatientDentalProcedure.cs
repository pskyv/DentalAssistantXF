using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.Models
{
    public class PatientDentalProcedure
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DentalProcedureType DentalProcedure { get; set; }

        public string TeethNumbers { get; set; }

        public string Description { get; set; }

        //public DentalProcedureStatus Status { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime StartDate { get; set; }

        public string Notes { get; set; }

        [Ignore]
        public bool IsLast { get; set; }

        [Ignore]
        public List<int> TeethList
        {
            get
            {
                if(string.IsNullOrEmpty(TeethNumbers))
                {
                    return new List<int>();
                }

                return TeethNumbers.Split(',').Select(int.Parse).ToList();
            }
        }
    }
}
