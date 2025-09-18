using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System; // Added for EventArgs
using System.Windows.Interop; // Added for WindowInteropHelper
using System.Runtime.InteropServices; // Added for DllImport
using System.Windows.Data; // Added for IValueConverter
using System.Globalization; // Added for CultureInfo

namespace Vms_page
{
    // Converter to show/hide drop zone hint based on item count
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MenuItemModel
    {
        public string Name { get; set; }
        public string Group { get; set; } // "Basic" or "Smart"
        public string Icon { get; set; } // Unicode icon character
        public string IconColor { get; set; } // Hex color for the icon
        public string Description { get; set; } // Description text for the card
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItemModel> BasicMenuItems { get; set; } = new();
        public ObservableCollection<MenuItemModel> SmartMenuItems { get; set; } = new();
        public ObservableCollection<MenuItemModel> DroppedItems { get; set; } = new();
        private bool isDarkMode = true;

        // Converter for binding
        public CountToVisibilityConverter CountToVisibilityConverter { get; } = new CountToVisibilityConverter();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            // Ensure the requested dark palette is applied at startup
            ThemeManager.ApplyTheme("Dark");
            isDarkMode = true;
            
            // Ensure window respects taskbar
            this.SourceInitialized += MainWindow_SourceInitialized;
            
            // Add click outside handler for popup
            this.PreviewMouseDown += MainWindow_PreviewMouseDown;
            
            // Initialize theme button icon based on current theme
            UpdateThemeButtonIcon();
            
            // Initialize menu items with calm and attractive icon colors and descriptions
            BasicMenuItems.Add(new MenuItemModel { Name = "User Management", Group = "Basic", Icon = "👥", IconColor = "#81C784", Description = "Manage user accounts and permissions" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Recording Schedule", Group = "Basic", Icon = "📅", IconColor = "#64B5F6", Description = "Configure recording schedules and timings" });
            BasicMenuItems.Add(new MenuItemModel { Name = "View", Group = "Basic", Icon = "👁️", IconColor = "#BA68C8", Description = "Add, edit and delete views" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Alarm Records", Group = "Basic", Icon = "🚨", IconColor = "#E57373", Description = "View alarms in real time when any exception occurs" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Device Management", Group = "Basic", Icon = "⚙️", IconColor = "#FFB74D", Description = "Add, edit, delete and configure devices" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Alarm Configuration", Group = "Basic", Icon = "🔧", IconColor = "#A1887F", Description = "Configure arming schedules and actions to trigger" });
            BasicMenuItems.Add(new MenuItemModel { Name = "System Configuration", Group = "Basic", Icon = "🖥️", IconColor = "#90A4AE", Description = "Configure system settings and preferences" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Sequence Resource", Group = "Basic", Icon = "📋", IconColor = "#4DD0E1", Description = "Manage sequence resources and configurations" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Live View", Group = "Basic", Icon = "📹", IconColor = "#F06292", Description = "Monitor live camera feeds and streams" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Operation Log", Group = "Basic", Icon = "📝", IconColor = "#AED581", Description = "View and manage operation logs" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Playback", Group = "Basic", Icon = "⏯️", IconColor = "#FF8A65", Description = "Playback recorded video and audio" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Audio", Group = "Basic", Icon = "🔊", IconColor = "#7986CB", Description = "Configure audio settings and controls" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Video Wall", Group = "Basic", Icon = "🖼️", IconColor = "#4DB6AC", Description = "Configure and manage video wall displays" });
            BasicMenuItems.Add(new MenuItemModel { Name = "E-map", Group = "Basic", Icon = "🗺️", IconColor = "#9575CD", Description = "Configure electronic maps and layouts" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Recording Schdual", Group = "Basic", Icon = "⏰", IconColor = "#FFD54F", Description = "Schedule and manage recording tasks" });
            SmartMenuItems.Add(new MenuItemModel { Name = "Face Recognition", Group = "Smart", Icon = "👤", IconColor = "#4DD0E1", Description = "Advanced face recognition and identification" });
            SmartMenuItems.Add(new MenuItemModel { Name = "Access Control", Group = "Smart", Icon = "🔐", IconColor = "#81C784", Description = "Manage access control systems and permissions" });
            SmartMenuItems.Add(new MenuItemModel { Name = "LPR", Group = "Smart", Icon = "🚗", IconColor = "#FFB74D", Description = "License Plate Recognition system" });
            SmartMenuItems.Add(new MenuItemModel { Name = "People Counting", Group = "Smart", Icon = "👥", IconColor = "#BA68C8", Description = "Monitor and count people in areas" });
            SmartMenuItems.Add(new MenuItemModel { Name = "VCA", Group = "Smart", Icon = "🎯", IconColor = "#F06292", Description = "Video Content Analysis and analytics" });
        }



        private void MenuButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var button = sender as Button;
                if (button?.Tag is MenuItemModel item)
                {
                    DragDrop.DoDragDrop(button, item, DragDropEffects.Move);
                }
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is MenuItemModel item)
            {
                // Handle specific menu item clicks
                if (item.Name == "View")
                {
                    var viewWindow = new ViewWindow();
                    viewWindow.Show();
                }
                else if (item.Name == "Live View")
                {
                    var liveViewWindow = new LiveViewWindow();
                    liveViewWindow.Show();
                }
                else if (item.Name == "User Management")
                {
                    var userManagementWindow = new UserManagementWindow();
                    userManagementWindow.Show();
                }
                else if (item.Name == "Alarm Records")
                {
                    var alarmRecordsWindow = new AlarmRecordsWindow();
                    alarmRecordsWindow.Show();
                }
                else if (item.Name == "VCA")
                {
                    var vcaWindow = new VcaWindow();
                    vcaWindow.Show();
                }
                else if (item.Name == "Face Recognition")
                {
                    var frWindow = new FaceRecognitionWindow();
                    frWindow.Show();
                }
                else if (item.Name == "Recording Schedule")
                {
                    var rsWindow = new RecordingScheduleWindow();
                    rsWindow.Show();
                }
                else if (item.Name == "Operation Log")
                {
                    var logWindow = new OperationLogWindow();
                    logWindow.Show();
                }
                else
                {
                    // Show alert for other menu items
                    MessageBox.Show($"Menu item '{item.Name}' was clicked!\n\nFunction: {item.Description}", 
                                  "Menu Item Clicked", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                }
            }
        }

        private void DropTargetGrid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MenuItemModel)))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void DropTargetGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MenuItemModel)))
            {
                var item = (MenuItemModel)e.Data.GetData(typeof(MenuItemModel));
                if (!DroppedItems.Contains(item))
                {
                    DroppedItems.Add(item);
                    if (item.Group == "Basic")
                        BasicMenuItems.Remove(item);
                    else if (item.Group == "Smart")
                        SmartMenuItems.Remove(item);
                }
            }
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is MenuItemModel item)
            {
                // Check if it's Live View and open the Live View window
                if (item.Name == "Live View")
                {
                    var liveViewWindow = new LiveViewWindow();
                    liveViewWindow.Show();
                }
                // Check if it's View and open the View window
                else if (item.Name == "View")
                {
                    var viewWindow = new ViewWindow();
                    viewWindow.Show();
                }
                // Check if it's User Management and open the User Management window
                else if (item.Name == "User Management")
                {
                    var userManagementWindow = new UserManagementWindow();
                    userManagementWindow.Show();
                }
                // Check if it's Alarm Records and open the Alarm Records window
                else if (item.Name == "Alarm Records")
                {
                    var alarmRecordsWindow = new AlarmRecordsWindow();
                    alarmRecordsWindow.Show();
                }
                // Check if it's People Counting and open the People Counting window
                else if (item.Name == "People Counting")
                {
                    var peopleCountingWindow = new PeopleCountingWindow();
                    peopleCountingWindow.Show();
                }
                else if (item.Name == "VCA")
                {
                    var vcaWindow = new VcaWindow();
                    vcaWindow.Show();
                }
                else if (item.Name == "Sequence Resource")
                {
                    var seqWindow = new SequenceResourceWindow();
                    seqWindow.Show();
                }
                else if (item.Name == "Audio")
                {
                    var audioWindow = new AudioWindow();
                    audioWindow.Show();
                }
                // Check if it's E-map and open the E-map window
                else if (item.Name == "E-map")
                {
                    var emapWindow = new EMapWindow();
                    emapWindow.Show();
                }
                else if (item.Name == "VCA")
                {
                    var vcaWindow = new VcaWindow();
                    vcaWindow.Show();
                }
                else if (item.Name == "Face Recognition")
                {
                    var frWindow = new FaceRecognitionWindow();
                    frWindow.Show();
                }
                else if (item.Name == "Recording Schedule")
                {
                    var rsWindow = new RecordingScheduleWindow();
                    rsWindow.Show();
                }
                else if (item.Name == "Operation Log")
                {
                    var logWindow = new OperationLogWindow();
                    logWindow.Show();
                }
                else
                {
                    // Show alert for other cards
                    MessageBox.Show($"Card '{item.Name}' was clicked!\n\nFunction: {item.Description}", 
                                  "Card Clicked", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                }
            }
        }

