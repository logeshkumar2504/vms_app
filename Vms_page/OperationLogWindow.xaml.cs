using System.Windows;

namespace Vms_page
{
    public partial class OperationLogWindow : Window
    {
        public OperationLogWindow()
        {
            InitializeComponent();
            InitializeDropdowns();
        }

        private void InitializeDropdowns()
        {
            if (ObjectComboBox != null)
            {
                ObjectComboBox.ItemsSource = new[] { "All", "User", "Device", "System" };
                ObjectComboBox.SelectedIndex = 0;
            }
            if (OperationComboBox != null)
            {
                OperationComboBox.ItemsSource = new[] { "All", "Login", "Logout", "Add", "Edit", "Delete", "Configure" };
                OperationComboBox.SelectedIndex = 0;
            }
            if (ResultComboBox != null)
            {
                ResultComboBox.ItemsSource = new[] { "All", "Success", "Failure" };
                ResultComboBox.SelectedIndex = 0;
            }
            if (DateRangeTextBox != null)
            {
                var today = System.DateTime.Today;
                var start = new System.DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                var end = new System.DateTime(today.Year, today.Month, today.Day, 23, 59, 0);
                DateRangeTextBox.Text = $"{start:yyyy-MM-dd HH:mm} ~ {end:yyyy-MM-dd HH:mm}";
            }
        }
    }
}


