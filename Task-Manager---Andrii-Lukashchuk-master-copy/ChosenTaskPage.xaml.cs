using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChosenTaskPage : Page
    {
        Task recievedTask;
        int indexOfChosenFolder = -1;

        public ChosenTaskPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recievedTask = e.Parameter as Task;

            if (recievedTask != null)
            {
                taskDescriptionTextBox.Text = recievedTask.taskDescription;
                taskNoteTextBox.Text = recievedTask.taskNotes;
                dueDateTextBox.Text = recievedTask.taskDue.ToString("dd/MM/yyyy");
                isTaskDoneCheckBox.IsChecked = recievedTask.taskCompleted ? true : false;
                comboBoxFolderList.ItemsSource = Folder.listOfFolders;

                Debug.WriteLine(Folder.listOfFolders.Count);
            }
        }

        private async void ButtonUpdateTask_Click(object sender, RoutedEventArgs e)
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

                recievedTask.taskDescription = taskDescriptionTextBox.Text;
                recievedTask.taskNotes = taskNoteTextBox.Text;
                recievedTask.taskCompleted = isTaskDoneCheckBox.IsChecked ?? false;
                recievedTask.taskDue = DateTime.Parse(dueDateTextBox.Text);

                if (indexOfChosenFolder != -1)
                {
                    //assing task to a specific folder
                    //Folder.listOfFolders[indexOfChosenFolder].listOfTasksGuids.Add(recievedTask.taskGuid);
                    recievedTask.folderGuid = Folder.listOfFolders[indexOfChosenFolder].folderGuid;
                }

                //updating the information for that chosen task
                DataModelV2.UpdateTask(recievedTask);
                

                Frame.Navigate(typeof(TasksPage));
            }
        }

        private void ButtonGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private void ButtonDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Task.listOfTasks.Remove(recievedTask);
            DataModelV2.DeleteTask(recievedTask);
            Frame.Navigate(typeof(TasksPage));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
