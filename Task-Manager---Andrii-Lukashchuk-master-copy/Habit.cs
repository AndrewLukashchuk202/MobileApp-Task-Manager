using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Task_Manager___Andrii_Lukashchuk.MainPage;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// Represents a habit in the Task Manager.
    /// </summary>
    public class Habit : RepeatingTask
    {
        // Habit duration tracker
        public int habitDurationTracker { get; set; }

        // Constructor
        public Habit(int habitDurationTracker, RepetitionSchedule repetitionSchedule, string taskDescription, string taskNotes, bool taskCompleted, DateTime taskDue) : base(repetitionSchedule, taskDescription, taskNotes, taskCompleted, taskDue)
        {
            this.habitDurationTracker = habitDurationTracker;
        }

        // Method to update habit duration
        public void UpdateHabitDuration()
        {
            if (!isTaskOverdue && taskCompleted == true)
            {
                habitDurationTracker++;
            }
            else if (isTaskOverdue && taskCompleted == false)
            {
                habitDurationTracker = 0;
            }
        }
    }
}
