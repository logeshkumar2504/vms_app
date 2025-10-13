using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vms_page
{
    public partial class PeopleCountingWindow : Window
    {
        private bool isRealtimeTabActive = true;
        private bool isSearchFocused = false;
        private string currentLayout = "2x2";

        public PeopleCountingWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
        }

        private void RealtimeTab_Click(object sender, MouseButtonEventArgs e)
        {
            if (!isRealtimeTabActive)
            {
                // Switch to Realtime Statistics tab
                isRealtimeTabActive = true;
                
                // Update tab appearances
                var realtimeTab = sender as Border;
                var reportTab = realtimeTab.Parent as Grid;
                var reportTabBorder = reportTab.Children[1] as Border;
                
                realtimeTab.Background = Application.Current.Resources["MenuHoverColor"] as SolidColorBrush;
                reportTabBorder.Background = Brushes.Transparent;
                
                // Update text colors
                var realtimeText = realtimeTab.Child as TextBlock;
                var reportText = reportTabBorder.Child as TextBlock;
                
                realtimeText.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                reportText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
                
                // Switch content views
                var realtimeView = FindName("RealtimeView") as Grid;
                var reportView = FindName("ReportView") as Grid;
                
                if (realtimeView != null) realtimeView.Visibility = Visibility.Visible;
                if (reportView != null) reportView.Visibility = Visibility.Collapsed;
            }
        }

        private void ReportTab_Click(object sender, MouseButtonEventArgs e)
        {
            if (isRealtimeTabActive)
            {
                // Switch to Report Statistics tab
                isRealtimeTabActive = false;
                
                // Update tab appearances
                var reportTab = sender as Border;
                var realtimeTab = reportTab.Parent as Grid;
                var realtimeTabBorder = realtimeTab.Children[0] as Border;
                
                reportTab.Background = Application.Current.Resources["MenuHoverColor"] as SolidColorBrush;
                realtimeTabBorder.Background = Brushes.Transparent;
                
                // Update text colors
                var reportText = reportTab.Child as TextBlock;
                var realtimeText = realtimeTabBorder.Child as TextBlock;
                
                reportText.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                realtimeText.Foreground = Application.Current.Resources["TextSecondaryColor"] as SolidColorBrush;
                
                // Switch content views
                var realtimeView = FindName("RealtimeView") as Grid;
                var reportView = FindName("ReportView") as Grid;
                
                if (realtimeView != null) realtimeView.Visibility = Visibility.Collapsed;
                if (reportView != null) reportView.Visibility = Visibility.Visible;
            }
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

        private void PeopleFlow_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle People Flow Counting selection
            MessageBox.Show("People Flow Counting selected", "Selection", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PeopleDensity_Click(object sender, MouseButtonEventArgs e)
        {
            // Handle People Density Monitoring selection
            MessageBox.Show("People Density Monitoring selected", "Selection", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)); // Gray border
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
                else if (layout == "custom_image_layout")
                {
                    System.Diagnostics.Debug.WriteLine("Applying custom image layout");
                    try
                    {
                        ApplyImageLayout(mainGrid);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error applying image layout: {ex.Message}");
                        // Fallback to 2x2 layout
                        ApplyFallbackLayout(mainGrid);
                    }
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

        private void ApplyImageLayout(Grid mainGrid)
        {
            System.Diagnostics.Debug.WriteLine("ApplyImageLayout called - creating 2x2 grid with sub-grids");
            
            // Create a 2x2 main grid like in the image
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Top-left cell with 2x2 sub-grid (4 small cells)
            var topLeftContainer = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(1),
                Cursor = Cursors.Hand
            };

            var topLeftGrid = new Grid();
            for (int i = 0; i < 2; i++)
            {
                topLeftGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                topLeftGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create 4 small cells in the top-left
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    var smallCell = CreateSmallVideoCell();
                    Grid.SetRow(smallCell, row);
                    Grid.SetColumn(smallCell, col);
                    topLeftGrid.Children.Add(smallCell);
                }
            }
            topLeftContainer.Child = topLeftGrid;
            Grid.SetRow(topLeftContainer, 0);
            Grid.SetColumn(topLeftContainer, 0);
            mainGrid.Children.Add(topLeftContainer);

            // Top-right cell with 1x2 sub-grid (2 small cells)
            var topRightContainer = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(1),
                Cursor = Cursors.Hand
            };

            var topRightGrid = new Grid();
            topRightGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            topRightGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            topRightGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Create 2 small cells in the top-right
            for (int row = 0; row < 2; row++)
            {
                var smallCell = CreateSmallVideoCell();
                Grid.SetRow(smallCell, row);
                Grid.SetColumn(smallCell, 0);
                topRightGrid.Children.Add(smallCell);
            }
            topRightContainer.Child = topRightGrid;
            Grid.SetRow(topRightContainer, 0);
            Grid.SetColumn(topRightContainer, 1);
            mainGrid.Children.Add(topRightContainer);

            // Bottom-left cell (regular)
            var bottomLeftCell = CreateVideoFeedBorder(1, 0, 2, 2);
            Grid.SetRow(bottomLeftCell, 1);
            Grid.SetColumn(bottomLeftCell, 0);
            mainGrid.Children.Add(bottomLeftCell);

            // Bottom-right cell (regular)
            var bottomRightCell = CreateVideoFeedBorder(1, 1, 2, 2);
            Grid.SetRow(bottomRightCell, 1);
            Grid.SetColumn(bottomRightCell, 1);
            mainGrid.Children.Add(bottomRightCell);
        }

        private Border CreateSmallVideoCell()
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(0.5),
                Margin = new Thickness(0.5),
                Cursor = Cursors.Hand
            };

            var grid = new Grid();
            
            // Create Image control for cctv.png
            var cameraImage = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Assets/Icons/cctv.png")),
                Width = 16,
                Height = 16,
                Stretch = System.Windows.Media.Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(cameraImage);
            border.Child = grid;

            // Add click handler
            border.MouseLeftButtonDown += VideoFeed_Click;

            return border;
        }

        private Border CreateCombinedVideoFeedBorder(int row, int col, int rowSpan, int colSpan)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                BorderThickness = new Thickness(2),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)), // Orange border for combined cells
                CornerRadius = new CornerRadius(0),
                Margin = new Thickness(1),
                Cursor = Cursors.Hand
            };

            var grid = new Grid();
            
            // Create Camera Icon using cctv.png for combined cells
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
                    // Check if this grid contains video feeds
                    var hasVideoFeeds = false;
                    foreach (var childElement in grid.Children)
                    {
                        if (childElement is Border border && border.Child is Grid innerGrid)
                        {
                            foreach (var innerChild in innerGrid.Children)
                            {
                                if (innerChild is TextBlock textBlock && textBlock.Text == "📹")
                                {
                                    hasVideoFeeds = true;
                                    break;
                                }
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
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                CornerRadius = new CornerRadius(0),
                Margin = new Thickness(0),
                Cursor = Cursors.Hand
            };

            // Set orange border for the last cell (bottom-right equivalent)
            if (row == totalRows - 1 && col == totalCols - 1)
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                border.Name = "SelectedFeed";
            }

            var grid = new Grid();
            
            // Create Camera Icon using cctv.png
            var cameraIcon = CreateCCTVCameraIcon(totalRows, totalCols);
            grid.Children.Add(cameraIcon);
            
            border.Child = grid;

            // Add click handler
            border.MouseLeftButtonDown += VideoFeed_Click;

            return border;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Report Statistics Event Handlers
        private void Today_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Today selected", "Date Range", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Last7Days_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Last 7 days selected", "Date Range", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Last30Days_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Last 30 days selected", "Date Range", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Count_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Count button clicked", "Report Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Export button clicked", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
