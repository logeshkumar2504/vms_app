using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vms_page
{
    public partial class DxVideoWallDialog : Window
    {
        public string VideoWallName => NameTextBox.Text.Trim();
        public int SizeX => ParseInt(SizeXTextBox.Text);
        public int SizeY => ParseInt(SizeYTextBox.Text);
        public string Resolution => (ResolutionCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        public bool AutoBind => AutoBindCheck.IsChecked == true;

        public DxVideoWallDialog()
        {
            InitializeComponent();
            BuildPreviewGrid(SizeX, SizeY);
        }

        private void EditSizeButton_Click(object sender, RoutedEventArgs e)
        {
            PopupRowText.Text = SizeX.ToString();
            PopupColText.Text = SizeY.ToString();
            EditSizePopup.IsOpen = true;
        }

        private void SizePopupOk_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PopupRowText.Text, out var r))
            {
                SizeXTextBox.Text = r.ToString();
            }
            if (int.TryParse(PopupColText.Text, out var c))
            {
                SizeYTextBox.Text = c.ToString();
            }
            BuildPreviewGrid(SizeX, SizeY);
            EditSizePopup.IsOpen = false;
        }

        private void SizePopupCancel_Click(object sender, RoutedEventArgs e)
        {
            EditSizePopup.IsOpen = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BuildPreviewGrid(int rows, int cols)
        {
            if (rows <= 0) rows = 1;
            if (cols <= 0) cols = 1;

            PreviewGrid.RowDefinitions.Clear();
            PreviewGrid.ColumnDefinitions.Clear();
            PreviewGrid.Children.Clear();

            for (int r = 0; r < rows; r++)
            {
                PreviewGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
            }
            for (int c = 0; c < cols; c++)
            {
                PreviewGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var border = new System.Windows.Controls.Border
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B2B2B")),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(r == rows - 1 && c == cols - 1 ? "#FF9500" : "#444444")),
                        BorderThickness = new Thickness(r == rows - 1 && c == cols - 1 ? 2 : 1),
                        Cursor = Cursors.Hand
                    };
                    Grid.SetRow(border, r);
                    Grid.SetColumn(border, c);
                    var text = new TextBlock
                    {
                        Text = "Not Bound",
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDBDBD")),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    border.Child = text;
                    border.MouseLeftButtonDown += VideoCell_Click;
                    PreviewGrid.Children.Add(border);
                }
            }
        }

        private void VideoCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border selected)
            {
                // Clear all
                foreach (var child in PreviewGrid.Children)
                {
                    if (child is Border b)
                    {
                        b.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444"));
                        b.BorderThickness = new Thickness(1);
                    }
                }
                // Select clicked
                selected.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9500"));
                selected.BorderThickness = new Thickness(2);
            }
        }

        private static int ParseInt(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            // Trim spaces and non-digits
            var cleaned = new string(text.Trim().ToCharArray());
            if (int.TryParse(cleaned, out var n)) return n;
            // Fallback: filter digits only
            var digitsOnly = string.Concat(cleaned.Where(char.IsDigit));
            return int.TryParse(digitsOnly, out n) ? n : 0;
        }
    }
}


