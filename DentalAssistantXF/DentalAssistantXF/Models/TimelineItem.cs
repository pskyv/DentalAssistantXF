using System;
using System.Collections.Generic;
using System.Text;

namespace DentalAssistantXF.Models
{
    public class TimelineItem
    {
        public DateTime TaskDate { get; set; }

        public string NurseName { get; set; }

        public string Icon { get; set; }

        public string TaskDescription { get; set; }
    }
}
