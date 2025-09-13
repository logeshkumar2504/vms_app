using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vms_page
{
    public partial class AlarmRecordsWindow : Window
    {
        public ObservableCollection<AlarmRecord> AlarmRecords { get; set; }

        public AlarmRecordsWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            var currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);
            
            // Initialize data
            InitializeAlarmData();
            
            // Set data context
            DataContext = this;
            
            // Data grid removed - no longer needed
        }

        private void InitializeAlarmData()
        {
            AlarmRecords = new ObservableCollection<AlarmRecord>();
            
            // Add sample alarm data for demonstration
            var sampleAlarms = new[]
            {
                new AlarmRecord
                {
                    AlarmTime = DateTime.Now.AddMinutes(-5).ToString("HH:mm:ss"),
                    AlarmSource = "Camera 01 - Main Entrance",
                    AlarmType = "Motion Detection",
                    AlarmLevel = "Level 1",
                    Preview = "View"
                },
                new AlarmRecord
                {
                    AlarmTime = DateTime.Now.AddMinutes(-12).ToString("HH:mm:ss"),
                    AlarmSource = "Camera 03 - Parking Lot",
                    AlarmType = "Line Crossing",
                    AlarmLevel = "Level 2",
                    Preview = "View"
                },
                new AlarmRecord
                {
                    AlarmTime = DateTime.Now.AddMinutes(-18).ToString("HH:mm:ss"),
                    AlarmSource = "Camera 07 - Loading Dock",
                    AlarmType = "Tampering",
                    AlarmLevel = "Level 3",
                    Preview = "View"
                },
                new AlarmRecord
                {
                    AlarmTime = DateTime.Now.AddMinutes(-25).ToString("HH:mm:ss"),
                    AlarmSource = "Camera 12 - Office Area",
                    AlarmType = "Audio Detection",
                    AlarmLevel = "Level 4",
                    Preview = "View"
                },
                new AlarmRecord
                {
                    AlarmTime = DateTime.Now.AddMinutes(-32).ToString("HH:mm:ss"),
                    AlarmSource = "Camera 05 - Reception",
                    AlarmType = "Intrusion",
                    AlarmLevel = "Level 5",
                    Preview = "View"
                }
            };

            foreach (var alarm in sampleAlarms)
            {
                AlarmRecords.Add(alarm);
            }
        }

        // Navbar button click handlers
        private void LatestAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to Latest Alarm tab
            LatestAlarmButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
            HistoryAlarmButton.Style = (Style)FindResource("NavbarButtonStyle");
            
            // Load latest alarm content
            InitializeAlarmData();
        }

        private void HistoryAlarmButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to History Alarm tab
            HistoryAlarmButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
            LatestAlarmButton.Style = (Style)FindResource("NavbarButtonStyle");
            
            // Load history alarm content (same as latest alarm for now)
            InitializeAlarmData();
        }
    }

    // Alarm Record model
    public class AlarmRecord
    {
        public string AlarmTime { get; set; }
        public string AlarmSource { get; set; }
        public string AlarmType { get; set; }
        public string AlarmLevel { get; set; }
        public string Preview { get; set; }
    }
}