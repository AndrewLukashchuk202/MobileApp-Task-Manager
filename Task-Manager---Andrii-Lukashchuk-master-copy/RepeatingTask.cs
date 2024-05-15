using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task_Manager___Andrii_Lukashchuk.MainPage;

namespace Task_Manager___Andrii_Lukashchuk
{

    // Enum for repetition schedule
    public enum RepetitionSchedule
    {
        Daily,
        Weekly,
        Monthly
    }

    /// <summary>
    /// Represents a repeating task in the Task Manager.
    /// </summary>
    public class RepeatingTask : Task
    {
        // Repetition schedule
        public RepetitionSchedule repetitionSchedule;

        // Constructors
        public RepeatingTask(RepetitionSchedule repetitionSchedule, string taskDescription, string taskNotes, bool taskCompleted, DateTime taskDue, Guid? folderGuid) : base(taskDescription, taskNotes, taskCompleted, taskDue, folderGuid)
        {
            this.repetitionSchedule = repetitionSchedule;

        }

        public RepeatingTask(RepetitionSchedule repetitionSchedule) : base("", "", false, DateTime.Now, null)
        {
            this.repetitionSchedule = repetitionSchedule;
        }

        // Method to update the due date of a repeating task
        public void UpdateDueDate()
        {
            if (taskCompleted)
            {
                switch (repetitionSchedule)
                {
                    case RepetitionSchedule.Daily:
                        taskDue = taskDue.AddDays(1);
                        break;

                    case RepetitionSchedule.Weekly:
                        taskDue = taskDue.AddDays(7);
                        break;

                    case RepetitionSchedule.Monthly:
                        taskDue = taskDue.AddMonths(1);
                        break;

                    default: throw new ArgumentException("Unsupported repetition schedule.");
                }
            }
        }
    }
}
