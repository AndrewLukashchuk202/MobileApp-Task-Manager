using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Globalization;
using System.ServiceModel.Channels;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTaskPage : Page
    {
        int indexOfChosenFolder = -1;
        public AddTaskPage()
        {
            this.InitializeComponent();
            comboBoxFolderList.ItemsSource = Folder.listOfFolders;
        }

        private async void ButtonTaskCreate_Click(object sender, RoutedEventArgs e)
        {
            DateTime dueTaskDate;

            bool isValidDate = DateTime.TryParseExact(dueDateTextBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueTaskDate);

            if (!isValidDate)
            {
                dueDateTextBox.Text = "";
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = new TextBlock { Text = "Due date format is wrong!" },
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
            else if (string.IsNullOrEmpty(taskDescriptionTextBox.Text))
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = new TextBlock { Text = "Task description is blank!" },
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
            }
            else
            {

                string taskDesrciption = taskDescriptionTextBox.Text;
                string taskNotes = taskNoteTextBox.Text ?? "";
                bool taskCompleted = isTaskDoneCheckBox.IsChecked ?? false;
                DateTime taskDue = DateTime.Parse(dueDateTextBox.Text);


                if (indexOfChosenFolder != -1)
                {
                    //We assigned task to a specific folder
                    //Folder.listOfFolders[indexOfChosenFolder].listOfTasksGuids.Add(task.taskGuid);
                    Task task = new Task(taskDesrciption, taskNotes, taskCompleted, taskDue, Folder.listOfFolders[indexOfChosenFolder].folderGuid);
                }
                else
                {
                    Task task = new Task(taskDesrciption, taskNotes, taskCompleted, taskDue, null);
                }

                Frame.Navigate(typeof(TasksPage));
            }
        }

        private void ButtonGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private void comboBoxFolderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as Folder;

            if (selectedItem != null)
            {
                Guid selectedFolderGuid = selectedItem.folderGuid;

                foreach (Folder folder in Folder.listOfFolders)
                {
                    if (folder.folderGuid == selectedFolderGuid)
                    {
                        //folder.listOfTasksGuids.Add(recievedTask.taskGuid);
                        indexOfChosenFolder = Folder.listOfFolders.IndexOf(folder);
                    }
                }
            }
        }
    }
}
