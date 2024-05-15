using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI;
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
    public sealed partial class SettingsPage : Page
    {
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

        public SettingsPage()
        {
            this.InitializeComponent();
            StartTimer();
            UpdatePreferences();
        }

        private void folderPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void taskPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TasksPage));
        }

        public void UpdatePreferences()
        {
            string backgroundColor = AppSettings.LoadSetting("BackgroundColor", "lightCoral");

            Color lightCoralColor = Colors.LightCoral;
            Color rosyBrownColor = Colors.RosyBrown;
            Color lightSeaGreenColor = Colors.LightSeaGreen;
            Color bisqueColor = Colors.Bisque;
            Color whiteColor = Colors.White;

            Grid myGrid = GetGrid();

            if (backgroundColor == "lightCoral")
            {
                myGrid.Background = new SolidColorBrush(lightCoralColor);
            }
            else if (backgroundColor == "rosyBrown")
            {
                myGrid.Background = new SolidColorBrush(rosyBrownColor);
            }
            else if (backgroundColor == "lightSeaGreen")
            {
                myGrid.Background = new SolidColorBrush(lightSeaGreenColor);
            }
            else if (backgroundColor == "bisque")
            {
                myGrid.Background = new SolidColorBrush(bisqueColor);
            }
            else if (backgroundColor == "white")
            {
                myGrid.Background = new SolidColorBrush(whiteColor);
            }

        }

        public Grid GetGrid()
        {
            /*Grid rootGrid = new Grid();
            FrameworkElement rootElement = Window.Current.Content as FrameworkElement;

            if (rootElement != null && rootElement is Frame)
            {
                // Root is a Frame, so get its Content (which might be a Page)
                Frame rootFrame = rootElement as Frame;
                Page rootPage = rootFrame.Content as Page;

                if (rootPage != null)
                {
                    // Page is found, try to get its Content (which might be a Grid)
                    FrameworkElement pageContent = rootPage.Content as FrameworkElement;

                    if (pageContent != null && pageContent is Grid)
                    {
                        // Content is a Grid, so set its Background
                        rootGrid = pageContent as Grid;

                        return rootGrid;
                    }

                }
            }
            return rootGrid;*/

            Grid rootGrid = new Grid();

            // Get the current page content
            FrameworkElement rootElement = Window.Current.Content as FrameworkElement;

            if (rootElement != null && rootElement is Grid)
            {
                // Replace the root Grid if the current content is a Grid
                rootGrid = rootElement as Grid;
            }

            return rootGrid;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                FrameworkElement rootElement = Window.Current.Content as FrameworkElement;
                string buttonName = clickedButton.Name;

                if (rootElement != null && rootElement is Frame)
                {
                    // Root is a Frame, so get its Content (which might be a Page)
                    Frame rootFrame = rootElement as Frame;
                    Page rootPage = rootFrame.Content as Page;

                    if (rootPage != null)
                    {
                        // Page is found, try to get its Content (which might be a Grid)
                        FrameworkElement pageContent = rootPage.Content as FrameworkElement;

                        if (pageContent != null && pageContent is Grid)
                        {
                            // Content is a Grid, so set its Background
                            Grid rootGrid = pageContent as Grid;

                            switch (buttonName)
                            {
                                case "lightCoralButton":
                                    rootGrid.Background = new SolidColorBrush(Colors.LightCoral);
                                    AppSettings.SaveSetting("BackgroundColor", "lightCoral");
                                    break;

                                case "rosyBrownButton":
                                    rootGrid.Background = new SolidColorBrush(Colors.RosyBrown);
                                    AppSettings.SaveSetting("BackgroundColor", "rosyBrown");
                                    break;

                                case "lightSeaGreenButton":
                                    rootGrid.Background = new SolidColorBrush(Colors.LightSeaGreen);
                                    AppSettings.SaveSetting("BackgroundColor", "lightSeaGreen");
                                    break;

                                case "bisqueButton":
                                    rootGrid.Background = new SolidColorBrush(Colors.Bisque);
                                    AppSettings.SaveSetting("BackgroundColor", "bisque");
                                    break;
                                case "whiteButton":
                                    rootGrid.Background = new SolidColorBrush(Colors.White);
                                    AppSettings.SaveSetting("BackgroundColor", "white");
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}

