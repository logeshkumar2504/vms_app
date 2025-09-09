using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            
            HeatMapBtn.Foreground = (SolidColorBrush)FindResource("TextSecondaryColor");
            HeatMapBtn.FontWeight = FontWeights.Normal;

            // Set the active button style
            switch (tabName)
            {
                case "Retail Monitoring":
                    RetailMonitoringBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    RetailMonitoringBtn.FontWeight = FontWeights.SemiBold;
                    break;
                case "Behavior Search":
                    BehaviorSearchBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    BehaviorSearchBtn.FontWeight = FontWeights.SemiBold;
                    break;
                case "Heat Map":
                    HeatMapBtn.Foreground = (SolidColorBrush)FindResource("PrimaryColor");
                    HeatMapBtn.FontWeight = FontWeights.SemiBold;
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
            HeatMapView.Visibility = Visibility.Collapsed;

            // Show the selected view
            switch (tabName)
            {
                case "Retail Monitoring":
                    RetailMonitoringView.Visibility = Visibility.Visible;
                    break;
                case "Behavior Search":
                    BehaviorSearchView.Visibility = Visibility.Visible;
                    break;
                case "Heat Map":
                    HeatMapView.Visibility = Visibility.Visible;
                    break;
            }
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

    }
}
