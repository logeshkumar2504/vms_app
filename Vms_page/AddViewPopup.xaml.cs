using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Vms_page
{
    public partial class AddViewPopup : Window
    {
        private int selectedGridRows = 1;
        private int selectedGridColumns = 1;

        public AddViewPopup()
        {
            InitializeComponent();
            SetupPlaceholderText();
            CreateGridPreview(1, 1);
        }

        private void CreateGridPreview(int rows, int columns)
        {
            // Clear existing content
            GridPreviewContainer.Children.Clear();
            GridPreviewContainer.RowDefinitions.Clear();
            GridPreviewContainer.ColumnDefinitions.Clear();

            // Create grid structure
            for (int i = 0; i < rows; i++)
            {
                GridPreviewContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                GridPreviewContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create grid cells
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cell = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(30, 35, 48)), // #1E2330
                        BorderBrush = new SolidColorBrush(Color.FromRgb(58, 63, 74)), // #3A3F4A
                        BorderThickness = new Thickness(1),
                        Margin = new Thickness(2),
                        CornerRadius = new CornerRadius(0),
                        Cursor = System.Windows.Input.Cursors.Hand,
                        ToolTip = $"Click to configure Cell {i + 1}-{j + 1}\nGrid Position: Row {i + 1}, Column {j + 1}"
                    };

                    // Add cell number for identification
                    var cellText = new TextBlock
                    {
                        Text = $"Cell {i + 1}-{j + 1}",
                        Foreground = new SolidColorBrush(Color.FromRgb(176, 190, 197)), // #B0BEC5
                        FontSize = 10,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Opacity = 0.8
                    };

                    // Add a subtle icon to make cells more visually appealing
                    var cellContent = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    // Add a small video camera icon
                    var iconText = new TextBlock
                    {
                        Text = "📹",
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 4)
                    };

                    cellContent.Children.Add(iconText);
                    cellContent.Children.Add(cellText);

                    cell.Child = cellContent;
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                    GridPreviewContainer.Children.Add(cell);
                }
            }
        }

        private void SetupPlaceholderText()
        {
            // Setup SearchBox placeholder
            SearchBox.GotFocus += (s, e) =>
            {
                if (SearchBox.Text == "Search cameras...")
                {
                    SearchBox.Text = "";
                    SearchBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            SearchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    SearchBox.Text = "Search cameras...";
                    SearchBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup CameraNameBox placeholder
            CameraNameBox.GotFocus += (s, e) =>
            {
                if (CameraNameBox.Text == "Camera Name")
                {
                    CameraNameBox.Text = "";
                    CameraNameBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            CameraNameBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(CameraNameBox.Text))
                {
                    CameraNameBox.Text = "Camera Name";
                    CameraNameBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup CameraUrlBox placeholder
            CameraUrlBox.GotFocus += (s, e) =>
            {
                if (CameraUrlBox.Text == "Camera URL")
                {
                    CameraUrlBox.Text = "";
                    CameraUrlBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            CameraUrlBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(CameraUrlBox.Text))
                {
                    CameraUrlBox.Text = "Camera URL";
                    CameraUrlBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup CameraTypeCombo with default options
            CameraTypeCombo.Items.Add("IP Camera");
            CameraTypeCombo.Items.Add("USB Camera");
            CameraTypeCombo.Items.Add("RTSP Stream");
            CameraTypeCombo.SelectedIndex = 0;
        }

        // Grid button click handlers
        private void Grid1x1Btn_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 1;
            selectedGridColumns = 1;
            CreateGridPreview(1, 1);
        }

        private void Grid2x2Btn_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 2;
            selectedGridColumns = 2;
            CreateGridPreview(2, 2);
        }

        private void Grid3x3Btn_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 3;
            selectedGridColumns = 3;
            CreateGridPreview(3, 3);
        }

        private void Grid4x4Btn_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 4;
            selectedGridColumns = 4;
            CreateGridPreview(4, 4);
        }

        private void GridCustomBtn_Click(object sender, RoutedEventArgs e)
        {
            // Show custom grid layout popup
            var customGridPopup = new CustomGridLayoutPopup();
            if (customGridPopup.ShowDialog() == true)
            {
                selectedGridRows = customGridPopup.SelectedRows;
                selectedGridColumns = customGridPopup.SelectedColumns;
                CreateGridPreview(selectedGridRows, selectedGridColumns);
            }
        }

        private void AddCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            // Add camera logic here
            MessageBox.Show("Add Camera functionality will be implemented here.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save view logic here
            MessageBox.Show("Save View functionality will be implemented here.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
