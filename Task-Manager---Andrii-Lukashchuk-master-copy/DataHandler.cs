using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Task_Manager___Andrii_Lukashchuk
{
    // Data Handler class for saving and loading data to a binary file
    public class DataHandler
    {
        // File names for saving task and folder data
        private const string FileTaskName = "dataTask.bin";
        private const string FileFolderName = "dataFolder.bin";

        // Method to save task data to a binary file
        public async void SaveTaskData(List<Task> tasks)
        {
            // Get the local folder for storing data
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            // Create or replace the task data file
            StorageFile file = await storageFolder.CreateFileAsync(FileTaskName, CreationCollisionOption.ReplaceExisting);
            Debug.WriteLine(file.Path);

            try
            {
                // Open the file for writing
                using (var stream = File.Open(file.Path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        // Iterate over each task
                        foreach (var task in tasks)
                        {
                            // Write task data to the file
                            byte[] guidBytes = task.taskGuid.ToByteArray();

                            writer.Write(guidBytes, 0, guidBytes.Length);
                            writer.Write(task.taskDescription);
                            writer.Write(task.taskNotes);
                            writer.Write(task.taskCompleted);
                            writer.Write(task.taskDue.Ticks);
                            writer.Write(task.dateTaskCompleted.Ticks);
                            writer.Write(task.isTaskOverdue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during saving
                Debug.WriteLine($"Error while saving data: {ex.Message}");
            }
        }

        // Method to load task data from a binary file
        public async void LoadTaskData()
        {
            // Get the local folder for storing data
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                // Open the task data file for reading
                StorageFile file = await storageFolder.GetFileAsync(FileTaskName);

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        int record = 1;

                        // Iterate over each record in the file
                        while (record <= Task.taskCounter)
                        {
                            byte[] guidBytes = reader.ReadBytes(16);
                            var taskGuid = new Guid(guidBytes);

                            var taskDescription = reader.ReadString();
                            var taskNotes = reader.ReadString();
                            var taskCompleted = reader.ReadBoolean();
                            var taskDueTicks = reader.ReadInt64();
                            var taskDue = new DateTime(taskDueTicks);
                            var dateTaskCompletedTicks = reader.ReadInt64();
                            var dateTaskCompleted = new DateTime(dateTaskCompletedTicks);
                            var isTaskOverdue = reader.ReadBoolean();

                            // Output loaded task data to debug console
                            Debug.WriteLine($"Record number: {record}");
                            Debug.WriteLine(taskGuid);
                            Debug.WriteLine(taskDescription);
                            Debug.WriteLine(taskNotes);
                            Debug.WriteLine(taskCompleted);
                            Debug.WriteLine(taskDue);
                            Debug.WriteLine(dateTaskCompleted);
                            Debug.WriteLine(isTaskOverdue);
                            Debug.WriteLine("--------------------");
                            record++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during loading
                Debug.WriteLine($"Error while loading data: {ex.Message}");
            }
        }

        // Method to save folder data to a binary file
        public async void SaveFolderData(List<Folder> folders)
        {
            // Get the local folder for storing data
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            // Create or replace the folder data file
            StorageFile file = await storageFolder.CreateFileAsync(FileFolderName, CreationCollisionOption.ReplaceExisting);
            Debug.WriteLine(file.Path);

            try
            {
                // Open the file for writing
                using (var stream = File.Open(file.Path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        // Iterate over each folder
                        foreach (var folder in folders)
                        {
                            // Write folder data to the file
                            byte[] guidBytes = folder.folderGuid.ToByteArray();
                            writer.Write(guidBytes);
                            writer.Write(folder.folderName);
                            writer.Write(folder.listOfTasksGuids.Count);

                            // Write task GUIDs associated with the folder
                            foreach (var taskId in folder.listOfTasksGuids)
                            {
                                writer.Write(taskId.ToByteArray());
                            }

                            writer.Write(folder.incompleteTaskCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during saving
                Debug.WriteLine($"Error while saving data: {ex.Message}");
            }
        }

        // Method to load folder data from a binary file
        public async void LoadFolderData()
        {
            // Get the local folder for storing data
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                // Open the folder data file for reading
                StorageFile file = await storageFolder.GetFileAsync(FileFolderName);

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        int record = 1;

                        // Iterate over each record in the file
                        while (record <= Folder.folderCounter)
                        {
                            // Read folder data from the file
                            byte[] guidBytes = reader.ReadBytes(16);
                            var folderGuid = new Guid(guidBytes);
                            var folderName = reader.ReadString();
                            var folderTasksCounter = reader.ReadInt32();

                            // Output loaded folder data to debug console
                            Debug.WriteLine($"Record number: {record}");
                            Debug.WriteLine(folderGuid);
                            Debug.WriteLine(folderTasksCounter);

                            // Read task GUIDs associated with the folder
                            for (int i = 0; i < folderTasksCounter; i++)
                            {
                                var folderTasksGuid = new Guid(reader.ReadBytes(16));
                                Debug.WriteLine(folderTasksGuid);
                            }

                            var folderIncompleteTaskCount = reader.ReadInt32();
                            Debug.WriteLine(folderIncompleteTaskCount);

                            Debug.WriteLine("--------------------");
                            record++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during loading
                Debug.WriteLine($"Error while loading data: {ex.Message}");
            }
        }
    }
}
