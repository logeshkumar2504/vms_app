using System;
using System.Windows;

namespace Vms_page
{
    public partial class DeviceInfoPopup : Window
    {
        public event EventHandler<DeviceInfoEventArgs> DeviceAdded;
        public event EventHandler<DeviceInfoEventArgs> DeviceAddedToGroup;

        public DeviceInfoPopup()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            
            // Set focus to first input field
            Loaded += (s, e) => DeviceNameTextBox.Focus();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var deviceInfo = GetDeviceInfo();
                DeviceAdded?.Invoke(this, new DeviceInfoEventArgs(deviceInfo));
                DialogResult = true;
                Close();
            }
        }

        private void AddToGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var deviceInfo = GetDeviceInfo();
                DeviceAddedToGroup?.Invoke(this, new DeviceInfoEventArgs(deviceInfo));
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            bool isValid = true;
            string errorMessage = "";

            // Validate Device Name
            if (string.IsNullOrWhiteSpace(DeviceNameTextBox.Text))
            {
                errorMessage += "Device Name is required.\n";
                isValid = false;
            }

            // Validate IP/Domain based on selected mode
            if (IPDomainRadio.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(IPDomainTextBox.Text))
                {
                    errorMessage += "IP/Domain is required.\n";
                    isValid = false;
                }
            }
            else if (IPSegmentRadio.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(IPDomainTextBox.Text))
                {
                    errorMessage += "IP Segment is required.\n";
                    isValid = false;
                }
            }
            else if (MyDDNSRadio.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(IPDomainTextBox.Text))
                {
                    errorMessage += "MyDDNS is required.\n";
                    isValid = false;
                }
            }

            // Validate Port
            if (string.IsNullOrWhiteSpace(PortTextBox.Text) || !int.TryParse(PortTextBox.Text, out int port) || port <= 0 || port > 65535)
            {
                errorMessage += "Port must be a valid number between 1 and 65535.\n";
                isValid = false;
            }

            // Validate Username
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                errorMessage += "Username is required.\n";
                isValid = false;
            }

            // Password is optional, so no validation needed

            if (!isValid)
            {
                MessageBox.Show(errorMessage.TrimEnd('\n'), "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return isValid;
        }

        private DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo
            {
                DeviceName = DeviceNameTextBox.Text.Trim(),
                IPDomain = IPDomainTextBox.Text.Trim(),
                Port = int.Parse(PortTextBox.Text),
                Username = UsernameTextBox.Text.Trim(),
                Password = PasswordTextBox.Password,
                AddingMode = GetSelectedAddingMode()
            };
        }

        private AddingMode GetSelectedAddingMode()
        {
            if (IPDomainRadio.IsChecked == true)
                return AddingMode.IPDomain;
            else if (IPSegmentRadio.IsChecked == true)
                return AddingMode.IPSegment;
            else if (MyDDNSRadio.IsChecked == true)
                return AddingMode.MyDDNS;
            else
                return AddingMode.IPDomain; // Default
        }
    }

    public class DeviceInfoEventArgs : EventArgs
    {
        public DeviceInfo DeviceInfo { get; }

        public DeviceInfoEventArgs(DeviceInfo deviceInfo)
        {
            DeviceInfo = deviceInfo;
        }
    }

    public class DeviceInfo
    {
        public string DeviceName { get; set; }
        public string IPDomain { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AddingMode AddingMode { get; set; }
    }

    public enum AddingMode
    {
        IPDomain,
        IPSegment,
        MyDDNS
    }
}
