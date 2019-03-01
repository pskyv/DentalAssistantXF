using DentalAssistantXF.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalAssistantXF.ViewModels
{
	public class TimelineTestPageViewModel : BindableBase
	{
        public TimelineTestPageViewModel()
        {

        }

        public List<TimelineItem> Items { get; set; } = new List<TimelineItem> {
            new TimelineItem{ TaskDate = DateTime.Now, NurseName = "Papadopoulou Maria", Icon = "ic_openCase", TaskDescription = "Task1 and task2, task3 task4, task5(for task6) every 2 hours, the end" },
            new TimelineItem{ TaskDate = DateTime.Now.AddHours(1), NurseName = "Dimitriou Eleni", Icon = "ic_openCase", TaskDescription = "Task1 and task2, task3 task4, task5(for task6) every 2 hours, the end" },
            new TimelineItem{ TaskDate = DateTime.Now.AddHours(-3), NurseName = "Terzi Vasiliki", Icon = "ic_openCase", TaskDescription = "Task1 and task2, task3 task4, task5(for task6) every 2 hours, the end" },
        };
	}
}
