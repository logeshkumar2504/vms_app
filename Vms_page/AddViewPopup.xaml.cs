using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vms_page
{
    public partial class AddViewPopup : Window
    {
        private string currentLayout = "2x2";

        public AddViewPopup()
        {
            InitializeComponent();
            SetupPlaceholderText();
        }

        private void SetupPlaceholderText()
        {
            // Setup NameInputBox placeholder
            NameInputBox.GotFocus += (s, e) =>
            {
                if (NameInputBox.Text == "Enter view name...")
                {
                    NameInputBox.Text = "";
                    NameInputBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            NameInputBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(NameInputBox.Text))
                {
                    NameInputBox.Text = "Enter view name...";
                    NameInputBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup VideoChannelSearchBox placeholder
            VideoChannelSearchBox.GotFocus += (s, e) =>
            {
                if (VideoChannelSearchBox.Text == "Enter Keywords")
                {
                    VideoChannelSearchBox.Text = "";
                    VideoChannelSearchBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            VideoChannelSearchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(VideoChannelSearchBox.Text))
                {
                    VideoChannelSearchBox.Text = "Enter Keywords";
                    VideoChannelSearchBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup SequenceResourceSearchBox placeholder
            SequenceResourceSearchBox.GotFocus += (s, e) =>
            {
                if (SequenceResourceSearchBox.Text == "Enter Keywords")
                {
                    SequenceResourceSearchBox.Text = "";
                    SequenceResourceSearchBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            SequenceResourceSearchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(SequenceResourceSearchBox.Text))
                {
                    SequenceResourceSearchBox.Text = "Enter Keywords";
                    SequenceResourceSearchBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Setup RemarksTextBox placeholder
            RemarksTextBox.GotFocus += (s, e) =>
            {
                if (RemarksTextBox.Text == "Enter remarks...")
                {
                    RemarksTextBox.Text = "";
                    RemarksTextBox.Foreground = System.Windows.Media.Brushes.White;
                }
            };

            RemarksTextBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(RemarksTextBox.Text))
                {
                    RemarksTextBox.Text = "Enter remarks...";
                    RemarksTextBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };
        }

        private void SelectLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string layout)
            {
                currentLayout = layout;
                UpdateCameraGrid(layout);
            }
        }

        private void UpdateCameraGrid(string layout)
        {
            // Clear existing grid
            CameraPreviewGrid.Children.Clear();
            CameraPreviewGrid.RowDefinitions.Clear();
            CameraPreviewGrid.ColumnDefinitions.Clear();

            // Parse layout (e.g., "2x2", "3x3", "4x4")
            var parts = layout.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int rows) && int.TryParse(parts[1], out int cols))
            {
                // Create grid structure
                for (int i = 0; i < rows; i++)
                {
                    CameraPreviewGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                for (int j = 0; j < cols; j++)
                {
                    CameraPreviewGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }

                // Create camera cells
                int cameraNumber = 1;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        var cell = new Border
                        {
                            Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)), // #1A1A1A
                            BorderBrush = new SolidColorBrush(Color.FromRgb(64, 64, 64)), // #404040
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(2),
                            CornerRadius = new CornerRadius(2)
                        };

                        var image = new Image
                        {
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Assets/Icons/cctv.png")),
                            Width = 28,
                            Height = 28,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Opacity = 0.9,
                            Stretch = Stretch.Uniform
                        };

                        cell.Child = image;
                        Grid.SetRow(cell, i);
                        Grid.SetColumn(cell, j);
                        CameraPreviewGrid.Children.Add(cell);
                        cameraNumber++;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save view logic here
            string viewName = NameInputBox.Text == "Enter view name..." ? "" : NameInputBox.Text;
            string remarks = RemarksTextBox.Text == "Enter remarks..." ? "" : RemarksTextBox.Text;
            MessageBox.Show($"Save View functionality will be implemented here.\nView Name: {viewName}\nLayout: {currentLayout}\nRemarks: {remarks}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
