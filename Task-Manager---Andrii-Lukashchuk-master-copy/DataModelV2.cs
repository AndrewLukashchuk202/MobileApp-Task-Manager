using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Data;
using System.Globalization;

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// Represents a data model for managing tasks and folders using SQLite.
    /// </summary>
    public class DataModelV2
    {
        private static SqliteConnection database;
        private static string filename = "myDatabaseTaskV2.db";

        //private const string InitializedKey = "IsDatabaseInitialized";

        private static bool isInitialised = false;

        /*public static bool IsInitialized
        {
            get { return ApplicationData.Current.LocalSettings.Values.ContainsKey(InitializedKey); }
        }*/

        public DataModelV2()
        {
            InitialiseDatabase();
        }

        public static async void InitialiseDatabase()
        {
            if (!isInitialised)
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
                                                  "taskDescription VARCHAR(100) NOT NULL, " +
                                                  "taskNotes VARCHAR(100), " +
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

                    isInitialised = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error creating tables: " + ex.Message);
                }
                finally
                {
                    if (database != null)
                    {
                        database.Close();
                    }
                }
            }
        }

        public static void AddTask(Task task)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                // Check if the task already exists in the database
                string checkTaskQuery = $"SELECT COUNT(*) FROM Task WHERE taskId = '{task.taskGuid.ToString()}'";
                SqliteCommand checkTaskCommand = new SqliteCommand(checkTaskQuery, database);
                int existingTaskCount = Convert.ToInt32(checkTaskCommand.ExecuteScalar());

                if (existingTaskCount > 0)
                {
                    // Task with the same taskId already exists, handle accordingly
                    Debug.WriteLine("Task with the same taskId already exists.");
                    return; // Exit the method without adding the task
                }

                String insertDataQuery = $"INSERT INTO Task (taskId, taskDescription, taskNotes, taskCompleted, taskDue, dateTaskCompleted," +
                    $"isTaskOverdue, folderId) VALUES ('{task.taskGuid.ToString()}', '{task.taskDescription}', '{task.taskNotes}', '{task.taskCompleted}'," +
                    $"'{task.taskDue}', '{task.dateTaskCompleted}', '{task.isTaskOverdue}', '{task.folderGuid}');";

                Debug.WriteLine(insertDataQuery);
                SqliteCommand insertData = new SqliteCommand(insertDataQuery, database);
                insertData.ExecuteNonQuery(); // Use ExecuteNonQuery instead of ExecuteReader
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding task: " + ex.Message);
            }
            /*finally
            {
                if (database != null && database.State == ConnectionState.Open)
                {
                    database.Close();
                }
            }*/
        }


        public static void DeleteTask(Task task)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

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
            finally
            {
                if (database != null)
                {
                    database.Close();
                }
            }
        }

        public static void UpdateTask(Task task)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                string updateDataQuery = $"UPDATE Task SET " +
                    $"taskDescription = '{task.taskDescription}', " +
                    $"taskCompleted = '{task.taskCompleted}', " +
                    $"taskNotes = '{task.taskNotes}', " +
                    $"taskDue = '{task.taskDue}', " +
                    $"dateTaskCompleted = '{task.dateTaskCompleted}', " +
                    $"isTaskOverdue = '{task.isTaskOverdue}' " +
                    $"WHERE taskId = '{task.taskGuid.ToString()}'";
                Debug.WriteLine(updateDataQuery);
                SqliteCommand updateData = new SqliteCommand(updateDataQuery, database);
                updateData.ExecuteReader();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating task: " + ex.Message);
            }
            finally
            {
                if (database != null)
                {
                    database.Close();
                }
            }
        }

        public static void ReadTaskData()
        {

            try
            {

                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                String getDataQuery = "SELECT * FROM Task;";
                SqliteCommand getData = new SqliteCommand(getDataQuery, database);
                SqliteDataReader queryResult = getData.ExecuteReader();

                int taskNumber = 1;

                while (queryResult.Read())
                {
                    Debug.WriteLine("Task number: " + taskNumber);
                    Debug.WriteLine(queryResult.GetString(0));
                    Debug.WriteLine(queryResult.GetString(1));
                    Debug.WriteLine(queryResult.GetString(2));
                    Debug.WriteLine(queryResult.GetBoolean(3));
                    //Debug.WriteLine(queryResult.GetDateTime(4));
                    Debug.WriteLine(queryResult.GetDateTime(5));
                    Debug.WriteLine(queryResult.GetBoolean(6));
                    Debug.WriteLine(queryResult.GetString(7));
                    Debug.WriteLine("");
                    taskNumber++;

                    /*string createTaskTableQuery = "CREATE TABLE IF NOT EXISTS Task (taskId VARCHAR(36) PRIMARY KEY NOT NULL, " +
                                                  "taskDescription VARCHAR(100) NOT NULL, " +
                                                  "taskNotes VARCHAR(100), " +
                                                  "taskCompleted BOOLEAN NOT NULL, " +
                                                  "taskDue DATETIME NOT NULL, " +
                                                  "dateTaskCompleted DATETIME NOT NULL," +
                                                  "isTaskOverdue BOOLEAN NOT NULL," +
                                                  "folderId VARCHAR(36))";*/

                    Guid taskGuid = Guid.Parse(queryResult.GetString(0));
                    string taskDescription = queryResult.GetString(1);
                    string taskNotes = queryResult.GetString(2);
                    bool taskCompleted = queryResult.GetBoolean(3);

                    //since DateTime created in MM/dd/YYYY format and we pass dd/MM/YYYY, we have to convert it
                    DateTime taskDue = DateTime.ParseExact(queryResult.GetString(4), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

                    Guid folderGuid;

                    bool isTaskFound = false;

                    foreach (Task task in Task.listOfTasks)
                    {
                        if (task.taskGuid == taskGuid)
                        {
                            task.taskDescription = taskDescription;
                            task.taskNotes = taskNotes;
                            task.taskCompleted = taskCompleted;
                            task.taskDue = taskDue;
                            if (Guid.TryParse(queryResult.GetString(7), out folderGuid))
                            {
                                task.folderGuid = folderGuid;
                            }
                            isTaskFound = true;
                            break;
                        }
                    }

                    if (!isTaskFound)
                    {
                        if (Guid.TryParse(queryResult.GetString(7), out folderGuid))
                        {
                            Task newTask = new Task(taskGuid, taskDescription, taskNotes, taskCompleted, taskDue, folderGuid);
                        }
                        else
                        {
                            Task newTask = new Task(taskGuid, taskDescription, taskNotes, taskCompleted, taskDue, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading folder data: " + ex.Message);
            }
            finally
            {
                // Ensure that the database connection is closed after use
                if (database != null)
                {
                    database.Close();
                }
            }
        }

        public static void AddFolder(Folder folder)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                // Check if the folder with the given folderId already exists
                bool folderExists = false;
                string checkExistenceQuery = $"SELECT COUNT(*) FROM Folder WHERE folderId = '{folder.folderGuid.ToString()}'";
                SqliteCommand checkExistenceCmd = new SqliteCommand(checkExistenceQuery, database);
                long count = (long)checkExistenceCmd.ExecuteScalar();
                folderExists = count > 0;

                if (folderExists)
                {
                    Debug.WriteLine($"Folder with id '{folder.folderGuid}' already exists.");
                    return; // or handle the situation as needed
                }

                // Insert the folder if it doesn't already exist
                string insertDataQuery = $"INSERT INTO Folder (folderId, folderName, folderTaskCount, folderTaskIncomplete) " +
                                         $"VALUES ('{folder.folderGuid.ToString()}', '{folder.folderName}', " +
                                         $"{(folder.listOfTasksGuids != null ? folder.listOfTasksGuids.Count.ToString() : "0")}, " +
                                         $"'{folder.incompleteTaskCount}')";

                Debug.WriteLine(insertDataQuery);
                SqliteCommand insertData = new SqliteCommand(insertDataQuery, database);
                insertData.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error adding folder: " + ex.Message);
            }
            /*finally
            {
                if (database != null)
                {
                    database.Close();
                }
            }*/
        }


        public static void DeleteFolder(Folder folder)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                string folderGuid = folder.folderGuid.ToString();
                string deleteDataQuery = $"DELETE FROM Folder WHERE folderId = '{folderGuid}'";
                Debug.WriteLine(deleteDataQuery);
                SqliteCommand deleteData = new SqliteCommand(deleteDataQuery, database);
                deleteData.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error deleting task: " + ex.Message);
            }
            finally
            {
                if (database != null)
                {
                    database.Close();
                }
            }
        }

        public static void UpdateFolder(Folder folder)
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

                string updateDataQuery = $"UPDATE Folder SET " +
                    $"folderName = '{folder.folderName}', " +
                    $"folderTaskCount = '{folder.listOfTasksGuids.Count}', " +
                    $"folderTaskIncomplete = '{folder.incompleteTaskCount}' " +
                    $"WHERE folderId = '{folder.folderGuid.ToString()}'";
                Debug.WriteLine(updateDataQuery);
                SqliteCommand updateData = new SqliteCommand(updateDataQuery, database);
                updateData.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating folder: " + ex.Message);
            }
            finally 
            { 
                if (database != null)
                {
                    database.Close();
                } 
            }
        }

        public static void ReadFolderData()
        {
            try
            {
                // Open the database connection if it's not already open
                if (database != null && database.State != ConnectionState.Open)
                {
                    database.Open();
                }

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

                    Guid folderGuid = Guid.Parse(queryResult.GetString(0));
                    string folderName = queryResult.GetString(1);
                    int folderTaskAmount = queryResult.GetInt32(2);

                    // Check if a folder with provided Guid already exists.
                    // If does we just pass the values (they might be updated), if does not we just create a new folder
                    bool isFolderFound = false;

                    foreach (Folder folder in Folder.listOfFolders)
                    {
                        if (folder.folderGuid == folderGuid)
                        {
                            folder.folderName = folderName;
                            folder.folderTaskAmount = folderTaskAmount.ToString();
                            isFolderFound = true;
                            break;
                        }
                    }

                    if (!isFolderFound)
                    {
                        Folder newFolder = new Folder(folderGuid, folderName, folderTaskAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading folder data: " + ex.Message);
            }
            finally
            {
                // Ensure that the database connection is closed after use
                if (database != null)
                {
                    database.Close();
                }
            }
        }

    }
}
