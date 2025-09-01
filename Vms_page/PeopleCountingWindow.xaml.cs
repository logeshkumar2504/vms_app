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
            // Open the grid layout selection popup
            var layoutPopup = new GridLayoutPopup();
            layoutPopup.Owner = this;
            
            if (layoutPopup.ShowDialog() == true)
            {
                // Apply the selected layout
                currentLayout = layoutPopup.SelectedLayout;
                ApplyGridLayout(currentLayout);
            }
        }

        private void ApplyGridLayout(string layout)
        {
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

                // Parse layout (e.g., "2x2", "3x3", etc.)
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
            var textBlock = new TextBlock
            {
                Text = "📹",
                FontSize = GetOptimalFontSize(totalRows, totalCols),
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(textBlock);
            border.Child = grid;

            // Add click handler
            border.MouseLeftButtonDown += VideoFeed_Click;

            return border;
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
