using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// Represents a folder to organize tasks in the Task Manager.
    /// </summary>
    public class Folder
    {
        public Guid folderGuid { get; }
        public string folderName { get; set; }
        public List<Guid> listOfTasksGuids { get; }

        public string folderTaskAmount { get; set; }

        public static int folderCounter = 0;

        // List to store folders
        public static List<Folder> listOfFolders = new List<Folder>();

        public Folder(string folderName)
        {
            this.folderGuid = Guid.NewGuid();
            this.folderName = folderName;
            this.listOfTasksGuids = new List<Guid>();

            // Adding the folder to the list of folders upon creation
            listOfFolders.Add(this);
            folderCounter++;

            DataModelV2.AddFolder(this);
        }

        //Constructor for passing values from sql database just for the cases where we already know a folder's GUID so we do not create a new one
        public Folder(Guid folderGuid, string folderName, int folderTaskAmount)
        {
            this.folderGuid = folderGuid;
            this.folderName = folderName;
            if (folderTaskAmount > 0)
            {
                this.folderTaskAmount = folderTaskAmount.ToString();
            }
            else
            {
                this.folderTaskAmount = "";
            }

            // Adding the folder to the list of folders upon creation
            listOfFolders.Add(this);
            folderCounter++;
            DataModelV2.AddFolder(this);
        }

        // Method to add a task to the folder
        public void AddTask(Guid taskId)
        {
            listOfTasksGuids.Add(taskId);
        }

        // Method to remove a task from the folder
        public void RemoveTask(Guid taskId)
        {
            listOfTasksGuids.Remove(taskId);
        }

        // Method to add a folder
        public void AddFolder(Folder folder)
        {
            listOfFolders.Add(folder);

            DataModelV2.AddFolder(this);
        }

        // Method to remove a folder
        public void RemoveFolder(Folder folder)
        {
            listOfFolders.Remove(folder);

            DataModelV2.DeleteFolder(this);
        }

        // Property to get the count of incomplete tasks in the folder
        public int incompleteTaskCount
        {
            get
            {
                int incompleteCount = 0;

                if (listOfTasksGuids != null)
                {
                    foreach (var taskId in listOfTasksGuids)
                    {
                        foreach (Task task in Task.listOfTasks)
                        {
                            if (task.taskGuid == taskId && task.taskCompleted == false)
                            {
                                incompleteCount++;
                            }
                        }
                    }
                    return incompleteCount;
                }
                return incompleteCount; ;
            }
        }
    }
}
