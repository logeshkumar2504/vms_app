using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Vms_page
{
    public partial class AddSequenceResourcePopup : Window
    {
        public AddSequenceResourcePopup()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete selected channels from sequence.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void IntervalBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
    }
}


