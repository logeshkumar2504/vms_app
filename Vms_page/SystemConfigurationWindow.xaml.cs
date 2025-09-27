using System.Windows;
using System.Windows.Controls;

namespace Vms_page
{
    public partial class SystemConfigurationWindow : Window
    {
        private bool isAudioVideoExpanded = true;
        private bool isSystemExpanded = false;
        private bool isOperationExpanded = false;

        public SystemConfigurationWindow()
        {
            InitializeComponent();
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all settings to default values
            MessageBox.Show("Settings have been reset to default values.", 
                          "Default Settings", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Apply changes and close the window
            MessageBox.Show("Settings have been saved successfully.", 
                          "Settings Saved", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without saving changes
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Apply changes without closing the window
            MessageBox.Show("Settings have been applied successfully.", 
                          "Settings Applied", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void AudioVideo_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the Audio & Video dropdown
            isAudioVideoExpanded = !isAudioVideoExpanded;
            AudioVideoSubOptions.Visibility = isAudioVideoExpanded ? Visibility.Visible : Visibility.Collapsed;
            
            // Update the chevron icon
            var button = sender as Button;
            var template = button?.Template;
            if (template != null)
            {
                var chevronIcon = template.FindName("ChevronIcon", button) as TextBlock;
                if (chevronIcon != null)
                {
                    chevronIcon.Text = isAudioVideoExpanded ? "▼" : "▶";
                }
            }
        }

        private void System_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the System dropdown
            isSystemExpanded = !isSystemExpanded;
            SystemSubOptions.Visibility = isSystemExpanded ? Visibility.Visible : Visibility.Collapsed;
            
            // Update the chevron icon
            var button = sender as Button;
            var template = button?.Template;
            if (template != null)
            {
                var chevronIcon = template.FindName("ChevronIcon", button) as TextBlock;
                if (chevronIcon != null)
                {
                    chevronIcon.Text = isSystemExpanded ? "▼" : "▶";
                }
            }
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the Operation dropdown
            isOperationExpanded = !isOperationExpanded;
            OperationSubOptions.Visibility = isOperationExpanded ? Visibility.Visible : Visibility.Collapsed;
            
            // Update the chevron icon
            var button = sender as Button;
            var template = button?.Template;
            if (template != null)
            {
                var chevronIcon = template.FindName("ChevronIcon", button) as TextBlock;
                if (chevronIcon != null)
                {
                    chevronIcon.Text = isOperationExpanded ? "▼" : "▶";
                }
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all buttons to normal style
            AudioVideoButton.Style = (Style)FindResource("DropdownSidebarButtonStyle");
            SystemButton.Style = (Style)FindResource("DropdownSidebarButtonStyle");
            OperationButton.Style = (Style)FindResource("DropdownSidebarButtonStyle");
            
            // Set the clicked button to active style
            activeButton.Style = (Style)FindResource("ActiveSidebarButtonStyle");
        }

        // Sub-option click handlers
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Video configuration panel would open here.", 
                          "Video", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Snapshot_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Snapshot configuration panel would open here.", 
                          "Snapshot", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Recording_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Recording configuration panel would open here.", 
                          "Recording", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void PosOsdConfig_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("POS OSD Configuration panel would open here.", 
                          "POS OSD Configuration", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        // System sub-option click handlers
        private void Startup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("System Startup configuration panel would open here.", 
                          "System Startup", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("System Log configuration panel would open here.", 
                          "System Log", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("System Maintenance configuration panel would open here.", 
                          "System Maintenance", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        // Operation sub-option click handlers
        private void Alarm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation Alarm configuration panel would open here.", 
                          "Operation Alarm", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Service_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation Service configuration panel would open here.", 
                          "Operation Service", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation Email configuration panel would open here.", 
                          "Operation Email", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void EpidemicControl_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation Epidemic Control configuration panel would open here.", 
                          "Operation Epidemic Control", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void AttributeDisplay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation Attribute Display configuration panel would open here.", 
                          "Operation Attribute Display", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }
    }
}
