using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Collections.Generic;

namespace Vms_page
{
    public partial class FaceRecognitionWindow : Window
    {
        public FaceRecognitionWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            
            // Set default view to Realtime Monitoring
            SetDefaultView();
            
            // Ensure placeholder is set if empty on load
            if (VideoChannelInput != null && string.IsNullOrWhiteSpace(VideoChannelInput.Text))
            {
                VideoChannelInput.Text = "Enter channel name";
                VideoChannelInput.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
            }
            // Default to single view at startup
            SetLayout(1);
        }

        private void SetDefaultView()
        {
            // Show sidebar for Realtime Monitoring
            if (LeftSidebar != null)
            {
                LeftSidebar.Visibility = Visibility.Visible;
            }

            // Show appropriate sections in sidebar
            if (VideoChannelSection != null && FaceLibrarySection != null)
            {
                VideoChannelSection.Visibility = Visibility.Visible;
                FaceLibrarySection.Visibility = Visibility.Collapsed;
            }

            // Show Realtime Monitoring view by default
            if (RealtimeGrid != null && FaceLibraryGrid != null && EmptyState != null)
            {
                RealtimeGrid.Visibility = Visibility.Visible;
                FaceLibraryGrid.Visibility = Visibility.Collapsed;
                EmptyState.Visibility = Visibility.Collapsed;
            }
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string tag)
            {
                // Show appropriate sidebar based on selected tab
                if (LeftSidebar != null)
                {
                    var showLeftSidebar = tag == "Realtime Monitoring" || tag == "Face Library Management";
                    LeftSidebar.Visibility = showLeftSidebar ? Visibility.Visible : Visibility.Collapsed;
                }

                if (MonitoringTaskSidebar != null)
                {
                    var showMonitoringSidebar = tag == "Monitoring Task" || tag == "Alarm Records" || tag == "Pass Thru Records" || tag == "Search by Image";
                    MonitoringTaskSidebar.Visibility = showMonitoringSidebar ? Visibility.Visible : Visibility.Collapsed;
                }

                // Show appropriate sections in sidebar
                if (VideoChannelSection != null && FaceLibrarySection != null)
                {
                    VideoChannelSection.Visibility = tag == "Realtime Monitoring" ? Visibility.Visible : Visibility.Collapsed;
                    FaceLibrarySection.Visibility = tag == "Face Library Management" ? Visibility.Visible : Visibility.Collapsed;
                }

                // Toggle main views
                if (RealtimeGrid != null && FaceLibraryGrid != null && EmptyState != null)
                {
                    var isRealtime = tag == "Realtime Monitoring";
                    var isFaceLibrary = tag == "Face Library Management";
                    var isMonitoringTask = tag == "Monitoring Task";
                    var isAlarmRecords = tag == "Alarm Records";
                    var isPassThruRecord = tag == "Pass Thru Records";
                    var isSearchByImage = tag == "Search by Image";
                    
                    RealtimeGrid.Visibility = isRealtime ? Visibility.Visible : Visibility.Collapsed;
                    FaceLibraryGrid.Visibility = isFaceLibrary ? Visibility.Visible : Visibility.Collapsed;
                    EmptyState.Visibility = (isRealtime || isFaceLibrary) ? Visibility.Collapsed : Visibility.Visible;
                    
                    // Show/hide filter bar based on selected tab
                    if (FilterBar != null)
                    {
                        FilterBar.Visibility = (isRealtime || isFaceLibrary) ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    // Show/hide Monitoring Task header and table header based on selected tab
                    if (MonitoringTaskHeader != null)
                    {
                        MonitoringTaskHeader.Visibility = isMonitoringTask ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    if (MonitoringTaskTable != null)
                    {
                        MonitoringTaskTable.Visibility = isMonitoringTask ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    // Show/hide Alarm Records filter bar and export button based on selected tab
                    if (AlarmRecordsFilterBar != null)
                    {
                        AlarmRecordsFilterBar.Visibility = isAlarmRecords ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    if (AlarmRecordsMainContent != null)
                    {
                        AlarmRecordsMainContent.Visibility = isAlarmRecords ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    // Show/hide Pass Thru Record filter bar and main content based on selected tab
                    if (PassThruRecordFilterBar != null)
                    {
                        PassThruRecordFilterBar.Visibility = isPassThruRecord ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    if (PassThruRecordMainContent != null)
                    {
                        PassThruRecordMainContent.Visibility = isPassThruRecord ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    // Show/hide Search by Image filter bar and main content based on selected tab
                    if (SearchByImageFilterBar != null)
                    {
                        SearchByImageFilterBar.Visibility = isSearchByImage ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    if (SearchByImageMainContent != null)
                    {
                        SearchByImageMainContent.Visibility = isSearchByImage ? Visibility.Visible : Visibility.Collapsed;
                    }
                    
                    // For Monitoring Task, show completely empty state
                    if (isMonitoringTask)
                    {
                        SetMonitoringTaskEmptyState();
                    }
                    
                    // For Alarm Records, show completely blank state
                    if (isAlarmRecords)
                    {
                        SetAlarmRecordsEmptyState();
                    }
                    
                    // For Pass Thru Record, show completely blank state
                    if (isPassThruRecord)
                    {
                        SetPassThruRecordEmptyState();
                    }
                    
                    // For Search by Image, show completely blank state
                    if (isSearchByImage)
                    {
                        SetSearchByImageEmptyState();
                    }
                }
            }
        }

        private void VideoFeed_Click(object sender, MouseButtonEventArgs e)
        {
            // Clear previous selection
            ClearCameraTileSelection();

            // Highlight the clicked tile
            if (sender is Border border)
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)); // Orange
            }
        }

        private void SetLayout_Click(object sender, RoutedEventArgs e)
        {
            if (CameraTilesHost == null || sender is not FrameworkElement fe || fe.Tag is not string tag)
                return;

            int tileCount = tag switch
            {
                "1" => 1,
                "2" => 2,
                "4" => 4,
                _ => 4
            };

            SetLayout(tileCount);
        }

        private void SetLayout(int tileCount)
        {
            if (CameraTilesHost == null) return;

            switch (tileCount)
            {
                case 1:
                    CameraTilesHost.Rows = 1;
                    CameraTilesHost.Columns = 1;
                    break;
                case 2:
                    CameraTilesHost.Rows = 1;
                    CameraTilesHost.Columns = 2;
                    break;
                default:
                    CameraTilesHost.Rows = 2;
                    CameraTilesHost.Columns = 2;
                    break;
            }

            CameraTilesHost.Children.Clear();
            for (int i = 0; i < tileCount; i++)
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand
                };

                var grid = new Grid();
                var image = new Image
                {
                    Source = new BitmapImage(new System.Uri("/Assets/Icons/cctv.png", System.UriKind.Relative)),
                    Width = 60,
                    Height = 60,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Uniform
                };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                grid.Children.Add(image);
                border.Child = grid;
                border.MouseLeftButtonDown += VideoFeed_Click;
                CameraTilesHost.Children.Add(border);
            }
        }

        private void ClearCameraTileSelection()
        {
            if (CameraTilesHost == null) return;

            foreach (var child in CameraTilesHost.Children)
            {
                if (child is Border border)
                {
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)); // Gray
                }
            }
        }

