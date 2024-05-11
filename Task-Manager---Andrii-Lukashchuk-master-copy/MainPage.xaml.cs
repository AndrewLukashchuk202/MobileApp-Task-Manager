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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Data.Sqlite;
using static Task_Manager___Andrii_Lukashchuk.MainPage;



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

        public MainPage()
        {
            this.InitializeComponent();

            // Example tasks:
            // Finished task
            Task eating = new Task("To eat 2 bananas", "They are delicious", true, new DateTime(2024, 3, 6, 15, 0, 0));
            Task gym = new Task("To go to the gym", "Who is gonna carry the boats and a logs", true, new DateTime(2023, 3, 6, 15, 0, 0));
            // Non-finished tasks
            Task cleanDishes = new Task("To clean dishes", "Don't be lazy", false, new DateTime(2025, 4, 6, 15, 0, 0));
            Task playGame = new Task("To play game with non-realistic friends", "They aren't real", false, new DateTime(2024, 9, 9, 11, 2, 3));

            DataModelV2 sql = new DataModelV2();
            sql.ReadTaskData();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FolderPage));
        }
    }
}