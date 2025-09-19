using System.Windows;

namespace Vms_page
{
    /// <summary>
    /// Interaction logic for SelectFaceLibraryDialog.xaml
    /// </summary>
    public partial class SelectFaceLibraryDialog : Window
    {
        public string SelectedFaceLibrary { get; private set; } = string.Empty;

        public SelectFaceLibraryDialog()
        {
            InitializeComponent();
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter Keywords")
            {
                SearchTextBox.Text = string.Empty;
                SearchTextBox.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter Keywords";
                SearchTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the selected face library (for now, just close the dialog)
            SelectedFaceLibrary = "Selected Library";
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
