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
            UpdateButtonStates();
        }

        private void VideoChannelIcon_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement video channel selection dialog
            MessageBox.Show("Video Channel Selection", "Select Channel", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddResource_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement add resource functionality
            MessageBox.Show("Add Resource", "Add Sequence Resource", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditResource_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement edit resource functionality
            MessageBox.Show("Edit Resource", "Edit Sequence Resource", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement delete resource functionality
            var result = MessageBox.Show("Are you sure you want to delete this resource?", "Delete Resource", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Resource deleted successfully", "Delete Resource", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Handle input changes if needed
        }

        private void SingleGridButton_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 1;
            selectedGridColumns = 1;
            UpdateButtonStates();
        }

        private void Grid2x2Button_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 2;
            selectedGridColumns = 2;
            UpdateButtonStates();
        }

        private void Grid3x3Button_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 3;
            selectedGridColumns = 3;
            UpdateButtonStates();
        }

        private void Grid4x4Button_Click(object sender, RoutedEventArgs e)
        {
            selectedGridRows = 4;
            selectedGridColumns = 4;
            UpdateButtonStates();
        }

        private void CustomGridButton_Click(object sender, RoutedEventArgs e)
        {
            // Show custom grid input dialog
            var customGridDialog = new CustomGridDialog();
            if (customGridDialog.ShowDialog() == true)
            {
                selectedGridRows = customGridDialog.SelectedRows;
                selectedGridColumns = customGridDialog.SelectedColumns;
                UpdateButtonStates();
            }
        }

        private void UpdateButtonStates()
        {
            // Reset all button styles
            SingleGridButton.Style = FindResource("GridLayoutButtonStyle") as Style;
            Grid2x2Button.Style = FindResource("GridLayoutButtonStyle") as Style;
            Grid3x3Button.Style = FindResource("GridLayoutButtonStyle") as Style;
            Grid4x4Button.Style = FindResource("GridLayoutButtonStyle") as Style;
            CustomGridButton.Style = FindResource("GridLayoutButtonStyle") as Style;

            // Highlight selected button
            if (selectedGridRows == 1 && selectedGridColumns == 1)
            {
                SingleGridButton.Style = FindResource("ActionButtonStyle") as Style;
            }
            else if (selectedGridRows == 2 && selectedGridColumns == 2)
            {
                Grid2x2Button.Style = FindResource("ActionButtonStyle") as Style;
            }
            else if (selectedGridRows == 3 && selectedGridColumns == 3)
            {
                Grid3x3Button.Style = FindResource("ActionButtonStyle") as Style;
            }
            else if (selectedGridRows == 4 && selectedGridColumns == 4)
            {
                Grid4x4Button.Style = FindResource("ActionButtonStyle") as Style;
            }
            else
            {
                CustomGridButton.Style = FindResource("ActionButtonStyle") as Style;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                NameInput.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(VideoChannelInput.Text))
            {
                MessageBox.Show("Please enter a video channel.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                VideoChannelInput.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(SequenceResourceInput.Text))
            {
                MessageBox.Show("Please enter a sequence resource.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                SequenceResourceInput.Focus();
                return;
            }

            // Create result object or save data
            var result = new
            {
                Name = NameInput.Text.Trim(),
                VideoChannel = VideoChannelInput.Text.Trim(),
                SequenceResource = SequenceResourceInput.Text.Trim(),
                Remarks = RemarksInput.Text.Trim(),
                GridRows = selectedGridRows,
                GridColumns = selectedGridColumns
            };

            // TODO: Save the data to your data source
            MessageBox.Show($"View saved successfully!\n\nName: {result.Name}\nChannel: {result.VideoChannel}\nResource: {result.SequenceResource}\nGrid Layout: {selectedGridRows}x{selectedGridColumns}", 
                          "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
            Close();
        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            // Make window draggable
            this.MouseLeftButtonDown += (s, args) =>
            {
                if (args.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
        }
    }

    // Custom Grid Dialog for custom grid sizes
    public class CustomGridDialog : Window
    {
        private TextBox RowsInput;
        private TextBox ColumnsInput;
        public int SelectedRows { get; private set; }
        public int SelectedColumns { get; private set; }

        public CustomGridDialog()
        {
            Title = "Custom Grid Size";
            Width = 350;
            Height = 250;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Background = new SolidColorBrush(Color.FromRgb(15, 20, 25)); // #0F1419

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var header = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 35, 48)), // #1E2330
                Padding = new Thickness(20, 15, 20, 15)
            };
            var headerText = new TextBlock
            {
                Text = "Custom Grid Size",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 18,
                FontWeight = FontWeights.SemiBold
            };
            header.Child = headerText;
            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            // Rows input
            var rowsPanel = new StackPanel { Margin = new Thickness(20, 15, 20, 10) };
            var rowsLabel = new TextBlock
            {
                Text = "Rows:",
                Foreground = new SolidColorBrush(Color.FromRgb(224, 230, 237)), // #E0E6ED
                Margin = new Thickness(0, 0, 0, 8),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            };
            RowsInput = new TextBox
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 35, 48)), // #1E2330
                Foreground = new SolidColorBrush(Color.FromRgb(224, 230, 237)), // #E0E6ED
                BorderBrush = new SolidColorBrush(Color.FromRgb(58, 63, 74)), // #3A3F4A
                BorderThickness = new Thickness(2, 2, 2, 2),
                Padding = new Thickness(12, 10, 12, 10),
                FontSize = 14,
                Text = "2"
            };
            rowsPanel.Children.Add(rowsLabel);
            rowsPanel.Children.Add(RowsInput);
            Grid.SetRow(rowsPanel, 1);
            grid.Children.Add(rowsPanel);

            // Columns input
            var columnsPanel = new StackPanel { Margin = new Thickness(20, 10, 20, 15) };
            var columnsLabel = new TextBlock
            {
                Text = "Columns:",
                Foreground = new SolidColorBrush(Color.FromRgb(224, 230, 237)), // #E0E6ED
                Margin = new Thickness(0, 0, 0, 8),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            };
            ColumnsInput = new TextBox
            {
                Background = new SolidColorBrush(Color.FromRgb(30, 35, 48)), // #1E2330
                Foreground = new SolidColorBrush(Color.FromRgb(224, 230, 237)), // #E0E6ED
                BorderBrush = new SolidColorBrush(Color.FromRgb(58, 63, 74)), // #3A3F4A
                BorderThickness = new Thickness(2, 2, 2, 2),
                Padding = new Thickness(12, 10, 12, 10),
                FontSize = 14,
                Text = "2"
            };
            columnsPanel.Children.Add(columnsLabel);
            columnsPanel.Children.Add(ColumnsInput);
            Grid.SetRow(columnsPanel, 2);
            grid.Children.Add(columnsPanel);

            // Buttons
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(20, 15, 20, 20)
            };

            var cancelButton = new Button
            {
                Content = "Cancel",
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Color.FromRgb(176, 190, 197)), // #B0BEC5
                BorderBrush = new SolidColorBrush(Color.FromRgb(58, 63, 74)), // #3A3F4A
                BorderThickness = new Thickness(2, 2, 2, 2),
                Padding = new Thickness(24, 12, 24, 12),
                Margin = new Thickness(0, 0, 12, 0),
                FontSize = 14
            };
            cancelButton.Click += (s, e) => { DialogResult = false; Close(); };

            var okButton = new Button
            {
                Content = "OK",
                Background = new SolidColorBrush(Color.FromRgb(74, 158, 255)), // #4A9EFF
                Foreground = new SolidColorBrush(Colors.White),
                BorderThickness = new Thickness(0, 0, 0, 0),
                Padding = new Thickness(24, 12, 24, 12),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold
            };
            okButton.Click += (s, e) =>
            {
                if (int.TryParse(RowsInput.Text, out int rows) && int.TryParse(ColumnsInput.Text, out int columns))
                {
                    if (rows > 0 && rows <= 10 && columns > 0 && columns <= 10)
                    {
                        SelectedRows = rows;
                        SelectedColumns = columns;
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter values between 1 and 10.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter valid numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(okButton);
            Grid.SetRow(buttonPanel, 3);
            grid.Children.Add(buttonPanel);

            Content = grid;

            // Make window draggable
            this.MouseLeftButtonDown += (s, args) =>
            {
                if (args.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                    this.DragMove();
            };
        }
    }
}
