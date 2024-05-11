using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// This class provides methods for testing task creation using sample sentences.
    /// </summary>
    public class Tester
    {
        public static void TestTaskCreation()
        {
            string[] sentences = {
                 "Doctor’s appointment next Tuesday at nine",
            "Call Rob on Wednesday at three PM",
            "Meeting with John at four PM on Friday",
            "Finish report by 10:00 AM tomorrow",
            "Prepare presentation on Thursday at 2:30 PM",
            "Buy groceries at six PM",
            "Dentist appointment at 9:30 AM on Saturday",
            "Meet Sarah at 7:45 AM on Sunday",
            "Conference call at 11:15 AM",
            "Study for exam at 8:00 PM tonight",
            "Workout at 6:30 AM",
            "Pick up dry cleaning at 5 PM",
            "Lunch with Tom at twelve thirty PM",
            "Feed the cat in an hour",
            "Review budget next week",
            "Go for a run this afternoon",
            "Call mom this evening",
            "Attend yoga class at 7 AM tomorrow",
            "Schedule meeting for next month",
            "Watch movie at 8:00 tonight",
            "Coffee with Alice at 11:30 AM",
            "Take the dog for a walk at 4:30 PM",
            "Dinner with friends at 7:00 PM",
            "Grocery shopping on Saturday morning",
            "Submit report by 5 PM tomorrow",
            "Go to bed at 10:00 PM",
            "Start reading book at 9:00 AM",
            "Book flight for vacation",
            "Complete online course by next Friday",
            "Pick up kids from school at 3:15 PM",
            "Water the plants at 9:30 AM on Sunday",
            "Finish coding project by 6 PM tomorrow",
            "Call customer support this afternoon",
            "Check email in an hour",
            "Attend webinar at 10:30 AM tomorrow",
            "Take medicine at 9:00 PM",
            "Visit dentist next month",
            "Go to the gym at 7:30 AM",
            "Send proposal by 3:00 PM on Friday",
            "Schedule dentist appointment for next week",
            "Call insurance company this morning",
            "Order groceries online for next week",
            "Pick up package from post office by 4 PM",
            "Clean the house this weekend",
            "Attend birthday party at 2:00 PM on Saturday",
            "Prepare for presentation at 9:00 AM tomorrow",
            "Buy birthday gift for Sarah",
            "Attend company meeting at 10 AM on Monday",
            "Book hotel for vacation",
            "Take out the trash tonight",
            "Meet with client at 2:30 PM",
            "Get haircut at 11:00 AM",
            "Pay bills by end of the month",
            "Call friend for catch-up",
            "Go to the bank this afternoon",
            "Schedule doctor's appointment for next week",
            "Finish painting the house by end of the week",
            "Buy new shoes this weekend",
            "Renew gym membership before expiration",
            "Pick up prescription from pharmacy at 6:45 PM",
            "Start new project next month",
            "Call tech support this morning",
            "Attend workshop at 1:00 PM on Friday",
            "Get car serviced next Tuesday",
            "Buy tickets for concert on Saturday night",
            "Go to the library tomorrow",
            "Meet with study group at 4:00 PM",
            "Have dinner with family at 6:00 PM",
            "Submit application by deadline",
            "Call landlord to fix leaky faucet",
            "Go to the cinema this evening",
            "Attend networking event at 5:30 PM",
            "Start new diet plan on Monday",
            "Visit parents next weekend",
            "Send birthday card to friend",
            "Plan vacation for summer",
            "Meet with financial advisor at 9:30 AM",
            "Practice guitar for an hour",
            "Take dog to the vet next Friday",
            "Call sister for her birthday",
            "Prepare for job interview tomorrow",
            "Go to the post office this afternoon",
            "Attend book club meeting at 7:00 PM",
            "Study for exam next week",
            "Review project proposal at 3:00 PM",
            "Buy new laptop before school starts",
            "Go for a hike on Sunday morning",
            "Have lunch with coworker at noon",
            "Submit expense report by Friday",
            "Call utility company to fix power outage",
            "Go to the beach next weekend",
            "Meet with accountant to file taxes",
            "Attend graduation ceremony on Saturday",
            "Start new book tonight",
            "Call friend for advice",
            "Attend parent-teacher conference next week",
            "Clean garage this weekend",
            "Plan surprise party for friend",
            "Pick up kids from soccer practice at 5:45 PM",
            "Buy anniversary gift for spouse",
            "Go to the doctor for checkup",
            "Attend concert on Friday night",
            "Start home renovation project next month",
            "Call plumber to fix leak",
            "Go to the museum this weekend",
            "Plan date night with partner",
            "Attend charity event at 6:30 PM",
            "Start new hobby this weekend",
            "Take online course on programming",
            "Call customer for feedback",
            "Go to the theater this evening",
            "Plan family vacation for summer"
             };

            //string[] sentences = { "Pick up prescription from pharmacy at 6:45 PM" };

            Debug.WriteLine("Testing Task Creation...");

            foreach (string sentence in sentences)
            {
                //Task newTask = new Task();
                Task newTask = Task.CreateTask(sentence);
                Debug.WriteLine("");
                Debug.WriteLine("Original sentence: " + sentence);
                Debug.WriteLine("Created task");
                Debug.WriteLine("Description: " + newTask.taskDescription);

                if (newTask.taskDue.Date != DateTime.MaxValue.Date)
                {
                    if (newTask.taskDue.TimeOfDay != DateTime.MinValue.TimeOfDay)
                    {
                        Debug.WriteLine("Due Date: " + newTask.taskDue);
                    }
                    else
                    {
                        Debug.WriteLine("Due Date: " + newTask.taskDue.Date);
                    }
                }
                else if (newTask.taskDue.TimeOfDay != DateTime.MinValue.TimeOfDay)
                {
                    Debug.WriteLine("Due Time: " + newTask.taskDue.ToString("HH:mm"));
                }
                else
                {
                    Debug.WriteLine("Due Date: Was not provided");
                }

                Debug.WriteLine("Completed: " + newTask.taskCompleted);
                Debug.WriteLine("Notes: " + newTask.taskNotes);
                Debug.WriteLine("");
            }

            Debug.WriteLine("Testing complete.");
        }
    }
}
