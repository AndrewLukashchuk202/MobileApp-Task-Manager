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
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TasksPage : Page
    {
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

        public TasksPage()
        {
            this.InitializeComponent();
            StartTimer();
            UpdatePreferences();
            LoadAllTasksFromSqlDatabase();
            DisplayTasks();
        }

        private void UpdatePreferences()
        {
            //cadet blue
            taskPageButton.Background = new SolidColorBrush(Color.FromArgb(255,95,158,160));
            //deep pink color
            folderPageButton.Background = new SolidColorBrush(Color.FromArgb(255,255, 255, 255));
        }

        private void LoadAllTasksFromSqlDatabase()
        {
            DataModelV2.ReadTaskData();

            List<Task> taskList = new List<Task>();

            if (Task.listOfTasks.Count != 0)
            {
                //sorting tasks but its due date/ overdue tasks listed first
                Task.listOfTasks.Sort();
                taskList = Task.listOfTasks;
            }

            if (taskList.Count > 0)
            {
                for (int i = 0; i < taskList.Count; i++)
                {
                    tasks.Add(taskList[i]);
                }
            }
        }

        private void DisplayTasks()
        {
            if (tasks.Count > 0)
            {   

                listTaskView.ItemsSource = tasks;
            }
        }

        private void folderPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void ButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTaskPage));
        }

        private void listTaskView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task task = e.ClickedItem as Task;
            Frame.Navigate(typeof(ChosenTaskPage), task);
        }

        private void ButtonSettingPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
    }

    // DataTemplateSelector to display image with pencil icon only if task object has notes attached
    public class TaskNotesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WithNotesTemplate { get; set; }
        public DataTemplate WithoutNotesTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            if (item is Task task && !string.IsNullOrEmpty(task.taskNotes))
            {
                return WithNotesTemplate;
            }
            else
            {
                return WithoutNotesTemplate;
            }
        }
    }

    
    public class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd/MM/yyyy");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TaskCompletedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CompletedTemplate { get; set; }
        public DataTemplate IncompleteTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            if (item is Task task && task.taskCompleted)
            {
                return CompletedTemplate;
            }
            else
            {
                return IncompleteTemplate;
            }
        }
    }


}
