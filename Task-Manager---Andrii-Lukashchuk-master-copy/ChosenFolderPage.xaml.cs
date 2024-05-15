using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChosenFolderPage : Page
    {
        Folder recievedFolder;
        public ObservableCollection<Task> tasks = new ObservableCollection<Task>();
        private DispatcherTimer timer;

        private void StartTimer()
        {
            currentTimeTextBlock.Text = DateTime.Now.ToString("HH:mm");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            currentTimeTextBlock.Text = DateTime.Now.ToString("HH:mm");
        }

        public ChosenFolderPage()
        {
            this.InitializeComponent();
            StartTimer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            recievedFolder = e.Parameter as Folder;

            for (int i = 0; i < Task.listOfTasks.Count; i++)
            {
                foreach (Folder folder in Folder.listOfFolders)
                {
                    if (Task.listOfTasks[i].folderGuid == folder.folderGuid)
                    {
                        tasks.Add(Task.listOfTasks[i]);
                    }
                }
            }

            if (tasks.Count > 0)
            {
                listTaskView.ItemsSource = tasks;
            }
        }

        private async void ButtonRemoveFolder_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Warning",
                Content = new TextBlock { Text = "Are you sure you want to delete this folder?" },
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
            };
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                foreach(Task task in Task.listOfTasks)
                {
                    if (task.taskGuid == recievedFolder.folderGuid)
                    {
                        task.taskGuid = Guid.Empty;
                    }
                }

                Folder.listOfFolders.Remove(recievedFolder);
                DataModelV2.DeleteFolder(recievedFolder);

                Frame.Navigate(typeof(MainPage));
            }
        }

        private void folderPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void taskPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private void ButtonSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate (typeof(SettingsPage));
        }
    }
}