        private void VideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (tb.Text == "Enter channel name")
                {
                    tb.Text = string.Empty;
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextPrimaryColor"];
                }
            }
        }

        private void VideoChannelInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = "Enter channel name";
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
                }
            }
        }

        private void MonitoringTaskVideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (tb.Text == "Enter channel name")
                {
                    tb.Text = string.Empty;
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextPrimaryColor"];
                }
            }
        }

        private void MonitoringTaskVideoChannelInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = "Enter channel name";
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
                }
            }
        }

        // Face Library Management event handlers
        private void BatchAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectFaceLibraryDialog
            {
                Owner = this
            };
            dialog.ShowDialog();
        }

        private void BatchDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Batch Delete functionality will be implemented here.", "Batch Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectFaceLibraryDialog
            {
                Owner = this
            };
            dialog.ShowDialog();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export functionality will be implemented here.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateTemplate_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Generate Template functionality will be implemented here.", "Generate Template", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewToggle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string viewType)
            {
                // Keep the same visual state as the image - no style switching
                // Grid button stays blue (active), List button stays gray (inactive)
                MessageBox.Show($"Switched to {viewType} view", "View Toggle", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Face Library Search/Filter event handlers
        private void FaceLibrarySearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Search functionality will be implemented here.", "Face Library Search", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FaceLibraryResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Reset functionality will be implemented here.", "Face Library Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Monitoring Task action button handlers
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add functionality will be implemented here.", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete functionality will be implemented here.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Copy functionality will be implemented here.", "Copy", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SetMonitoringTaskEmptyState()
        {
            if (EmptyState == null) return;

            // Hide the filter bar for Monitoring Task
            if (FilterBar != null)
            {
                FilterBar.Visibility = Visibility.Collapsed;
            }

            // Show the Monitoring Task header and table header
            if (MonitoringTaskHeader != null)
            {
                MonitoringTaskHeader.Visibility = Visibility.Visible;
            }

            if (MonitoringTaskTable != null)
            {
                MonitoringTaskTable.Visibility = Visibility.Visible;
            }

            // Hide all children of EmptyState except the headers
            foreach (var child in EmptyState.Children)
            {
                if (child is FrameworkElement element)
                {
                    // Keep the headers visible, hide everything else
                    if (element.Name != "MonitoringTaskHeader" && element.Name != "MonitoringTaskTable")
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void SetAlarmRecordsEmptyState()
        {
            if (EmptyState == null) return;

            // Hide the filter bar for Alarm Records
            if (FilterBar != null)
            {
                FilterBar.Visibility = Visibility.Collapsed;
            }

            // Show the sidebar for Alarm Records
            if (MonitoringTaskSidebar != null)
            {
                MonitoringTaskSidebar.Visibility = Visibility.Visible;
            }

            // Show the Alarm Records filter bar and export button
            if (AlarmRecordsFilterBar != null)
            {
                AlarmRecordsFilterBar.Visibility = Visibility.Visible;
            }

            if (AlarmRecordsMainContent != null)
            {
                AlarmRecordsMainContent.Visibility = Visibility.Visible;
            }

            // Hide all children of EmptyState except the sidebar, filter bar, and main content
            foreach (var child in EmptyState.Children)
            {
                if (child is FrameworkElement element)
                {
                    // Keep the sidebar, filter bar, and main content visible, hide everything else
                    if (element.Name != "MonitoringTaskSidebar" && 
                        element.Name != "AlarmRecordsFilterBar" && 
                        element.Name != "AlarmRecordsMainContent")
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void SetPassThruRecordEmptyState()
        {
            if (EmptyState == null) return;

            // Hide the filter bar for Pass Thru Record
            if (FilterBar != null)
            {
                FilterBar.Visibility = Visibility.Collapsed;
            }

            // Show the sidebar for Pass Thru Record
            if (MonitoringTaskSidebar != null)
            {
                MonitoringTaskSidebar.Visibility = Visibility.Visible;
            }

            // Show the Pass Thru Record filter bar and main content
            if (PassThruRecordFilterBar != null)
            {
                PassThruRecordFilterBar.Visibility = Visibility.Visible;
            }

            if (PassThruRecordMainContent != null)
            {
                PassThruRecordMainContent.Visibility = Visibility.Visible;
            }

            // Hide all children of EmptyState except the sidebar, filter bar, and main content
            foreach (var child in EmptyState.Children)
            {
                if (child is FrameworkElement element)
                {
                    // Keep the sidebar, filter bar, and main content visible, hide everything else
                    if (element.Name != "MonitoringTaskSidebar" && 
                        element.Name != "PassThruRecordFilterBar" && 
                        element.Name != "PassThruRecordMainContent")
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void SetSearchByImageEmptyState()
        {
            if (EmptyState == null) return;

            // Hide the filter bar for Search by Image
            if (FilterBar != null)
            {
                FilterBar.Visibility = Visibility.Collapsed;
            }

            // Show the sidebar for Search by Image
            if (MonitoringTaskSidebar != null)
            {
                MonitoringTaskSidebar.Visibility = Visibility.Visible;
            }

            // Show the Search by Image filter bar and main content
            if (SearchByImageFilterBar != null)
            {
                SearchByImageFilterBar.Visibility = Visibility.Visible;
            }

            if (SearchByImageMainContent != null)
            {
                SearchByImageMainContent.Visibility = Visibility.Visible;
            }

            // Hide all children of EmptyState except the sidebar, filter bar, and main content
            foreach (var child in EmptyState.Children)
            {
                if (child is FrameworkElement element)
                {
                    // Keep the sidebar, filter bar, and main content visible, hide everything else
                    if (element.Name != "MonitoringTaskSidebar" && 
                        element.Name != "SearchByImageFilterBar" && 
                        element.Name != "SearchByImageMainContent")
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        // Helper method to find visual children
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}