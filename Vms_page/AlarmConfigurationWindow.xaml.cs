using System.Windows;
using System.Windows.Controls;

namespace Vms_page
{
    public partial class AlarmConfigurationWindow : Window
    {
        public AlarmConfigurationWindow()
        {
            InitializeComponent();
        }

        private void SetActiveTab(Button active, params Button[] others)
        {
            active.Style = (Style)FindResource("SelectedNavbarButtonStyle");
            foreach (var b in others)
            {
                b.Style = (Style)FindResource("NavbarButtonStyle");
            }
        }

        private void ShowContent(Grid show, params Grid[] hide)
        {
            show.Visibility = Visibility.Visible;
            foreach (var g in hide)
            {
                g.Visibility = Visibility.Collapsed;
            }
        }


        private void SetActiveSubTab(Button active, params Button[] others)
        {
            active.Style = (Style)FindResource("SelectedNavbarButtonStyle");
            foreach (var b in others)
            {
                b.Style = (Style)FindResource("NavbarButtonStyle");
            }
        }

        private void ShowSubContent(Grid show, params Grid[] hide)
        {
            show.Visibility = Visibility.Visible;
            foreach (var g in hide)
            {
                g.Visibility = Visibility.Collapsed;
            }
        }

        private void CameraAlarmTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(CameraAlarmTab, DeviceAlarmTab, PeopleCountingAlarmTab);
            ShowContent(CameraAlarmContent, DeviceAlarmContent, PeopleCountingAlarmContent);
            // Reset to Alarm Parameters when switching to Camera Alarm
            SetActiveSubTab(CameraAlarmParametersTab, CameraTriggerActionTab);
            ShowSubContent(CameraAlarmParametersContent, CameraTriggerActionContent);
        }

        private void DeviceAlarmTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(DeviceAlarmTab, CameraAlarmTab, PeopleCountingAlarmTab);
            ShowContent(DeviceAlarmContent, CameraAlarmContent, PeopleCountingAlarmContent);
            // Reset to Alarm Parameters when switching to Device Alarm
            SetActiveSubTab(DeviceAlarmParametersTab, DeviceTriggerActionTab);
            ShowSubContent(DeviceAlarmParametersContent, DeviceTriggerActionContent);
        }

        private void PeopleCountingAlarmTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveTab(PeopleCountingAlarmTab, CameraAlarmTab, DeviceAlarmTab);
            ShowContent(PeopleCountingAlarmContent, CameraAlarmContent, DeviceAlarmContent);
            // People Counting only shows Trigger Action
            SetActiveSubTab(PeopleCountingTriggerActionTab);
        }

        private void CameraSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CameraSearchTextBox.Text == "Enter camera name")
            {
                CameraSearchTextBox.Text = string.Empty;
                CameraSearchTextBox.Foreground = (System.Windows.Media.Brush)FindResource("TextPrimaryColor");
            }
        }

        private void CameraSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CameraSearchTextBox.Text))
            {
                CameraSearchTextBox.Text = "Enter camera name";
                CameraSearchTextBox.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
            }
        }

        private void CameraAlarmParametersTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSubTab(CameraAlarmParametersTab, CameraTriggerActionTab);
            ShowSubContent(CameraAlarmParametersContent, CameraTriggerActionContent);
        }

        private void CameraTriggerActionTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSubTab(CameraTriggerActionTab, CameraAlarmParametersTab);
            ShowSubContent(CameraTriggerActionContent, CameraAlarmParametersContent);
        }

        private void DeviceAlarmParametersTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSubTab(DeviceAlarmParametersTab, DeviceTriggerActionTab);
            ShowSubContent(DeviceAlarmParametersContent, DeviceTriggerActionContent);
        }

        private void DeviceTriggerActionTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSubTab(DeviceTriggerActionTab, DeviceAlarmParametersTab);
            ShowSubContent(DeviceTriggerActionContent, DeviceAlarmParametersContent);
        }

        private void PeopleCountingTriggerActionTab_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSubTab(PeopleCountingTriggerActionTab);
            // People Counting only has one option, so no content switching needed
        }

        // Camera Alarm Action Button Handlers
        private void CameraAddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Camera Add button clicked!", "Camera Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CameraDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Camera Delete button clicked!", "Camera Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Device Alarm Action Button Handlers
        private void DeviceAddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Device Add button clicked!", "Device Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeviceDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Device Delete button clicked!", "Device Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // People Counting Alarm Action Button Handlers
        private void PeopleCountingAddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("People Counting Add button clicked!", "People Counting Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PeopleCountingDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("People Counting Delete button clicked!", "People Counting Alarm", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}


