using System.Collections.ObjectModel;
using System.Windows;

namespace Vms_page
{
    public partial class DeviceManagementWindow : Window
    {
        public ObservableCollection<Device> Devices { get; set; }
        public ObservableCollection<OnlineDevice> OnlineDevices { get; set; }

        public DeviceManagementWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            SetActiveTab("Camera Alarm");
            SetActiveDeviceType("Encoding Device");
            InitializeDeviceList();
            InitializeOnlineDeviceList();
        }

        private void InitializeDeviceList()
        {
            // Initialize empty device list
            Devices = new ObservableCollection<Device>();
            EncodingDeviceGrid.ItemsSource = Devices;
            UpdateDeviceCount();
        }

        private void InitializeOnlineDeviceList()
        {
            // Initialize empty online device list
            OnlineDevices = new ObservableCollection<OnlineDevice>();
            OnlineDeviceGrid.ItemsSource = OnlineDevices;
            UpdateOnlineDeviceCount();
        }

        private void UpdateDeviceCount()
        {
            if (Devices != null)
            {
                EncodingDeviceCount.Text = $"Managed Device({Devices.Count})";
            }
        }

        private void UpdateOnlineDeviceCount()
        {
            if (OnlineDevices != null)
            {
                OnlineDeviceCount.Text = $"Online Device({OnlineDevices.Count})";
            }
        }

        private void SetActiveTab(string tab)
        {
            // Button visual state via Tag for template trigger
            CameraAlarmBtn.Tag = tab == "Camera Alarm" ? "Active" : null;
            DeviceAlarmBtn.Tag = tab == "Device Alarm" ? "Active" : null;

            // Content visibility
            CameraAlarmView.Visibility = tab == "Camera Alarm" ? Visibility.Visible : Visibility.Collapsed;
            DeviceAlarmView.Visibility = tab == "Device Alarm" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetActiveDeviceType(string deviceType)
        {
            // Update sidebar button states
            EncodingDeviceBtn.Tag = deviceType == "Encoding Device" ? "Selected" : null;
            DecodingDeviceBtn.Tag = deviceType == "Decoding Device" ? "Selected" : null;
            CloudDeviceBtn.Tag = deviceType == "Cloud Device" ? "Selected" : null;
            AccessControlDeviceBtn.Tag = deviceType == "Access Control Device" ? "Selected" : null;
            IPSpeakerBtn.Tag = deviceType == "IP Speaker" ? "Selected" : null;
            NetworkKeyboardBtn.Tag = deviceType == "Network Keyboard" ? "Selected" : null;

            // Update content visibility
            EncodingDeviceView.Visibility = deviceType == "Encoding Device" ? Visibility.Visible : Visibility.Collapsed;
            DecodingDeviceView.Visibility = deviceType == "Decoding Device" ? Visibility.Visible : Visibility.Collapsed;
            CloudDeviceView.Visibility = deviceType == "Cloud Device" ? Visibility.Visible : Visibility.Collapsed;
            AccessControlDeviceView.Visibility = deviceType == "Access Control Device" ? Visibility.Visible : Visibility.Collapsed;
            IPSpeakerView.Visibility = deviceType == "IP Speaker" ? Visibility.Visible : Visibility.Collapsed;
            NetworkKeyboardView.Visibility = deviceType == "Network Keyboard" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn && btn.Content is string text)
            {
                SetActiveTab(text);
            }
        }

        private void DeviceTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                // Find the TextBlock with device name inside the button's StackPanel
                if (btn.Content is System.Windows.Controls.StackPanel stackPanel)
                {
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is System.Windows.Controls.TextBlock textBlock && 
                            textBlock.Text != null && 
                            !string.IsNullOrEmpty(textBlock.Text) &&
                            textBlock.Text.Length > 2) // Skip emoji TextBlocks
                        {
                            SetActiveDeviceType(textBlock.Text);
                            break;
                        }
                    }
                }
            }
        }

        // Toolbar button event handlers
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Device functionality will be implemented here", "Add Device");
        }

        private void EditDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit Device functionality will be implemented here", "Edit Device");
        }

        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete Device functionality will be implemented here", "Delete Device");
        }

        private void TimeSync_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Time Sync functionality will be implemented here", "Time Sync");
        }

        private void Status_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Device Status functionality will be implemented here", "Device Status");
        }

        private void DeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality here
            }
        }

        // DataGrid operation button event handlers
        private void EditDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit device from grid", "Edit Device");
        }

        private void SettingsDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Configure device settings", "Device Settings");
        }

        private void DeleteDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete device from grid", "Delete Device");
        }

        // Online Device toolbar button event handlers
        private void AddOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Online Device functionality will be implemented here", "Add Online Device");
        }

        private void RefreshOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Refresh Online Devices functionality will be implemented here", "Refresh");
        }

        private void SearchConfig_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Search Config functionality will be implemented here", "Search Config");
        }

        private void OnlineDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for online devices here
            }
        }
    }

    // Device data model
    public class Device
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string Status { get; set; }
        public string Model { get; set; }
        public string DeviceConfiguration { get; set; }
        public string VersionInfo { get; set; }
    }

    // Online Device data model
    public class OnlineDevice
    {
        public string IPAddress { get; set; }
        public string Model { get; set; }
        public string DeviceConfiguration { get; set; }
        public string SerialNo { get; set; }
        public string VersionInfo { get; set; }
        public string Added { get; set; }
    }
}
