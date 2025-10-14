using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vms_page
{
    public partial class LiveViewWindow : Window
    {
        private bool isVideoTabActive = true;
        private bool isSearchFocused = false;
        private string currentLayout = "2x2";
        private bool isFullScreen = false;
        private WindowState previousWindowState;
        private WindowStyle previousWindowStyle;
        private bool previousTopmost;

        public LiveViewWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            
            // Add key down event handler for escape key
            this.KeyDown += LiveViewWindow_KeyDown;
            this.Focusable = true;
            
            // Initialize the grid layout with default 2x2 layout
            this.Loaded += (s, e) => ApplyGridLayout(currentLayout);
        }

        private void VideoTab_Click(object sender, MouseButtonEventArgs e)
        {
            if (!isVideoTabActive)
            {
                // Switch to Video tab
                isVideoTabActive = true;
                
                // Update tab appearances
                var videoTab = sender as Border;
                var parentGrid = videoTab.Parent as Grid;
                var channelTab = parentGrid.Children[1] as Border;
                var viewTab = parentGrid.Children[2] as Border;
                
                videoTab.Background = Application.Current.Resources["MenuHoverColor"] as SolidColorBrush;
                channelTab.Background = Brushes.Transparent;
                viewTab.Background = Brushes.Transparent;
                
                // Update text colors
                var videoText = videoTab.Child as TextBlock;
                var channelText = channelTab.Child as TextBlock;
                var viewText = viewTab.Child as TextBlock;
                
                videoText.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                channelText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
                viewText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
            }
        }

        private void ChannelTab_Click(object sender, MouseButtonEventArgs e)
        {
            if (isVideoTabActive)
            {
                // Switch to Channel tab
                isVideoTabActive = false;
                
                // Update tab appearances
                var channelTab = sender as Border;
                var parentGrid = channelTab.Parent as Grid;
                var videoTab = parentGrid.Children[0] as Border;
                var viewTab = parentGrid.Children[2] as Border;
                
                channelTab.Background = Application.Current.Resources["MenuHoverColor"] as SolidColorBrush;
                videoTab.Background = Brushes.Transparent;
                viewTab.Background = Brushes.Transparent;
                
                // Update text colors
                var channelText = channelTab.Child as TextBlock;
                var videoText = videoTab.Child as TextBlock;
                var viewText = viewTab.Child as TextBlock;
                
                channelText.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                videoText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
                viewText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
            }
        }

        private void ViewTab_Click(object sender, MouseButtonEventArgs e)
        {
            if (isVideoTabActive)
            {
                // Switch to View tab
                isVideoTabActive = false;
                
                // Update tab appearances
                var viewTab = sender as Border;
                var parentGrid = viewTab.Parent as Grid;
                var videoTab = parentGrid.Children[0] as Border;
                var channelTab = parentGrid.Children[1] as Border;
                
                viewTab.Background = Application.Current.Resources["MenuHoverColor"] as SolidColorBrush;
                videoTab.Background = Brushes.Transparent;
                channelTab.Background = Brushes.Transparent;
                
                // Update text colors
                var viewText = viewTab.Child as TextBlock;
                var videoText = videoTab.Child as TextBlock;
                var channelText = channelTab.Child as TextBlock;
                
                viewText.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                videoText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
                channelText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
            }
        }



        private void VideoFeed_Click(object sender, MouseButtonEventArgs e)
        {
            // Clear previous selection
            ClearVideoFeedSelection();
            
            // Set new selection
            if (sender is Border border)
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)); // Orange border
            }
        }

        private void ClearVideoFeedSelection()
        {
            // Clear all video feed selections
            var mainGrid = FindName("MainVideoGrid") as Grid;
            if (mainGrid != null)
            {
                foreach (var child in mainGrid.Children)
                {
                    if (child is Border border)
                    {
                        // Use SetResourceReference to bind to dynamic resource
                        border.SetResourceReference(Border.BorderBrushProperty, "BorderColor");
                    }
                }
            }
        }

        private void GridLayout_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the dropdown popup
            GridLayoutDropdownPopup.IsOpen = !GridLayoutDropdownPopup.IsOpen;
        }

        private void SelectLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string layout)
            {
                // Apply the selected layout
                currentLayout = layout;
                System.Diagnostics.Debug.WriteLine($"Layout selected: {currentLayout}");
                
                // Close the dropdown
                GridLayoutDropdownPopup.IsOpen = false;
                
                // Apply the layout
                ApplyGridLayout(currentLayout);
            }
        }

        private void CustomLayout_Click(object sender, RoutedEventArgs e)
        {
            // Close the dropdown first
            GridLayoutDropdownPopup.IsOpen = false;
            
            // Open the custom layout designer popup directly
            var customLayoutPopup = new PeopleCountingCustomLayoutPopup();
            customLayoutPopup.Owner = this;
            
            if (customLayoutPopup.ShowDialog() == true)
            {
                // Apply the custom layout
                currentLayout = customLayoutPopup.CustomLayout;
                System.Diagnostics.Debug.WriteLine($"Custom layout popup returned: {currentLayout}");
                
                // Apply the layout
                ApplyGridLayout(currentLayout);
            }
        }

        private void ApplyGridLayout(string layout)
        {
            // Debug: Show what layout is being applied
            System.Diagnostics.Debug.WriteLine($"Applying Layout: {layout}");
            
            // Find the main grid container
            var mainGrid = FindName("MainVideoGrid") as Grid;
            if (mainGrid == null)
            {
                // If we can't find the named grid, find it by traversing the visual tree
                mainGrid = FindVideoGrid(this);
            }

            if (mainGrid != null)
            {
                // Clear existing children
                mainGrid.Children.Clear();
                mainGrid.RowDefinitions.Clear();
                mainGrid.ColumnDefinitions.Clear();

                // Check if it's a custom layout with combined cells
                if (layout.StartsWith("custom_") && layout.Contains("_combined"))
                {
                    System.Diagnostics.Debug.WriteLine("Applying custom combined layout");
                    ApplyCustomCombinedLayout(mainGrid, layout);
                }
                else if (layout.StartsWith("custom_") && !layout.Contains("_combined"))
                {
                    System.Diagnostics.Debug.WriteLine("Applying custom regular layout");
                    ApplyCustomRegularLayout(mainGrid, layout);
                }
                else
                {
                    // Parse regular layout (e.g., "2x2", "3x3", etc.)
                    var parts = layout.Split('x');
                    if (parts.Length == 2 && 
                        int.TryParse(parts[0], out int rows) && 
                        int.TryParse(parts[1], out int cols))
                    {
                        // Create row definitions
                        for (int i = 0; i < rows; i++)
                        {
                            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        }

                        // Create column definitions
                        for (int i = 0; i < cols; i++)
                        {
                            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        }

                        // Create video feed borders
                        for (int row = 0; row < rows; row++)
                        {
                            for (int col = 0; col < cols; col++)
                            {
                                var border = CreateVideoFeedBorder(row, col, rows, cols);
                                Grid.SetRow(border, row);
                                Grid.SetColumn(border, col);
                                mainGrid.Children.Add(border);
                            }
                        }
                    }
                }
            }
        }

        private void ApplyCustomRegularLayout(Grid mainGrid, string layout)
        {
            System.Diagnostics.Debug.WriteLine($"Applying custom regular layout: {layout}");
            
            // Parse custom layout: custom_4x4, custom_3x5, custom_6x2, etc. - works for ANY size
            var parts = layout.Split('_');
            if (parts.Length >= 2)
            {
                // Get grid dimensions - flexible parsing
                var dimensions = parts[1].Split('x');
                if (dimensions.Length == 2 && 
                    int.TryParse(dimensions[0], out int rows) && 
                    int.TryParse(dimensions[1], out int cols) &&
                    rows > 0 && cols > 0) // Validate dimensions
                {
                    System.Diagnostics.Debug.WriteLine($"Creating {rows}x{cols} regular grid (any size supported)");
                    
                    // Create row definitions - works for any number of rows
                    for (int i = 0; i < rows; i++)
                    {
                        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    }

                    // Create column definitions - works for any number of columns
                    for (int i = 0; i < cols; i++)
                    {
                        mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    // Create individual cells - works for any grid size
                    for (int row = 0; row < rows; row++)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            var border = CreateVideoFeedBorder(row, col, rows, cols);
                            Grid.SetRow(border, row);
                            Grid.SetColumn(border, col);
                            mainGrid.Children.Add(border);
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid grid dimensions in layout: {layout}");
                    ApplyFallbackLayout(mainGrid);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Invalid layout format: {layout}");
                ApplyFallbackLayout(mainGrid);
            }
        }

        private void ApplyCustomCombinedLayout(Grid mainGrid, string layout)
        {
            System.Diagnostics.Debug.WriteLine($"Applying custom combined layout: {layout}");
            
            // Parse custom layout: custom_4x4_combined_0_0_2_2_2_2_1_1 - works for ANY grid size and ANY combinations
            var parts = layout.Split('_');
            if (parts.Length >= 3)
            {
                // Get grid dimensions - flexible for any size
                var dimensions = parts[1].Split('x');
                if (dimensions.Length == 2 && 
                    int.TryParse(dimensions[0], out int rows) && 
                    int.TryParse(dimensions[1], out int cols) &&
                    rows > 0 && cols > 0) // Validate dimensions
                {
                    System.Diagnostics.Debug.WriteLine($"Creating {rows}x{cols} grid with combined cells (any size supported)");
                    
                    // Create row definitions - works for any number of rows
                    for (int i = 0; i < rows; i++)
                    {
                        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    }

                    // Create column definitions - works for any number of columns
                    for (int i = 0; i < cols; i++)
                    {
                        mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    // Create a grid to track which cells are occupied
                    var occupied = new bool[rows, cols];

                    // Parse combined cells - works for any number of combinations
                    if (parts.Length > 3 && parts[2] == "combined")
                    {
                        for (int i = 3; i < parts.Length; i += 4)
                        {
                            if (i + 3 < parts.Length &&
                                int.TryParse(parts[i], out int startRow) &&
                                int.TryParse(parts[i + 1], out int startCol) &&
                                int.TryParse(parts[i + 2], out int rowSpan) &&
                                int.TryParse(parts[i + 3], out int colSpan) &&
                                startRow >= 0 && startCol >= 0 && 
                                startRow < rows && startCol < cols &&
                                rowSpan > 0 && colSpan > 0 &&
                                startRow + rowSpan <= rows && startCol + colSpan <= cols) // Validate bounds
                            {
                                System.Diagnostics.Debug.WriteLine($"Creating combined cell: {startRow},{startCol} span {rowSpan}x{colSpan}");
                                
                                // Mark cells as occupied
                                for (int r = startRow; r < startRow + rowSpan && r < rows; r++)
                                {
                                    for (int c = startCol; c < startCol + colSpan && c < cols; c++)
                                    {
                                        occupied[r, c] = true;
                                    }
                                }

                                // Create combined cell
                                var border = CreateCombinedVideoFeedBorder(startRow, startCol, rowSpan, colSpan);
                                Grid.SetRow(border, startRow);
                                Grid.SetColumn(border, startCol);
                                Grid.SetRowSpan(border, rowSpan);
                                Grid.SetColumnSpan(border, colSpan);
                                mainGrid.Children.Add(border);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Invalid combined cell parameters at index {i}");
                            }
                        }
                    }

                    // Create individual cells for unoccupied positions
                    for (int row = 0; row < rows; row++)
                    {
                        for (int col = 0; col < cols; col++)
                        {
                            if (!occupied[row, col])
                            {
                                var border = CreateVideoFeedBorder(row, col, rows, cols);
                                Grid.SetRow(border, row);
                                Grid.SetColumn(border, col);
                                mainGrid.Children.Add(border);
                            }
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid grid dimensions in combined layout: {layout}");
                    ApplyFallbackLayout(mainGrid);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Invalid combined layout format: {layout}");
                ApplyFallbackLayout(mainGrid);
            }
        }

        private void ApplyFallbackLayout(Grid mainGrid)
        {
            System.Diagnostics.Debug.WriteLine("Applying fallback 2x2 layout");
            
            // Create a simple 2x2 grid as fallback
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Create 4 regular video cells
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    var border = CreateVideoFeedBorder(row, col, 2, 2);
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                    mainGrid.Children.Add(border);
                }
            }
        }

        private Border CreateCombinedVideoFeedBorder(int row, int col, int rowSpan, int colSpan)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)), // Orange border for combined cells
                CornerRadius = new CornerRadius(0),
                Margin = new Thickness(1),
                Cursor = Cursors.Hand
            };

            // Bind to dynamic resources instead of capturing static values
            border.SetResourceReference(Border.BackgroundProperty, "SurfaceColor");

            var grid = new Grid();
            
            // Create CCTV Camera Icon using cctv.png for combined cells
            var cameraIcon = CreateCCTVCameraIcon(rowSpan, colSpan);
            grid.Children.Add(cameraIcon);
            
            border.Child = grid;

            // Add click handler
            border.MouseLeftButtonDown += VideoFeed_Click;

            return border;
        }

        private Grid FindVideoGrid(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Grid grid && grid.Children.Count > 0)
                {
                    // Check if this grid contains video feeds (empty grids with borders)
                    var hasVideoFeeds = false;
                    foreach (var childElement in grid.Children)
                    {
                        if (childElement is Border border && border.Child is Grid innerGrid)
                        {
                            // Check if this looks like a video feed cell (has the right background color)
                            if (border.Background is SolidColorBrush brush && 
                                brush.Color.R == 0x1A && brush.Color.G == 0x1A && brush.Color.B == 0x1A)
                            {
                                hasVideoFeeds = true;
                                break;
                            }
                        }
                        if (hasVideoFeeds) break;
                    }
                    if (hasVideoFeeds) return grid;
                }
                
                var result = FindVideoGrid(child);
                if (result != null) return result;
            }
            return null;
        }

        private Border CreateVideoFeedBorder(int row, int col, int totalRows, int totalCols)
        {
            var border = new Border
            {
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(0),
                Margin = new Thickness(0),
                Cursor = Cursors.Hand
            };

            // Bind to dynamic resources instead of capturing static values
            border.SetResourceReference(Border.BackgroundProperty, "SurfaceColor");
            border.SetResourceReference(Border.BorderBrushProperty, "BorderColor");

            // Set orange border for the last cell (bottom-right equivalent)
            if (row == totalRows - 1 && col == totalCols - 1)
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                border.Name = "SelectedFeed";
            }

            var grid = new Grid();
            
            // Create CCTV Camera Icon using cctv.png
            var cameraIcon = CreateCCTVCameraIcon(totalRows, totalCols);
            grid.Children.Add(cameraIcon);
            
            border.Child = grid;

            // Add click handler
            border.MouseLeftButtonDown += VideoFeed_Click;

            return border;
        }

        private Border CreateCCTVCameraIcon(int totalRows, int totalCols)
        {
            var iconSize = GetOptimalIconSize(totalRows, totalCols);
            
            var cameraBorder = new Border
            {
                Width = iconSize,
                Height = iconSize,
                Background = Brushes.Transparent
            };

            // Create Image control for cctv.png
            var cameraImage = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Assets/Icons/cctv.png")),
                Width = iconSize,
                Height = iconSize,
                Stretch = System.Windows.Media.Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            
            cameraBorder.Child = cameraImage;
            return cameraBorder;
        }

        private double GetOptimalIconSize(int rows, int cols)
        {
            var totalCells = rows * cols;
            if (totalCells <= 1) return 80;
            if (totalCells <= 4) return 60;
            if (totalCells <= 9) return 40;
            if (totalCells <= 16) return 30;
            return 20;
        }

        private double GetOptimalFontSize(int rows, int cols)
        {
            // Calculate optimal font size based on grid size
            var totalCells = rows * cols;
            if (totalCells <= 1) return 60;
            if (totalCells <= 4) return 40;
            if (totalCells <= 9) return 25;
            if (totalCells <= 16) return 18;
            return 12;
        }



        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!isSearchFocused)
            {
                isSearchFocused = true;
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                isSearchFocused = false;
                SearchTextBox.Text = "Enter Keywords";
                SearchTextBox.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
            }
        }

        private void AddIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Open Sequence Resource Info popup as modal dialog
            var infoPopup = new AddSequenceResourcePopup();
            infoPopup.Owner = this;
            infoPopup.ShowInTaskbar = false;
            infoPopup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            infoPopup.ShowDialog();
        }

        private void EditIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle edit icon click
            MessageBox.Show("Edit icon clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle delete icon click
            MessageBox.Show("Delete icon clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VideoIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle video icon click
            MessageBox.Show("Video icon clicked", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void ViewIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle view icon click
            MessageBox.Show("View icon clicked", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SequenceResourcesIcon_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle sequence resources icon click
            MessageBox.Show("Sequence Resources icon clicked", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VideoIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            VideoLabel.Opacity = 1;
        }

        private void VideoIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            VideoLabel.Opacity = 0;
        }



        private void ViewIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            ViewLabel.Opacity = 1;
        }

        private void ViewIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            ViewLabel.Opacity = 0;
        }

        private void SequenceResourcesIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            SequenceResourcesLabel.Opacity = 1;
        }

        private void SequenceResourcesIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            SequenceResourcesLabel.Opacity = 0;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle previous button click
            MessageBox.Show("Previous button clicked", "Media Control", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle play/pause button click
            var button = sender as Button;
            if (button != null)
            {
                // Toggle between play and pause icons
                if (button.Content.ToString() == "▶")
                {
                    button.Content = "⏸";
                    MessageBox.Show("Video paused", "Media Control", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    button.Content = "▶";
                    MessageBox.Show("Video playing", "Media Control", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle next button click
            MessageBox.Show("Next button clicked", "Media Control", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // New event handlers for bottom bar icons
        private void SaveViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle save view button click
            MessageBox.Show("Save View clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseAllWindowsButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle close all windows button click
            MessageBox.Show("Close All Windows clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SnapshotAllButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle snapshot all button click
            MessageBox.Show("Snapshot All clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StartRecordingAllButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle start recording all button click
            MessageBox.Show("Start Recording All clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StopRecordingAllButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle stop recording all button click
            MessageBox.Show("Stop Recording All clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BroadcastListButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle broadcast list button click
            MessageBox.Show("Broadcast List clicked", "Action", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LeftIconButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle left icon button click
            MessageBox.Show("Left icon button clicked", "Navigation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleFullScreen();
        }

        private void LiveViewWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && isFullScreen)
            {
                ExitFullScreen();
                e.Handled = true;
            }
        }

        private void ToggleFullScreen()
        {
            if (!isFullScreen)
            {
                EnterFullScreen();
            }
            else
            {
                ExitFullScreen();
            }
        }

        private void EnterFullScreen()
        {
            // Store current window properties
            previousWindowState = this.WindowState;
            previousWindowStyle = this.WindowStyle;
            previousTopmost = this.Topmost;

            // Hide sidebar
            var sidebar = this.FindName("Sidebar") as Border;
            if (sidebar != null)
            {
                sidebar.Visibility = Visibility.Collapsed;
            }

            // Make the main content area span the full width by setting the first column width to 0
            var mainGrid = this.Content as Grid;
            if (mainGrid != null && mainGrid.ColumnDefinitions.Count >= 2)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(0);
            }

            // Hide the bottom controls by finding the VideoView grid and hiding its bottom row
            var videoView = this.FindName("VideoView") as Grid;
            if (videoView != null && videoView.RowDefinitions.Count > 1)
            {
                // Store the original height of the bottom row
                videoView.RowDefinitions[1].Height = new GridLength(0);
            }

            // Set window to full screen
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.Topmost = true;
            this.isFullScreen = true;

            // Update button icon to show exit full screen
            if (FullScreenButton != null)
            {
                FullScreenButton.Content = "⛶";
                FullScreenButton.ToolTip = "Exit Full Screen";
            }

            // Focus the window to receive key events
            this.Focus();
        }

        private void ExitFullScreen()
        {
            // Restore window properties
            this.WindowStyle = previousWindowStyle;
            this.WindowState = previousWindowState;
            this.Topmost = previousTopmost;
            this.isFullScreen = false;

            // Show sidebar
            var sidebar = this.FindName("Sidebar") as Border;
            if (sidebar != null)
            {
                sidebar.Visibility = Visibility.Visible;
            }

            // Restore the main content area column width
            var mainGrid = this.Content as Grid;
            if (mainGrid != null && mainGrid.ColumnDefinitions.Count >= 2)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(150); // Restore original sidebar width
            }

            // Show the bottom controls by restoring the VideoView grid bottom row
            var videoView = this.FindName("VideoView") as Grid;
            if (videoView != null && videoView.RowDefinitions.Count > 1)
            {
                // Restore the bottom row height
                videoView.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);
            }

            // Update button icon to show enter full screen
            if (FullScreenButton != null)
            {
                FullScreenButton.Content = "⛶";
                FullScreenButton.ToolTip = "Full Screen";
            }
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

    }
}
