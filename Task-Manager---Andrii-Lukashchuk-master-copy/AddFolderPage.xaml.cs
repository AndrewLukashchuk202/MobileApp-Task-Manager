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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Manager___Andrii_Lukashchuk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddFolderPage : Page
    {
        public AddFolderPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(folderNameTextBox.Text))
            {
                Frame.Navigate(typeof(MainPage));
                return;
            }

            if (!FolderExists(folderNameTextBox.Text))
            {
                Folder newFolder = new Folder(folderNameTextBox.Text);
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Warning",
                    Content = new TextBlock { Text = "Folder with this name already exists" },
                    PrimaryButtonText = "OK",
                };
                await dialog.ShowAsync();
                folderNameTextBox.Text = "";
            }
        }
        private bool FolderExists(string folderName)
        {
            foreach (var folder in Folder.listOfFolders)
            {
                if (folder.folderName == folderName)
                {
                    return true;
                }
            }
            return false;
        }

        private void ButtonGoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
