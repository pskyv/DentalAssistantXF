using System;
using System.Collections.Generic;
using System.Text;

namespace DentalAssistantXF.Models
{
    public class GroupedOpenDentalProcedure
    {
        public DentalProcedureType DentalProcedureType { get; set; }

        public int Count { get; set; }
    }
}
