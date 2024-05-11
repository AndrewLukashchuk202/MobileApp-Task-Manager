using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// Represents a task in the Task Manager.
    /// </summary>
    public class Task : IComparable<Task>
    {
        // Properties of a task

        private DateTime _taskDue;

        public Guid taskGuid { get; }
        public string taskDescription { get; set; }
        public string taskNotes { get; set; }
        public bool taskCompleted { get; set; }

        DataModelV2 sqlModel = new DataModelV2();

        public DateTime taskDue
        {
            get { return _taskDue; }
            set
            {
                if (_taskDue != value)
                {
                    _taskDue = value;

                    // Re-sorting the list everytime we update task due Date
                    listOfTasks.Sort();
                }
            }
        }

        // Optional feature: date when the task was completed
        public DateTime dateTaskCompleted { get; set; }

        // List to store tasks
        public static List<Task> listOfTasks = new List<Task>();

        // Current system date and time
        public DateTime currentSystemDateTime = DateTime.Now;

        public static int taskCounter = 0;

        // Constructor
        public Task(string taskDescription, string taskNotes, bool taskCompleted, DateTime taskDue)
        {
            this.taskGuid = Guid.NewGuid();
            this.taskDescription = taskDescription;
            this.taskNotes = taskNotes;
            this.taskCompleted = taskCompleted;
            this._taskDue = taskDue;

            // Adding the task to the list of tasks upon creation
            listOfTasks.Add(this);
            taskCounter++;

            sqlModel.AddTask(this);
        }

        // Constructor
        public Task()
        {
            this.taskGuid = Guid.NewGuid();
            this.taskDescription = "";
            this.taskNotes = "";
            this.taskCompleted = false;
            this._taskDue = DateTime.MaxValue.Date;
            // Adding the task to the list of tasks upon creation
            listOfTasks.Add(this);
            taskCounter++;

            sqlModel.AddTask(this);
        }

        /// <summary>
        /// Extracts date from the given sentence.
        /// </summary>
        /// <param name="sentence">The input sentence.</param>
        /// <returns>The extracted date or null if not found.</returns>
        public static DateTime? GetDate(string sentence)
        {
            // Define delimiters to split the sentence
            char[] delimitersChars = { ' ', ',', '.', ':', '\t' };

            //DateTime newDateTask = currentSystemDateTime;

            DateTime currentDate = DateTime.Now.Date;
            DateTime targetDate = currentDate;

            Dictionary<string, int> weekDays = new Dictionary<string, int>()
                {
                   { "sunday", 0 },
                    { "monday", 1 },
                    { "tuesday", 2 },
                    { "wednesday", 3 },
                    { "thursday", 4 },
                    { "friday", 5 },
                    { "saturday", 6 },
                };

            Dictionary<string, int> adjectives = new Dictionary<string, int>
                {
                    {"this", 1},
                    {"next", 2},
                    {"tomorrow", 3}
                };

            string[] words = sentence.ToLower().Split(delimitersChars);

            // Initialize variables to track the week and days
            string week = "";
            int days = 0;


            try
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (adjectives.ContainsKey(words[i]))
                    {
                        week = words[i];
                    }

                    if (weekDays.ContainsKey(words[i]))
                    {
                        if (week == "this" || week == "")
                        {
                            days = weekDays[words[i]] - (int)currentDate.DayOfWeek;
                            if (days < 0) days += 7;
                        }
                        else if (week == "next")
                        {
                            days = weekDays[words[i]] - (int)currentDate.DayOfWeek;

                            if (days <= 0)
                            {
                                if (words.Contains("sunday"))
                                {
                                    days += 7;
                                }

                                days += 7;
                            }
                            // check if today is not sunday
                            else if (days > 0 && (int)currentDate.DayOfWeek != 0)
                            {
                                days += 7;
                            }
                        }
                        targetDate = currentDate.AddDays(days);
                    }
                    else if (week == "tomorrow")
                    {
                        days += 1;
                        targetDate = currentDate.AddDays(days);
                    }
                }


                if (currentDate == targetDate && week != "this")
                {
                    return null;
                }
                else
                {
                    return targetDate.Date;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Wrong input: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Extracts time from the given sentence.
        /// </summary>
        /// <param name="sentence">The input sentence.</param>
        /// <returns>The extracted time or null if not found.</returns>
        public static TimeSpan? GetTime(string sentence)
        {

            TimeSpan timeResult = new TimeSpan();

            //Morning - 6:00 -> 12:00;
            //Aternoon - 12:00 -> 18:00;
            //Evening - 18:00 -> 00:00;
            //Night - 00:00 -> 6:00

            //AM - 00:00 - 12:00
            //PM - 12:00 - 00:00

            // Define delimiters to split the sentence
            char[] delimitersChars = { ' ', ',', '.', ':', '\t' };

            string[] words = sentence.Split(delimitersChars);

            Dictionary<string, int> digits = new Dictionary<string, int>() {
                    {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7},
                    {"eight", 8}, {"nine", 9}, {"ten", 10}, {"eleven", 11}, {"twelve", 12}, {"thirteen", 13}, {"fourteen", 14},
                    {"fifteen", 15}, {"sixteen", 16}, {"seventeen", 17}, {"eighteen", 18}, {"nineteen", 19}, {"twenty", 20}, {"thirty", 30},
                    {"forty", 40}, {"fifty", 50}, {"half", 30}, {"quarter", 15}
                };

            int hours = 0, minutes = 0;

            sentence = sentence.ToLower();

            bool isPM = sentence.Contains("pm") || sentence.Contains("afternoon") || sentence.Contains("evening") || sentence.Contains("noon");
            bool isAM = sentence.Contains("am") || sentence.Contains("morning") || sentence.Contains("night");

            bool isTimeProvided = false;

            for (int currentWordIndex = 0; currentWordIndex < words.Length; currentWordIndex++)
            {
                if ((words[currentWordIndex] == "to" || words[currentWordIndex] == "past") && currentWordIndex > 0 && currentWordIndex < words.Length - 1)
                {
                    string previousWord = words[currentWordIndex - 1];
                    string nextWord = words[currentWordIndex + 1];

                    if (digits.ContainsKey(previousWord))
                    {
                        if (digits.ContainsKey(nextWord))
                        {
                            hours = digits[nextWord];
                        }
                        else
                        {
                            hours = int.Parse(nextWord);
                        }

                        timeResult = new TimeSpan(hours, 0, 0);

                        TimeSpan timeAdjustment;

                        if (words[currentWordIndex] == "to")
                        {
                            timeAdjustment = new TimeSpan(0, digits[previousWord], 0);
                            timeResult = timeResult.Subtract(timeAdjustment);
                        }
                        else // "past"
                        {
                            timeAdjustment = new TimeSpan(0, digits[previousWord], 0);
                            timeResult.Add(timeAdjustment);
                        }

                        isTimeProvided = true;
                    }
                    else if (int.TryParse(previousWord, out int minute))
                    {
                        if (digits.ContainsKey(nextWord))
                        {
                            hours = digits[nextWord];
                        }
                        else
                        {
                            hours = int.Parse(nextWord);
                        }

                        timeResult = new TimeSpan(hours, 0, 0);

                        TimeSpan timeAdjustment;

                        if (words[currentWordIndex] == "to")
                        {
                            timeAdjustment = new TimeSpan(0, minute, 0);
                            timeResult = timeResult.Subtract(timeAdjustment);
                        }
                        else
                        {
                            timeAdjustment = new TimeSpan(0, minute, 0);
                            timeResult = timeResult.Add(timeAdjustment);
                        }

                        isTimeProvided = true;
                    }
                }
            }


            if (!isTimeProvided)
            {
                try
                {
                    foreach (string word in words)
                    {
                        // case when we use number in our sentence, example: 12:45 PM
                        if (int.TryParse(word, out int number))
                        {
                            if (hours == 0)
                            {
                                if ((isPM && number < 12) || (isAM && number == 12))
                                {
                                    hours = number + 12;

                                    isTimeProvided = true;
                                }
                                else if ((isAM && number < 12) || (isPM && number == 12))
                                {
                                    hours = number;
                                    isTimeProvided = true;
                                }
                                else
                                {
                                    hours = number;
                                    isTimeProvided = true;
                                }
                            }
                            else if (hours != 0)
                            {
                                minutes = number;
                            }
                            timeResult = new TimeSpan(hours, minutes, 0);
                        }
                        //case when we use words to tell about time, example: twelve forty-five
                        else if (digits.ContainsKey(word))
                        {
                            if (hours == 0)
                            {
                                if ((isPM && digits[word] < 12) || (isAM && digits[word] == 12))
                                {
                                    hours = digits[word] + 12;
                                    isTimeProvided = true;
                                }
                                else if ((isAM && digits[word] < 12) || (isPM && digits[word] == 12))
                                {
                                    hours = digits[word];
                                    isTimeProvided = true;

                                }
                                else
                                {
                                    hours = digits[word];
                                    isTimeProvided = true;
                                }
                            }
                            else if (hours != 0)
                            {
                                minutes = digits[word];
                            }
                            else if (word.Contains("-"))
                            {
                                string[] minutesPart = word.Split("-");

                                minutes = digits[minutesPart[0]] + digits[minutesPart[1]];
                            }
                            timeResult = new TimeSpan(hours, minutes, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Wrong input - ", ex.Message);
                    return null;
                }
            }



            if (isTimeProvided)
            {
                return timeResult;
            }
            else
            {
                return null;
            }
        }

        //<Task> on <date> at <time>. Eg “Call Rob on Wednesday at three PM”
        /*•<Task> at<time> on<date>.Eg “Call Rob at three PM on Wednesday”
        • <Task>. Eg. “Call Rob”
        • <Task>, <time>, <date>. Eg. “Call Rob, three PM, Wednesday”
        • <Task>, <date>, <time>. Eg. “Call Rob, Wednesday, three PM”
        • <Task> <time> <date>. Eg. “Call Rob three PM Wednesday”
        • <Task> <date> <time>. Eg. “Call Rob Wednesday three PM” 
        */

        public static Task CreateTask(string sentence)
        {
            /*this.taskGuid = Guid.NewGuid();
            this.taskDescription = taskDescription;
            this.taskNotes = taskNotes;
            this.taskCompleted = taskCompleted;
            this._taskDue = taskDue;*/




            char[] delimitersChars = { ' ', ',', '.', ':', '\t', '-' };

            Dictionary<string, int> keyWordsToRemove = new Dictionary<string, int>() {
                    {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5}, {"six", 6}, {"seven", 7},
                    {"eight", 8}, {"nine", 9}, {"ten", 10}, {"eleven", 11}, {"twelve", 12}, {"thirteen", 13}, {"fourteen", 14},
                    {"fifteen", 15}, {"sixteen", 16}, {"seventeen", 17}, {"eighteen", 18}, {"nineteen", 19}, {"twenty", 20}, {"thirty", 30},
                    {"forty", 40}, {"fifty", 50}, {"half", 30}, {"quarter", 15}, {"at", 0}, {"on", 0}, {"morning", 0},
                    {"evening", 0}, {"night", 0}, {"afternoon", 0}, {"noon", 0}, {"this", 0}, {"next", 0}, {"sunday", 0},
                    {"saturday", 0}, {"monday", 0}, {"tuesday", 0}, {"wednesday", 0}, {"thursday", 0}, {"friday", 0},
                    {"pm", 0}, {"am", 0}, {"by", 0}, {"tomorrow", 0}
                };

            DateTime? dueDate = GetDate(sentence);
            TimeSpan? dueTime = GetTime(sentence);


            Task newTask = new Task();

            if (sentence == null)
            {
                return null;
            }
            else
            {
                List<string> processedSentence = sentence.Split(delimitersChars).ToList();

                newTask.taskNotes = sentence;



                for (int i = 0; i < processedSentence.Count; i++)
                {
                    bool isNumber = int.TryParse(processedSentence[i], out int number);

                    if (keyWordsToRemove.ContainsKey(processedSentence[i].ToLower()))
                    {
                        processedSentence.RemoveAt(i);
                        i--;
                    }
                    else if (isNumber && keyWordsToRemove.ContainsValue(number))
                    {
                        processedSentence.RemoveAt(i);
                        i--;
                    }
                    else if (isNumber)
                    {
                        processedSentence.RemoveAt(i);
                        i--;
                    }
                }

                newTask.taskDescription = string.Join(" ", processedSentence);

                bool timeProvided = false;

                if (dueDate != null)
                {
                    newTask.taskDue = dueDate.Value;

                    if (dueTime != null)
                    {
                        newTask.taskDue = newTask.taskDue.Date + dueTime.Value;
                        timeProvided = true;
                    }
                    else
                    {
                        newTask.taskDue = newTask.taskDue.Date;
                    }

                }
                else if (dueTime != null && !timeProvided)
                {

                    newTask.taskDue = newTask.taskDue.Date + dueTime.Value;
                }

                return newTask;
            }

        }

        // Property to determine if a task is overdue
        public bool isTaskOverdue
        {
            get
            {
                if (currentSystemDateTime >= taskDue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Method to add a task
        public void AddTask(Task task)
        {
            listOfTasks.Add(task);

            // Will sort list automatically everytime we add a new task to it
            listOfTasks.Sort();

            sqlModel.AddTask(this);
        }

        // Method to remove a task
        public void RemoveTask(Task task)
        {

            listOfTasks.Remove(task);
            // Will sort list automatically everytime we remove the task from it
            listOfTasks.Sort();

            sqlModel.DeleteTask(this);
        }

        // Method to compare two due dates of two different tasks
        public int CompareTo(Task other)
        {
            return this.taskDue.CompareTo(other.taskDue);
        }

        // Method to compare two tasks by their description names
        public static int CompareTasksByDescriptionName(Task task1, Task task2)
        {
            return task1.taskDescription.CompareTo(task2.taskDescription);
        }
    }
}
