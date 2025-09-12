using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace Vms_page
{
    public partial class VcaWindow : Window
    {
        private string currentTab = "Retail Monitoring";

        public VcaWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            
            // Set the initial active tab
            SetActiveTab("Retail Monitoring");
        }

        private void SetActiveTab(string tabName)
        {
            // Reset all buttons to inactive style
            RetailMonitoringBtn.Foreground = (SolidColorBrush)FindResource("TextSecondaryColor");
            RetailMonitoringBtn.FontWeight = FontWeights.Normal;
            
            BehaviorSearchBtn.Foreground = (SolidColorBrush)FindResource("TextSecondaryColor");
            BehaviorSearchBtn.FontWeight = FontWeights.Normal;
            
            StatisticsBtn.Foreground = (SolidColorBrush)FindResource("TextSecondaryColor");
            StatisticsBtn.FontWeight = FontWeights.Normal;

            // Set the active button style
            switch (tabName)
            {
                case "Realtime Monitoring":
                    RetailMonitoringBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    RetailMonitoringBtn.FontWeight = FontWeights.SemiBold;
                    break;
                case "Behaviour Research":
                    BehaviorSearchBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    BehaviorSearchBtn.FontWeight = FontWeights.SemiBold;
                    break;
                case "Heat Map":
                    StatisticsBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    StatisticsBtn.FontWeight = FontWeights.SemiBold;
                    break;
            }

            currentTab = tabName;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string tabName = button.Content.ToString();
                SetActiveTab(tabName);
                
                // Switch views based on selected tab
                SwitchView(tabName);
            }
        }

        private void SwitchView(string tabName)
        {
            // Hide all views first
            RetailMonitoringView.Visibility = Visibility.Collapsed;
            BehaviorSearchView.Visibility = Visibility.Collapsed;
            StatisticsView.Visibility = Visibility.Collapsed;

            // Show the selected view and adjust layout
            switch (tabName)
            {
                case "Realtime Monitoring":
                    RetailMonitoringView.Visibility = Visibility.Visible;
                    // Show right sidebar and bottom bar for realtime monitoring
                    ShowRightSidebarAndBottomBar(true);
                    break;
                case "Behaviour Research":
                    BehaviorSearchView.Visibility = Visibility.Visible;
                    // Hide right sidebar and bottom bar for behaviour research
                    ShowRightSidebarAndBottomBar(false);
                    break;
                case "Heat Map":
                    StatisticsView.Visibility = Visibility.Visible;
                    // Show right sidebar and bottom bar for heat map
                    ShowRightSidebarAndBottomBar(true);
                    break;
            }
        }

        private void ShowRightSidebarAndBottomBar(bool show)
        {
            // Find the bottom bar element
            var bottomBar = this.FindName("BottomBar") as Border;
            
            if (bottomBar != null)
            {
                bottomBar.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }

            // Main content grid now always spans 1 column since right sidebar is removed
            Grid.SetColumnSpan(MainContentGrid, 1);
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter Keywords")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = (SolidColorBrush)FindResource("TextPrimaryColor");
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter Keywords";
                SearchTextBox.Foreground = (SolidColorBrush)FindResource("TextSecondaryColor");
            }
        }

        private void VideoFeed_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Border border)
            {
                // Toggle the border color for the single camera feed
                if (border.BorderBrush.ToString() == "#FF9500")
                {
                    // If currently selected (orange), deselect it
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
                }
                else
                {
                    // If not selected, select it (orange)
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SingleView_Click(object sender, RoutedEventArgs e)
        {
            // Show single camera view, hide 4-camera grid
            SingleCameraView.Visibility = Visibility.Visible;
            FourCameraView.Visibility = Visibility.Collapsed;
        }

        private void GridView_Click(object sender, RoutedEventArgs e)
        {
            // Show 4-camera grid, hide single camera view
            SingleCameraView.Visibility = Visibility.Collapsed;
            FourCameraView.Visibility = Visibility.Visible;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement pause functionality
            MessageBox.Show("Pause functionality will be implemented here.", "Pause", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        // Statistics Event Handlers
        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            StatisticsDatePicker.SelectedDate = DateTime.Today;
        }

        private void StatisticsReset_Click(object sender, RoutedEventArgs e)
        {
            // Reset all statistics filters
            StatisticsChannelComboBox.SelectedIndex = 0; // "All Channels"
            StatisticsTimeComboBox.SelectedIndex = 0; // "By Day"
            StatisticsDatePicker.SelectedDate = DateTime.Today;
        }

        private void StatisticsCount_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement statistics count functionality
            MessageBox.Show("Statistics count functionality will be implemented here.", "Statistics Count", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StatisticsExport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement statistics export functionality
            MessageBox.Show("Statistics export functionality will be implemented here.", "Statistics Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Behavior Search Event Handlers
        private void QuickTime_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var today = DateTime.Today;
                switch (button.Content.ToString())
                {
                    case "Today":
                        StartTimePicker.SelectedDate = today;
                        EndTimePicker.SelectedDate = today;
                        break;
                    case "Last 3 days":
                        StartTimePicker.SelectedDate = today.AddDays(-3);
                        EndTimePicker.SelectedDate = today;
                        break;
                    case "Last 7 days":
                        StartTimePicker.SelectedDate = today.AddDays(-7);
                        EndTimePicker.SelectedDate = today;
                        break;
                }
            }
        }

        private void ObjectType_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Reset all object type buttons to unselected state (exact colors from image)
                AllObjectBtn.Background = new SolidColorBrush(Colors.Transparent);
                AllObjectBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                AllObjectBtn.BorderThickness = new Thickness(0);
                
                PedestrianBtn.Background = new SolidColorBrush(Colors.Transparent);
                PedestrianBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                PedestrianBtn.BorderThickness = new Thickness(0);
                
                NonMotorVehicleBtn.Background = new SolidColorBrush(Colors.Transparent);
                NonMotorVehicleBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                NonMotorVehicleBtn.BorderThickness = new Thickness(0);
                
                MotorVehicleBtn.Background = new SolidColorBrush(Colors.Transparent);
                MotorVehicleBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                MotorVehicleBtn.BorderThickness = new Thickness(0);

                // Set selected button (exact blue color from image)
                button.Background = new SolidColorBrush(Color.FromRgb(0x00, 0x78, 0xD4));
                button.Foreground = new SolidColorBrush(Colors.White);
                button.BorderThickness = new Thickness(0);
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export functionality will be implemented here.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Search functionality will be implemented here.", "Search", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            // Reset all filters
            AlarmTypeComboBox.SelectedIndex = 0; // "All"
            StartTimePicker.SelectedDate = DateTime.Today;
            EndTimePicker.SelectedDate = DateTime.Today;
            
            // Reset object type selection
            ObjectType_Click(AllObjectBtn, e);
        }

        private void ViewToggle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Name == "GridViewBtn")
                {
                    // Grid view selected (exact colors from image)
                    GridViewBtn.Background = new SolidColorBrush(Color.FromRgb(0x4A, 0x4A, 0x4A));
                    GridViewBtn.Foreground = new SolidColorBrush(Colors.White);
                    GridViewBtn.BorderThickness = new Thickness(0);
                    
                    ListViewBtn.Background = new SolidColorBrush(Colors.Transparent);
                    ListViewBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                    ListViewBtn.BorderThickness = new Thickness(0);
                }
                else
                {
                    // List view selected
                    ListViewBtn.Background = new SolidColorBrush(Color.FromRgb(0x4A, 0x4A, 0x4A));
                    ListViewBtn.Foreground = new SolidColorBrush(Colors.White);
                    ListViewBtn.BorderThickness = new Thickness(0);
                    
                    GridViewBtn.Background = new SolidColorBrush(Colors.Transparent);
                    GridViewBtn.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
                    GridViewBtn.BorderThickness = new Thickness(0);
                }
            }
        }

    }
}
