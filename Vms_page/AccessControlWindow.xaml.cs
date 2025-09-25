using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vms_page
{
    public partial class AccessControlWindow : Window
    {
        public AccessControlWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            var currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);
            
            // Initialize camera grid with default 4-camera layout
            InitializeCameraGrid();
        }

        private void InitializeCameraGrid()
        {
            // Clear existing children
            AccessControlCameraTilesHost.Children.Clear();
            
            // Set default 4-camera layout
            AccessControlCameraTilesHost.Rows = 2;
            AccessControlCameraTilesHost.Columns = 2;
            
            // Create default camera tiles
            CreateCameraTile("Camera 01", "#ff4444", 0);
            CreateCameraTile("Camera 02", "#44ff44", 1);
            CreateCameraTile("Camera 03", "#ffaa00", 2);
            CreateCameraTile("Camera 04", "#4444ff", 3);
        }

        private void LibraryManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all button styles
            LibraryManagementButton.Style = (Style)FindResource("ActiveNavButtonStyle");
            RealtimeMonitoringButton.Style = (Style)FindResource("NavButtonStyle");
            EntryExitRecordsButton.Style = (Style)FindResource("NavButtonStyle");

            // Show Library Management content
            LibraryManagementContent.Visibility = Visibility.Visible;
            RealtimeMonitoringContent.Visibility = Visibility.Collapsed;
            EntryExitRecordsContent.Visibility = Visibility.Collapsed;
        }

        private void RealtimeMonitoringButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all button styles
            LibraryManagementButton.Style = (Style)FindResource("NavButtonStyle");
            RealtimeMonitoringButton.Style = (Style)FindResource("ActiveNavButtonStyle");
            EntryExitRecordsButton.Style = (Style)FindResource("NavButtonStyle");

            // Show Realtime Monitoring content
            LibraryManagementContent.Visibility = Visibility.Collapsed;
            RealtimeMonitoringContent.Visibility = Visibility.Visible;
            EntryExitRecordsContent.Visibility = Visibility.Collapsed;
        }

        private void EntryExitRecordsButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset all button styles
            LibraryManagementButton.Style = (Style)FindResource("NavButtonStyle");
            RealtimeMonitoringButton.Style = (Style)FindResource("NavButtonStyle");
            EntryExitRecordsButton.Style = (Style)FindResource("ActiveNavButtonStyle");

            // Show Entry/Exit Records content
            LibraryManagementContent.Visibility = Visibility.Collapsed;
            RealtimeMonitoringContent.Visibility = Visibility.Collapsed;
            EntryExitRecordsContent.Visibility = Visibility.Visible;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get search criteria
            string name = NameTextBox.Text?.Trim() ?? string.Empty;
            string id = IdTextBox.Text?.Trim() ?? string.Empty;

            // TODO: Implement search functionality
            // This is where you would perform the actual search based on the criteria
            // Search functionality - no popup needed
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear search fields
            NameTextBox.Text = string.Empty;
            IdTextBox.Text = string.Empty;

            // TODO: Reset any search results or filters
            // This is where you would clear search results and show all items
        }

        // Toolbar Action Buttons
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement add functionality
            // Add functionality - no popup needed
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement delete functionality
            // Delete functionality - no popup needed
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement import functionality
            // Import functionality - no popup needed
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement export functionality
            // Export functionality - no popup needed
        }

        private void GenerateTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement generate template functionality
            // Generate Template functionality - no popup needed
        }

        // View Toggle Buttons
        private void GridViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to grid view
            GridViewButton.Background = (Brush)FindResource("PrimaryColor");
            ListViewButton.Background = (Brush)FindResource("CardBackgroundColor");
            
            // TODO: Switch content to grid view
            // Switched to Grid View - no popup needed
        }

        private void ListViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to list view
            ListViewButton.Background = (Brush)FindResource("PrimaryColor");
            GridViewButton.Background = (Brush)FindResource("CardBackgroundColor");
            
            // TODO: Switch content to list view
            // Switched to List View - no popup needed
        }

        // Realtime Monitoring Event Handlers (LPR-style)
        private void VideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Enter channel name")
            {
                textBox.Text = string.Empty;
            }
        }

        private void VideoChannelInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter channel name";
            }
        }

        private void SetLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string layout = button.Tag?.ToString() ?? "1";
                
                // Clear existing children
                AccessControlCameraTilesHost.Children.Clear();
                
                switch (layout)
                {
                    case "1":
                        // Single camera view
                        AccessControlCameraTilesHost.Rows = 1;
                        AccessControlCameraTilesHost.Columns = 1;
                        CreateCameraTile("Camera 01", "#ff4444", 0);
                        break;
                    case "4":
                        // 4-camera grid view
                        AccessControlCameraTilesHost.Rows = 2;
                        AccessControlCameraTilesHost.Columns = 2;
                        CreateCameraTile("Camera 01", "#ff4444", 0);
                        CreateCameraTile("Camera 02", "#44ff44", 1);
                        CreateCameraTile("Camera 03", "#ffaa00", 2);
                        CreateCameraTile("Camera 04", "#4444ff", 3);
                        break;
                    case "9":
                        // 9-camera grid view
                        AccessControlCameraTilesHost.Rows = 3;
                        AccessControlCameraTilesHost.Columns = 3;
                        for (int i = 1; i <= 9; i++)
                        {
                            string[] colors = { "#ff4444", "#44ff44", "#ffaa00", "#4444ff", "#ff44ff", "#44ffff", "#ffff44", "#ff8844", "#8844ff" };
                            CreateCameraTile($"Camera {i:D2}", colors[(i-1) % colors.Length], i-1);
                        }
                        break;
                    case "16":
                        // 16-camera grid view
                        AccessControlCameraTilesHost.Rows = 4;
                        AccessControlCameraTilesHost.Columns = 4;
                        for (int i = 1; i <= 16; i++)
                        {
                            string[] colors = { "#ff4444", "#44ff44", "#ffaa00", "#4444ff", "#ff44ff", "#44ffff", "#ffff44", "#ff8844", "#8844ff", "#44ff88", "#ff4488", "#88ff44", "#4488ff", "#ff8844", "#8844ff", "#ff44aa" };
                            CreateCameraTile($"Camera {i:D2}", colors[(i-1) % colors.Length], i-1);
                        }
                        break;
                }
                
                // Layout changed successfully - no popup needed
            }
        }

        private void CreateCameraTile(string cameraName, string statusColor, int index)
        {
            // Create the camera tile border (same design as LiveViewWindow)
            var tileBorder = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0),
                CornerRadius = new CornerRadius(0),
                Cursor = Cursors.Hand
            };

            // Add click event handler
            tileBorder.MouseLeftButtonDown += CameraTile_Click;

            // Create the main grid
            var mainGrid = new Grid();

            // Create the camera feed area
            var feedGrid = new Grid();

            // Add camera icon using PNG image file
            var cameraIcon = new Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Assets/Icons/cctv.png", UriKind.Relative)),
                Width = 60,
                Height = 60,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Uniform
            };

            // Simplified design - no status indicators or name overlays like LiveViewWindow

            // Add elements to feed grid
            feedGrid.Children.Add(cameraIcon);

            // Add to main grid (simplified like LiveViewWindow)
            mainGrid.Children.Add(feedGrid);

            tileBorder.Child = mainGrid;

            // Add to the uniform grid
            AccessControlCameraTilesHost.Children.Add(tileBorder);
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement fullscreen functionality
            // Fullscreen mode activated - no popup needed
        }

        // Camera Selection Functionality (same as LiveViewWindow)
        private void CameraTile_Click(object sender, MouseButtonEventArgs e)
        {
            // Clear previous selection
            ClearCameraTileSelection();
            
            // Set new selection
            if (sender is Border border)
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)); // Orange border
            }
        }

        private void ClearCameraTileSelection()
        {
            // Clear all camera tile selections
            var mainGrid = AccessControlCameraTilesHost;
            if (mainGrid != null)
            {
                foreach (var child in mainGrid.Children)
                {
                    if (child is Border border)
                    {
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)); // Gray border
                    }
                }
            }
        }
    }
}
