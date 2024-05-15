using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using Microsoft.Data.Sqlite;
using static Task_Manager___Andrii_Lukashchuk.MainPage;
using System.Collections.ObjectModel;
using Windows.Storage;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Comment 1: Verify the proper instantiation of objects.
        // Comment 2: Validate task completion status to determine if it's overdue.
        // Comment 3: Ensure correct handling of repeating tasks by updating their due dates upon completion.
        // Comment 4: Assess the functionality of the Habit class by tracking repeated task executions.
        // Comment 5: Confirm the correct creation of folder objects.
        // Comment 6: Evaluate the accuracy of methods, including calculating the total number of incomplete tasks.

        public ObservableCollection<Folder> folders = new ObservableCollection<Folder>();

        private DispatcherTimer timer;

        //Function that updates time every second
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

        public MainPage()
        {
            this.InitializeComponent();
            //Load sql database only once
            DataModelV2.InitialiseDatabase();
            DataModelV2.ReadTaskData();
            LoadAllFoldersFromSqlDatabase();
            DisplayFolders();
            StartTimer();
            UpdatePreferences();
        }

        private void ButtonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddFolderPage));
        }

        private void LoadAllFoldersFromSqlDatabase()
        {
            DataModelV2.ReadFolderData();

            List<Folder> folderList = new List<Folder>();

            if (Folder.listOfFolders.Count != 0)
            {
                folderList = Folder.listOfFolders;
            }

            if (folderList.Count > 0)
            {
                for (int i = 0; i < folderList.Count; i++)
                {
                    folders.Add(folderList[i]);
                }
            }
        }

        private void DisplayFolders()
        {
            if (folders.Count > 0)
            {
                listFolderView.ItemsSource = folders;
            }
        }

        private void ButtonOpenTaskPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        private void UpdatePreferences()
        {
            //blue violet color
            taskPageButton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //deep pink color
            folderPageButton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 147));
        }

        private void listFolderView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Folder folder = e.ClickedItem as Folder;
            Frame.Navigate(typeof(ChosenFolderPage), folder);
        }

        private void ButtonSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }

    public class IntegerGreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int intValue && intValue > 0)
            {
                return intValue.ToString();
            }
            return ""; // Return null if the value is not greater than zero
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}