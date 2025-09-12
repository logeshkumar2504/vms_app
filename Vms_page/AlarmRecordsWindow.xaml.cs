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
        private bool isSoundEnabled = true;

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
            
            // Manually set the ItemsSource to ensure binding works
            AlarmRecordsDataGrid.ItemsSource = AlarmRecords;
        }

        private void InitializeAlarmData()
        {
            AlarmRecords = new ObservableCollection<AlarmRecord>();
        }

        // Tab switching
        private void LatestAlarmTab_Click(object sender, RoutedEventArgs e)
        {
            LatestAlarmTab.Style = (Style)FindResource("SelectedTabButtonStyle");
            HistoryAlarmTab.Style = (Style)FindResource("TabButtonStyle");
            
            // Load latest alarm data
            InitializeAlarmData();
            AlarmRecordsDataGrid.ItemsSource = AlarmRecords;
        }

        private void HistoryAlarmTab_Click(object sender, RoutedEventArgs e)
        {
            HistoryAlarmTab.Style = (Style)FindResource("SelectedTabButtonStyle");
            LatestAlarmTab.Style = (Style)FindResource("TabButtonStyle");
            
            // Load historical alarm data
            LoadHistoryAlarmData();
            AlarmRecordsDataGrid.ItemsSource = AlarmRecords;
        }

        private void LoadHistoryAlarmData()
        {
            // Clear historical data - keep table empty
            AlarmRecords.Clear();
        }

        private string GetRandomAlarmType(int index)
        {
            string[] types = { "Motion Detection", "Line Crossing", "Audio Detection", "Tampering", "Intrusion" };
            return types[index % types.Length];
        }

        private SolidColorBrush GetLevelColor(int level)
        {
            return level switch
            {
                1 => new SolidColorBrush(Color.FromRgb(255, 68, 68)),
                2 => new SolidColorBrush(Color.FromRgb(255, 136, 0)),
                3 => new SolidColorBrush(Color.FromRgb(255, 221, 0)),
                4 => new SolidColorBrush(Color.FromRgb(68, 170, 255)),
                5 => new SolidColorBrush(Color.FromRgb(0, 68, 170)),
                _ => new SolidColorBrush(Color.FromRgb(128, 128, 128))
            };
        }

        // Filter controls
        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Level1CheckBox != null) Level1CheckBox.IsChecked = true;
            if (Level2CheckBox != null) Level2CheckBox.IsChecked = true;
            if (Level3CheckBox != null) Level3CheckBox.IsChecked = true;
            if (Level4CheckBox != null) Level4CheckBox.IsChecked = true;
            if (Level5CheckBox != null) Level5CheckBox.IsChecked = true;
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Level1CheckBox != null) Level1CheckBox.IsChecked = false;
            if (Level2CheckBox != null) Level2CheckBox.IsChecked = false;
            if (Level3CheckBox != null) Level3CheckBox.IsChecked = false;
            if (Level4CheckBox != null) Level4CheckBox.IsChecked = false;
            if (Level5CheckBox != null) Level5CheckBox.IsChecked = false;
        }

        // Ellipsis button functionality
        private void EllipsisButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Additional alarm type filter options would be shown here.", 
                          "Filter Options", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Action buttons
        private void AcknowledgeButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedCount = 0;
            foreach (var record in AlarmRecords)
            {
                if (record.IsSelected)
                    selectedCount++;
            }

            if (selectedCount > 0)
            {
                MessageBox.Show($"Acknowledged {selectedCount} alarm(s).", "Acknowledge", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select alarms to acknowledge.", "No Selection", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SoundToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isSoundEnabled = !isSoundEnabled;
            SoundToggleButton.Content = isSoundEnabled ? "🔊" : "🔇";
            SoundToggleButton.ToolTip = isSoundEnabled ? "Turn Off Sound" : "Turn On Sound";
            
            MessageBox.Show($"Alarm sound {(isSoundEnabled ? "enabled" : "disabled")}.", "Sound Toggle", 
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // DataGrid events
        private void SelectAllHeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var record in AlarmRecords)
            {
                record.IsSelected = true;
            }
            AlarmRecordsDataGrid.Items.Refresh();
        }

        private void SelectAllHeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var record in AlarmRecords)
            {
                record.IsSelected = false;
            }
            AlarmRecordsDataGrid.Items.Refresh();
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is AlarmRecord record)
            {
                MessageBox.Show($"Preview for alarm:\n\nTime: {record.AlarmTime}\nSource: {record.AlarmSource}\nType: {record.AlarmType}\nLevel: {record.AlarmLevel}", 
                              "Alarm Preview", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    // Alarm Record model
    public class AlarmRecord
    {
        public string AlarmTime { get; set; }
        public string AlarmSource { get; set; }
        public string AlarmType { get; set; }
        public string AlarmLevel { get; set; }
        public SolidColorBrush LevelColor { get; set; }
        public bool IsSelected { get; set; }
    }
}