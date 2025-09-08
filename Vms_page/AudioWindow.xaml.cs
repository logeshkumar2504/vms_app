using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vms_page
{
    public partial class AudioWindow : Window
    {
        private Button currentActiveButton;

        public AudioWindow()
        {
            try
            {
                InitializeComponent();
                ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
                
                // Set Two-way Audio as the default active button
                currentActiveButton = TwoWayAudioBtn;
                SetActiveButton(TwoWayAudioBtn);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing audio window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                SetActiveButton(button);
                
                // Handle different navigation options
                switch (button.Name)
                {
                    case "TwoWayAudioBtn":
                        // Handle Two-way Audio functionality
                        System.Diagnostics.Debug.WriteLine("Two-way Audio selected");
                        break;
                    case "BroadcastBtn":
                        // Handle Broadcast functionality
                        System.Diagnostics.Debug.WriteLine("Broadcast selected");
                        break;
                    case "AudioFileManagementBtn":
                        // Handle Audio File Management functionality
                        System.Diagnostics.Debug.WriteLine("Audio File Management selected");
                        break;
                }
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            // Reset all buttons to inactive state
            TwoWayAudioBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0xB0, 0xB0));
            TwoWayAudioBtn.FontWeight = FontWeights.Normal;
            
            BroadcastBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0xB0, 0xB0));
            BroadcastBtn.FontWeight = FontWeights.Normal;
            
            AudioFileManagementBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0xB0, 0xB0));
            AudioFileManagementBtn.FontWeight = FontWeights.Normal;

            // Set the active button
            activeButton.Foreground = new SolidColorBrush(Color.FromRgb(0x4A, 0x9E, 0xFF));
            activeButton.FontWeight = FontWeights.SemiBold;
            
            currentActiveButton = activeButton;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter Keywords")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0xB0, 0xB0));
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter Keywords";
                SearchTextBox.Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0xB0, 0xB0));
            }
        }
    }
}
