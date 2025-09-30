using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;

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
            // Ensure only the default placeholder is visible on startup
            HideAllPanels();
            DefaultPlaceholder.Visibility = Visibility.Visible;
        }

        private void HideAllPanels()
        {
            // Hide all configuration panels
            DefaultPlaceholder.Visibility = Visibility.Collapsed;
            SnapshotPanel.Visibility = Visibility.Collapsed;
            PosOsdConfigPanel.Visibility = Visibility.Collapsed;
            VideoPanel.Visibility = Visibility.Collapsed;
            RecordingPanel.Visibility = Visibility.Collapsed;
            StartupPanel.Visibility = Visibility.Collapsed;
            LogPanel.Visibility = Visibility.Collapsed;
            MaintenancePanel.Visibility = Visibility.Collapsed;
            ServicePanel.Visibility = Visibility.Collapsed;
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
            HideAllPanels();
            VideoPanel.Visibility = Visibility.Visible;
            SetActiveButton(AudioVideoButton);
        }

        private void Snapshot_Click(object sender, RoutedEventArgs e)
        {
            // Show Snapshot configuration panel in the right content area
            HideAllPanels();
            SnapshotPanel.Visibility = Visibility.Visible;
            SetActiveButton(AudioVideoButton);
        }

        private void Recording_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            RecordingPanel.Visibility = Visibility.Visible;
            SetActiveButton(AudioVideoButton);
        }

        private void PosOsdConfig_Click(object sender, RoutedEventArgs e)
        {
            // Show POS OSD Configuration panel
            HideAllPanels();
            PosOsdConfigPanel.Visibility = Visibility.Visible;
            SetActiveButton(AudioVideoButton);
        }

        // System sub-option click handlers
        private void Startup_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            StartupPanel.Visibility = Visibility.Visible;
            SetActiveButton(SystemButton);
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            LogPanel.Visibility = Visibility.Visible;
            SetActiveButton(SystemButton);
        }

        private void Maintenance_Click(object sender, RoutedEventArgs e)
        {
            HideAllPanels();
            MaintenancePanel.Visibility = Visibility.Visible;
            SetActiveButton(SystemButton);
        }

        private void ImportConfig_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Import Configuration File",
                Filter = "Configuration Files (*.cfg;*.xml;*.json)|*.cfg;*.xml;*.json|All Files (*.*)|*.*",
                CheckFileExists = true,
                CheckPathExists = true
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Configuration imported from:\n{dialog.FileName}", 
                              "Import Successful", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ExportConfig_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Export Configuration File",
                Filter = "Configuration Files (*.cfg)|*.cfg|XML Files (*.xml)|*.xml|JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                DefaultExt = ".cfg",
                FileName = "system_config.cfg"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Configuration exported to:\n{dialog.FileName}", 
                              "Export Successful", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
        }

        private void ExportMaintenanceInfo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Export Maintenance Information",
                Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All Files (*.*)|*.*",
                DefaultExt = ".txt",
                FileName = "maintenance_info.txt"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                MessageBox.Show($"Maintenance information exported to:\n{dialog.FileName}", 
                              "Export Successful", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Information);
            }
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
            HideAllPanels();
            ServicePanel.Visibility = Visibility.Visible;
            SetActiveButton(OperationButton);
        }

        private void EnableAutoTimeSync_Changed(object sender, RoutedEventArgs e)
        {
            // Update the visual appearance of the interval section based on checkbox state
            if (IntervalBorder != null)
            {
                IntervalBorder.Opacity = EnableAutoTimeSyncCheckBox.IsChecked == true ? 1.0 : 0.5;
            }
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

        private void DurationMode_Changed(object sender, RoutedEventArgs e)
        {
            if (DurationSecondsTextBox != null && DurationCustomRadio != null)
            {
                DurationSecondsTextBox.IsEnabled = DurationCustomRadio.IsChecked == true;
            }
        }

        private void FontColorSwatch_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FontColorPopup.IsOpen = true;
        }

        private void FontColorOption_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border colorCell && colorCell.Tag is string hex)
            {
                try
                {
                    var brush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(hex);
                    FontColorSwatch.Background = brush;
                }
                catch
                {
                    // ignore invalid
                }
            }
            FontColorPopup.IsOpen = false;
        }

        private void BrowseSnapshotPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Folder",
                CheckFileExists = false,
                CheckPathExists = true,
                ValidateNames = false,
                FileName = "Select Folder"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                var selectedDirectory = Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrWhiteSpace(selectedDirectory))
                {
                    ImageSavePathTextBox.Text = selectedDirectory + (selectedDirectory.EndsWith("\\") ? string.Empty : "\\");
                }
            }
        }

        private void BrowseDownloadedRecordingPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Folder",
                CheckFileExists = false,
                CheckPathExists = true,
                ValidateNames = false,
                FileName = "Select Folder"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                var selectedDirectory = Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrWhiteSpace(selectedDirectory))
                {
                    DownloadedRecordingPathTextBox.Text = selectedDirectory + (selectedDirectory.EndsWith("\\") ? string.Empty : "\\");
                }
            }
        }

        private void BrowseLocalRecordingPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Folder",
                CheckFileExists = false,
                CheckPathExists = true,
                ValidateNames = false,
                FileName = "Select Folder"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                var selectedDirectory = Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrWhiteSpace(selectedDirectory))
                {
                    LocalRecordingPathTextBox.Text = selectedDirectory + (selectedDirectory.EndsWith("\\") ? string.Empty : "\\");
                }
            }
        }
    }
}
