using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vms_page
{
    public partial class GridLayoutPopup : Window
    {
        public string SelectedLayout { get; private set; } = "2x2"; // Default layout
        private Button selectedButton;

        public GridLayoutPopup()
        {
            InitializeComponent();
            
            // Apply the current theme
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
        }

        private void SelectLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string layout)
            {
                // Clear previous selection
                if (selectedButton != null)
                {
                    selectedButton.Background = Application.Current.Resources["CardBackgroundColor"] as SolidColorBrush;
                    selectedButton.Foreground = Application.Current.Resources["TextPrimaryColor"] as SolidColorBrush;
                }

                // Set new selection
                selectedButton = button;
                SelectedLayout = layout;
                
                // Update button appearance
                button.Background = Application.Current.Resources["PrimaryColor"] as SolidColorBrush;
                button.Foreground = Brushes.White;
                
                // Enable Apply button
                ApplyButton.IsEnabled = true;
            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            // Return the selected layout and close the window
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Cancel and close the window
            DialogResult = false;
            Close();
        }
    }
}