        private void RemoveCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is MenuItemModel item)
            {
                // Remove the card from dropped items
                DroppedItems.Remove(item);
                
                // Add the item back to its original group
                if (item.Group == "Basic")
                    BasicMenuItems.Add(item);
                else if (item.Group == "Smart")
                    SmartMenuItems.Add(item);
            }
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            // Ensure window respects taskbar by setting appropriate window style
            // This prevents the window from covering the taskbar
            var helper = new System.Windows.Interop.WindowInteropHelper(this);
            var source = System.Windows.Interop.HwndSource.FromHwnd(helper.Handle);
            if (source?.Handle != IntPtr.Zero)
            {
                // Set window style to respect taskbar
                var style = NativeMethods.GetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE);
                style |= NativeMethods.WS_EX_APPWINDOW;
                style &= ~NativeMethods.WS_EX_TOOLWINDOW;
                NativeMethods.SetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE, style);
            }
        }
        
        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the popup
            ThemePopup.IsOpen = !ThemePopup.IsOpen;
        }
        
        private void LightMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Light");
            isDarkMode = false;
            UpdateThemeButtonIcon();
            ThemePopup.IsOpen = false;
        }
        
        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Dark");
            isDarkMode = true;
            UpdateThemeButtonIcon();
            ThemePopup.IsOpen = false;
        }
        
        private void UpdateThemeButtonIcon()
        {
            // Update the button icon based on current theme
            var themeButton = ThemeButton.Content as TextBlock;
            if (themeButton != null)
            {
                themeButton.Text = isDarkMode ? "🌙" : "☀️";
            }
        }

        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Check if the click is outside the theme popup
            if (ThemePopup.IsOpen && !ThemePopup.IsMouseOver)
            {
                ThemePopup.IsOpen = false;
            }
        }
        
        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle lock state
            var lockButton = LockButton.Content as TextBlock;
            if (lockButton != null)
            {
                if (lockButton.Text == "🔒")
                {
                    lockButton.Text = "🔓";
                    MessageBox.Show("Application unlocked!", "Lock Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    lockButton.Text = "🔒";
                    MessageBox.Show("Application locked!", "Lock Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Show menu options
            MessageBox.Show("Menu options:\n• Settings\n• Help\n• About\n• Exit", "Menu", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


}
// This code is part of a WPF application that implements a main window with buttons for various functionalities.