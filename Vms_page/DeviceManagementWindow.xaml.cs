using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Vms_page
{
    public partial class DeviceManagementWindow : Window
    {
        public ObservableCollection<Device> Devices { get; set; }
        public ObservableCollection<OnlineDevice> OnlineDevices { get; set; }
        public ObservableCollection<Device> DecodingDevices { get; set; }
        public ObservableCollection<OnlineDevice> DecodingOnlineDevices { get; set; }
        public ObservableCollection<Device> AccessControlDevices { get; set; }
        public ObservableCollection<OnlineDevice> AccessControlOnlineDevices { get; set; }
        public ObservableCollection<Device> IPSpeakerDevices { get; set; }
        public ObservableCollection<OnlineDevice> IPSpeakerOnlineDevices { get; set; }

        public DeviceManagementWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            SetActiveTab("Device");
            SetActiveDeviceType("Encoding Device");
            InitializeDeviceList();
            InitializeOnlineDeviceList();
            InitializeDecodingDeviceList();
            InitializeDecodingOnlineDeviceList();
            InitializeAccessControlDeviceList();
            InitializeAccessControlOnlineDeviceList();
            InitializeIPSpeakerDeviceList();
            InitializeIPSpeakerOnlineDeviceList();
            InitializeNetworkKeyboardNavbar();
        }

        private void InitializeNetworkKeyboardNavbar()
        {
            // Default active link
            SetNKActiveLink(NK_CameraLink);
        }

        private void SetNKActiveLink(System.Windows.Controls.TextBlock active)
        {
            if (NK_CameraLink != null)
            {
                NK_CameraLink.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
                NK_CameraLink.FontWeight = System.Windows.FontWeights.Normal;
                NK_CameraLink.TextDecorations = null;
            }
            if (NK_VideowallLink != null)
            {
                NK_VideowallLink.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
                NK_VideowallLink.FontWeight = System.Windows.FontWeights.Normal;
                NK_VideowallLink.TextDecorations = null;
            }
            if (NK_SequenceLink != null)
            {
                NK_SequenceLink.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
                NK_SequenceLink.FontWeight = System.Windows.FontWeights.Normal;
                NK_SequenceLink.TextDecorations = null;
            }

            active.Foreground = (System.Windows.Media.Brush)FindResource("PrimaryColor");
            active.FontWeight = System.Windows.FontWeights.SemiBold;
            active.TextDecorations = System.Windows.TextDecorations.Underline;
        }

        private void NK_CameraLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetNKActiveLink(NK_CameraLink);
            NK_CameraContent.Visibility = Visibility.Visible;
            NK_VideowallContent.Visibility = Visibility.Collapsed;
            NK_SequenceContent.Visibility = Visibility.Collapsed;
        }

        private void NK_VideowallLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetNKActiveLink(NK_VideowallLink);
            NK_CameraContent.Visibility = Visibility.Collapsed;
            NK_VideowallContent.Visibility = Visibility.Visible;
            NK_SequenceContent.Visibility = Visibility.Collapsed;
        }

        private void NK_SequenceLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetNKActiveLink(NK_SequenceLink);
            NK_CameraContent.Visibility = Visibility.Collapsed;
            NK_VideowallContent.Visibility = Visibility.Collapsed;
            NK_SequenceContent.Visibility = Visibility.Visible;
        }

        private void NK_AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // Reuse existing popup for demo; this can be replaced with NK-specific popup later
            var deviceInfoPopup = new DeviceInfoPopup();
            deviceInfoPopup.Owner = this;
            deviceInfoPopup.ShowDialog();
        }

        private void NK_DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete action for Network Keyboard section.");
        }

        private void NK_SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Placeholder for future filtering of NK_CameraGrid items
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

        private void InitializeDecodingDeviceList()
        {
            // Initialize empty decoding device list
            DecodingDevices = new ObservableCollection<Device>();
            DecodingDeviceGrid.ItemsSource = DecodingDevices;
            UpdateDecodingDeviceCount();
        }

        private void InitializeDecodingOnlineDeviceList()
        {
            // Initialize empty decoding online device list
            DecodingOnlineDevices = new ObservableCollection<OnlineDevice>();
            DecodingOnlineDeviceGrid.ItemsSource = DecodingOnlineDevices;
            UpdateDecodingOnlineDeviceCount();
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

        private void UpdateDecodingDeviceCount()
        {
            if (DecodingDevices != null)
            {
                DecodingDeviceCount.Text = $"Managed Device({DecodingDevices.Count})";
            }
        }

        private void UpdateDecodingOnlineDeviceCount()
        {
            if (DecodingOnlineDevices != null)
            {
                DecodingOnlineDeviceCount.Text = $"Online Device({DecodingOnlineDevices.Count})";
            }
        }

        private void InitializeAccessControlDeviceList()
        {
            // Initialize empty access control device list
            AccessControlDevices = new ObservableCollection<Device>();
            AccessControlDeviceGrid.ItemsSource = AccessControlDevices;
            UpdateAccessControlDeviceCount();
        }

        private void InitializeAccessControlOnlineDeviceList()
        {
            // Initialize empty access control online device list
            AccessControlOnlineDevices = new ObservableCollection<OnlineDevice>();
            AccessControlOnlineDeviceGrid.ItemsSource = AccessControlOnlineDevices;
            UpdateAccessControlOnlineDeviceCount();
        }

        private void UpdateAccessControlDeviceCount()
        {
            if (AccessControlDevices != null)
            {
                AccessControlDeviceCount.Text = $"Managed Device({AccessControlDevices.Count})";
            }
        }

        private void UpdateAccessControlOnlineDeviceCount()
        {
            if (AccessControlOnlineDevices != null)
            {
                AccessControlOnlineDeviceCount.Text = $"Online Device({AccessControlOnlineDevices.Count})";
            }
        }

        private void InitializeIPSpeakerDeviceList()
        {
            // Initialize empty IP Speaker device list
            IPSpeakerDevices = new ObservableCollection<Device>();
            IPSpeakerDeviceGrid.ItemsSource = IPSpeakerDevices;
            UpdateIPSpeakerDeviceCount();
        }

        private void InitializeIPSpeakerOnlineDeviceList()
        {
            // Initialize empty IP Speaker online device list
            IPSpeakerOnlineDevices = new ObservableCollection<OnlineDevice>();
            IPSpeakerOnlineDeviceGrid.ItemsSource = IPSpeakerOnlineDevices;
            UpdateIPSpeakerOnlineDeviceCount();
        }

        private void UpdateIPSpeakerDeviceCount()
        {
            if (IPSpeakerDevices != null)
            {
                IPSpeakerDeviceCount.Text = $"Managed Device({IPSpeakerDevices.Count})";
            }
        }

        private void UpdateIPSpeakerOnlineDeviceCount()
        {
            if (IPSpeakerOnlineDevices != null)
            {
                IPSpeakerOnlineDeviceCount.Text = $"Online Device({IPSpeakerOnlineDevices.Count})";
            }
        }

        private void SetActiveTab(string tab)
        {
            // Update button visual states
            if (CameraAlarmBtn != null && DeviceAlarmBtn != null)
            {
                // Reset both buttons
                CameraAlarmBtn.Background = (System.Windows.Media.Brush)FindResource("CardBackgroundColor");
                DeviceAlarmBtn.Background = (System.Windows.Media.Brush)FindResource("CardBackgroundColor");
                
                // Highlight active button
                if (tab == "Device")
                {
                    CameraAlarmBtn.Background = (System.Windows.Media.Brush)FindResource("PrimaryColor");
                    CameraAlarmBtn.Foreground = System.Windows.Media.Brushes.White;
                }
                else if (tab == "Group")
                {
                    DeviceAlarmBtn.Background = (System.Windows.Media.Brush)FindResource("PrimaryColor");
                    DeviceAlarmBtn.Foreground = System.Windows.Media.Brushes.White;
                }
            }

            // Update content visibility
            if (CameraAlarmView != null && DeviceAlarmView != null)
            {
                CameraAlarmView.Visibility = tab == "Device" ? Visibility.Visible : Visibility.Collapsed;
                DeviceAlarmView.Visibility = tab == "Group" ? Visibility.Visible : Visibility.Collapsed;
            }
            
            // Show/hide left device type sidebar based on tab
            if (tab == "Device")
            {
                if (DeviceSidebar != null)
                    DeviceSidebar.Visibility = Visibility.Visible;
                if (SidebarColumn != null)
                    SidebarColumn.Width = new System.Windows.GridLength(180);
            }
            else
            {
                if (DeviceSidebar != null)
                    DeviceSidebar.Visibility = Visibility.Collapsed;
                if (SidebarColumn != null)
                    SidebarColumn.Width = new System.Windows.GridLength(0);
            }
        }

        private void SetActiveDeviceType(string deviceType)
        {
            System.Diagnostics.Debug.WriteLine($"SetActiveDeviceType called with: {deviceType}");
            
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
            
            System.Diagnostics.Debug.WriteLine($"Views updated for: {deviceType}");
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
                // Debug: Show which button was clicked
                System.Diagnostics.Debug.WriteLine($"Button clicked: {btn.Name}");
                
                // Check if the button content is directly a TextBlock
                if (btn.Content is System.Windows.Controls.TextBlock textBlock)
                {
                    System.Diagnostics.Debug.WriteLine($"TextBlock text: {textBlock.Text}");
                    SetActiveDeviceType(textBlock.Text);
                }
                // Fallback: Check if the button content is a StackPanel (for backward compatibility)
                else if (btn.Content is System.Windows.Controls.StackPanel stackPanel)
                {
                    foreach (var child in stackPanel.Children)
                    {
                        if (child is System.Windows.Controls.TextBlock childTextBlock && 
                            childTextBlock.Text != null && 
                            !string.IsNullOrEmpty(childTextBlock.Text) &&
                            childTextBlock.Text.Length > 2) // Skip emoji TextBlocks
                        {
                            System.Diagnostics.Debug.WriteLine($"StackPanel TextBlock text: {childTextBlock.Text}");
                            SetActiveDeviceType(childTextBlock.Text);
                            break;
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Button content type: {btn.Content?.GetType()}");
                }
            }
        }

        // Toolbar button event handlers
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            var deviceInfoPopup = new DeviceInfoPopup();
            deviceInfoPopup.Owner = this;
            deviceInfoPopup.DeviceAdded += OnDeviceAdded;
            deviceInfoPopup.DeviceAddedToGroup += OnDeviceAddedToGroup;
            
            var result = deviceInfoPopup.ShowDialog();
            
            // Clean up event handlers
            deviceInfoPopup.DeviceAdded -= OnDeviceAdded;
            deviceInfoPopup.DeviceAddedToGroup -= OnDeviceAddedToGroup;
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

        // Decoding Device toolbar button event handlers
        private void AddDecodingDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Decoding Device functionality will be implemented here", "Add Decoding Device");
        }

        private void EditDecodingDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit Decoding Device functionality will be implemented here", "Edit Decoding Device");
        }

        private void DeleteDecodingDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete Decoding Device functionality will be implemented here", "Delete Decoding Device");
        }

        private void DecodingConfiguration_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Decoding Device Configuration functionality will be implemented here", "Configuration");
        }

        private void DecodingDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for decoding devices here
            }
        }


        // Decoding Online Device toolbar button event handlers
        private void AddDecodingOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Decoding Online Device functionality will be implemented here", "Add Decoding Online Device");
        }

        private void RefreshDecodingOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Refresh Decoding Online Devices functionality will be implemented here", "Refresh");
        }

        private void SearchDecodingConfig_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Search Decoding Config functionality will be implemented here", "Search Config");
        }

        private void DecodingOnlineDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for decoding online devices here
            }
        }

        // Cloud Device button event handlers
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Login functionality will be implemented here", "Login");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Register functionality will be implemented here", "Register");
        }

        // Access Control Device toolbar button event handlers
        private void AddAccessControlDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Access Control Device functionality will be implemented here", "Add Access Control Device");
        }

        private void EditAccessControlDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit Access Control Device functionality will be implemented here", "Edit Access Control Device");
        }

        private void DeleteAccessControlDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete Access Control Device functionality will be implemented here", "Delete Access Control Device");
        }

        private void AccessControlTimeSync_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Access Control Time Sync functionality will be implemented here", "Time Sync");
        }

        private void AccessControlConfiguration_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Access Control Device Configuration functionality will be implemented here", "Configuration");
        }

        private void AccessControlDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for access control devices here
            }
        }

        // Access Control Device DataGrid operation button event handlers
        private void EditAccessControlDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit access control device from grid", "Edit Device");
        }

        private void SettingsAccessControlDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Configure access control device settings", "Device Settings");
        }

        private void DeleteAccessControlDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete access control device from grid", "Delete Device");
        }

        // Access Control Online Device toolbar button event handlers
        private void AddAccessControlOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Access Control Online Device functionality will be implemented here", "Add Online Device");
        }

        private void RefreshAccessControlOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Refresh Access Control Online Devices functionality will be implemented here", "Refresh");
        }

        private void SearchAccessControlConfig_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Search Access Control Config functionality will be implemented here", "Search Config");
        }

        private void AccessControlOnlineDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for access control online devices here
            }
        }

        // IP Speaker Device toolbar button event handlers
        private void AddIPSpeakerDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add IP Speaker Device functionality will be implemented here", "Add IP Speaker Device");
        }

        private void EditIPSpeakerDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit IP Speaker Device functionality will be implemented here", "Edit IP Speaker Device");
        }

        private void DeleteIPSpeakerDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete IP Speaker Device functionality will be implemented here", "Delete IP Speaker Device");
        }

        private void IPSpeakerDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for IP Speaker devices here
            }
        }

        // IP Speaker Device DataGrid operation button event handlers
        private void EditIPSpeakerDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit IP Speaker device from grid", "Edit Device");
        }

        private void SettingsIPSpeakerDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Configure IP Speaker device settings", "Device Settings");
        }

        private void DeleteIPSpeakerDeviceGrid_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete IP Speaker device from grid", "Delete Device");
        }

        // IP Speaker Online Device toolbar button event handlers
        private void AddIPSpeakerOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add IP Speaker Online Device functionality will be implemented here", "Add Online Device");
        }

        private void RefreshIPSpeakerOnlineDevice_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Refresh IP Speaker Online Devices functionality will be implemented here", "Refresh");
        }

        private void SearchIPSpeakerConfig_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Search IP Speaker Config functionality will be implemented here", "Search Config");
        }

        private void IPSpeakerOnlineDeviceSearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                string searchText = textBox.Text;
                // TODO: Implement search/filter functionality for IP Speaker online devices here
            }
        }

        // Device Info Popup event handlers
        private void OnDeviceAdded(object sender, DeviceInfoEventArgs e)
        {
            // Add the device to the appropriate collection based on current device type
            var device = new Device
            {
                Name = e.DeviceInfo.DeviceName,
                IPAddress = e.DeviceInfo.IPDomain,
                Status = "Offline", // Default status
                Model = "Unknown", // Will be updated when device is discovered
                DeviceConfiguration = $"{e.DeviceInfo.Username}@{e.DeviceInfo.IPDomain}:{e.DeviceInfo.Port}",
                VersionInfo = "Unknown" // Will be updated when device is discovered
            };

            // Add to the appropriate device collection based on current view
            if (EncodingDeviceView.Visibility == Visibility.Visible)
            {
                Devices.Add(device);
                UpdateDeviceCount();
            }
            else if (DecodingDeviceView.Visibility == Visibility.Visible)
            {
                DecodingDevices.Add(device);
                UpdateDecodingDeviceCount();
            }
            else if (AccessControlDeviceView.Visibility == Visibility.Visible)
            {
                AccessControlDevices.Add(device);
                UpdateAccessControlDeviceCount();
            }
            else if (IPSpeakerView.Visibility == Visibility.Visible)
            {
                IPSpeakerDevices.Add(device);
                UpdateIPSpeakerDeviceCount();
            }

            MessageBox.Show($"Device '{e.DeviceInfo.DeviceName}' has been added successfully!", "Device Added", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnDeviceAddedToGroup(object sender, DeviceInfoEventArgs e)
        {
            // For now, just add the device normally
            // In a real application, this would show a group selection dialog
            OnDeviceAdded(sender, e);
            MessageBox.Show($"Device '{e.DeviceInfo.DeviceName}' has been added to the default group.", "Device Added to Group", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Top bar event handlers
        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            // Lock/Unlock functionality
            MessageBox.Show("Lock/Unlock functionality not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Main menu functionality
            MessageBox.Show("Main menu functionality not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle theme popup
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = !ThemePopup.IsOpen;
            }
        }

        private void LightMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Light");
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = false;
            }
        }

        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Dark");
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = false;
            }
        }

        // Group action button event handlers
        private void GroupAddButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Group functionality will be implemented here", "Add Group");
        }

        private void GroupEditButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit Group functionality will be implemented here", "Edit Group");
        }

        private void GroupDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete Group functionality will be implemented here", "Delete Group");
        }

        private void GroupImportCamera_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Import Camera functionality will be implemented here", "Import Camera");
        }

        private void GroupConfiguration_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Configuration functionality will be implemented here", "Configuration");
        }

        private void GroupSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Search functionality for Group view
            if (GroupSearchTextBox != null)
            {
                var searchText = GroupSearchTextBox.Text;
                // TODO: Implement search logic for groups
            }
        }

        // Left sidebar icon button event handlers
        private void GroupSidebarAddButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Group functionality will be implemented here", "Add Group");
        }

        private void GroupSidebarEditButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Edit Group functionality will be implemented here", "Edit Group");
        }

        private void GroupSidebarDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Delete Group functionality will be implemented here", "Delete Group");
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
