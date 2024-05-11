using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// Represents a data model for managing tasks and folders using SQLite.
    /// </summary>
    public class DataModelV2
    {
        private static SqliteConnection database;
        private string filename = "myDatabaseTaskV2.db";

        public DataModelV2()
        {
            InitialiseDatabase();
        }

        public async void InitialiseDatabase()
        {
            // Create the database file if it doesn’t exist 
            var appLocalFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine(appLocalFolder.Path);
            await appLocalFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

            // Open the database
            string fullFilePath = Path.Combine(appLocalFolder.Path, filename);
            database = new SqliteConnection("Filename=" + fullFilePath);
            database.Open();

            try
            {
                // Create Task Table
                string createTaskTableQuery = "CREATE TABLE IF NOT EXISTS Task (taskId VARCHAR(36) PRIMARY KEY NOT NULL, " +
                                              "taskDescription VARCHAR(100) NOT NULL," +
                                              "taskCompleted BOOLEAN NOT NULL, " +
                                              "taskDue DATETIME NOT NULL, " +
                                              "dateTaskCompleted DATETIME NOT NULL," +
                                              "isTaskOverdue BOOLEAN NOT NULL," +
                                              "folderId VARCHAR(36))";
                SqliteCommand createTaskTable = new SqliteCommand(createTaskTableQuery, database);
                createTaskTable.ExecuteReader();

                // Create Folder Table
                string createFolderTableQuery = "CREATE TABLE IF NOT EXISTS Folder (folderId VARCHAR(36) PRIMARY KEY NOT NULL," +
                                                "folderName VARCHAR(50) NOT NULL," +
                                                "folderTaskCount INT NOT NULL," +
                                                "folderTaskIncomplete INT NOT NULL)";
                SqliteCommand createFolderTable = new SqliteCommand(createFolderTableQuery, database);
                createFolderTable.ExecuteReader();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating tables: " + ex.Message);
            }
        }

        public void AddTask(Task task)
        {
            string folderId = "";

            foreach (var folder in Folder.listOfFolders)
            {
                if (folder.listOfTasksGuids.Contains(task.taskGuid))
                {
                    folderId = folder.folderGuid.ToString();
                }
            }

            try
            {
                String insertDataQuery = $"INSERT INTO Task (taskId, taskDescription, taskCompleted, taskDue, dateTaskCompleted," +
                    $"isTaskOverdue, folderId) VALUES ('{task.taskGuid.ToString()}', '{task.taskDescription}', '{task.taskCompleted}'," +
                    $"'{task.taskDue}', '{task.dateTaskCompleted}', '{task.isTaskOverdue}', '{folderId}');";
                Debug.WriteLine(insertDataQuery);
                SqliteCommand insertData = new SqliteCommand(insertDataQuery, database);
                insertData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding task: " + ex.Message);
            }
        }

        public void DeleteTask(Task task)
        {
            try
            {
                string taskGuid = task.taskGuid.ToString();
                string deleteDataQuery = $"DELETE FROM Task WHERE taskId = '{taskGuid}'";
                Debug.WriteLine(deleteDataQuery);
                SqliteCommand deleteData = new SqliteCommand(deleteDataQuery, database);
                deleteData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error deleting task: " + ex.Message);
            }
        }

        public void UpdateTask(Task task)
        {
            try
            {
                string updateDataQuery = $"UPDATE Task SET " +
                    $"taskDescription = '{task.taskDescription}', " +
                    $"taskCompleted = '{task.taskCompleted}', " +
                    $"taskDue = '{task.taskDue}', " +
                    $"dateTaskCompleted = '{task.dateTaskCompleted}', " +
                    $"isTaskOverdue = '{task.isTaskOverdue}', " +
                    $"WHERE taskId = '{task.taskGuid.ToString()}'";
                Debug.WriteLine(updateDataQuery);
                SqliteCommand updateData = new SqliteCommand(updateDataQuery, database);
                updateData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating task: " + ex.Message);
            }
        }

        public void ReadTaskData()
        {
            String getDataQuery = "SELECT * FROM Task;";
            SqliteCommand getData = new SqliteCommand(getDataQuery, database);
            SqliteDataReader queryResult = getData.ExecuteReader();

            int taskNumber = 1;

            while (queryResult.Read())
            {
                Debug.WriteLine("Task number: " + taskNumber);
                Debug.WriteLine(queryResult.GetString(0));
                Debug.WriteLine(queryResult.GetString(1));
                Debug.WriteLine(queryResult.GetBoolean(2));
                Debug.WriteLine(queryResult.GetDateTime(3));
                Debug.WriteLine(queryResult.GetDateTime(4));
                Debug.WriteLine(queryResult.GetBoolean(5));
                Debug.WriteLine(queryResult.GetString(6));
                Debug.WriteLine("");
                taskNumber++;
            }
        }

        public void AddFolder(Folder folder)
        {
            try
            {
                String insertDataQuery = $"INSERT INTO Folder (folderId, folderName, folderTaskCount, folderTaskIncomplete) " +
                    $"VALUES ('{folder.folderGuid.ToString()}', '{folder.folderName}', '{folder.listOfTasksGuids.Count}', " +
                    $"'{folder.incompleteTaskCount}')";

                Debug.WriteLine(insertDataQuery);
                SqliteCommand insertData = new SqliteCommand(insertDataQuery, database);
                insertData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding folder: " + ex.Message);
            }
        }

        public void DeleteFolder(Folder folder)
        {
            try
            {
                string folderGuid = folder.folderGuid.ToString();
                string deleteDataQuery = $"DELETE FROM Folder WHERE folderId = '{folderGuid}'";
                Debug.WriteLine(deleteDataQuery);
                SqliteCommand deleteData = new SqliteCommand(deleteDataQuery, database);
                deleteData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error deleting task: " + ex.Message);
            }
        }

        public void UpdateFolder(Folder folder)
        {
            try
            {
                string updateDataQuery = $"UPDATE Folder SET " +
                    $"folderName = '{folder.folderName}', " +
                    $"folderTaskCount = '{folder.listOfTasksGuids.Count}', " +
                    $"foldetTaskIncomplete = '{folder.incompleteTaskCount}' " +
                    $"WHERE folderId = '{folder.folderGuid.ToString()}'";
                Debug.WriteLine(updateDataQuery);
                SqliteCommand updateData = new SqliteCommand(updateDataQuery, database);
                updateData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating folder: " + ex.Message);
            }
        }

        public void ReadFolderData()
        {
            String getDataQuery = "SELECT * FROM Folder;";
            SqliteCommand getData = new SqliteCommand(getDataQuery, database);
            SqliteDataReader queryResult = getData.ExecuteReader();

            int folderNumber = 1;

            while (queryResult.Read())
            {
                Debug.WriteLine("Folder number: " + folderNumber);
                Debug.WriteLine(queryResult.GetString(0));
                Debug.WriteLine(queryResult.GetString(1));
                Debug.WriteLine(queryResult.GetInt32(2));
                Debug.WriteLine(queryResult.GetInt32(3));
                Debug.WriteLine("");
                folderNumber++;
            }
        }
    }
}
