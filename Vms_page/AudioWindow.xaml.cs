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
                
                // Toggle main content views
                if (TwoWayView != null && BroadcastView != null)
                {
                    switch (button.Name)
                    {
                        case "TwoWayAudioBtn":
                            TwoWayView.Visibility = Visibility.Visible;
                            BroadcastView.Visibility = Visibility.Collapsed;
                            if (AudioManagementView != null) AudioManagementView.Visibility = Visibility.Collapsed;
                            if (BottomBar != null) BottomBar.Visibility = Visibility.Visible;
                            System.Diagnostics.Debug.WriteLine("Two-way Audio selected");
                            break;
                        case "BroadcastBtn":
                            TwoWayView.Visibility = Visibility.Collapsed;
                            BroadcastView.Visibility = Visibility.Visible;
                            if (AudioManagementView != null) AudioManagementView.Visibility = Visibility.Collapsed;
                            if (BottomBar != null) BottomBar.Visibility = Visibility.Collapsed;
                            System.Diagnostics.Debug.WriteLine("Broadcast selected");
                            break;
                        case "AudioFileManagementBtn":
                            TwoWayView.Visibility = Visibility.Collapsed;
                            BroadcastView.Visibility = Visibility.Collapsed;
                            if (AudioManagementView != null) AudioManagementView.Visibility = Visibility.Visible;
                            if (BottomBar != null) BottomBar.Visibility = Visibility.Collapsed;
                            System.Diagnostics.Debug.WriteLine("Audio File Management selected");
                            break;
                    }
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

        private void SpeakerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Toggle speaker mute/unmute
            System.Diagnostics.Debug.WriteLine("Speaker button clicked");
            // Add your speaker toggle logic here
        }

        private void MicrophoneBtn_Click(object sender, RoutedEventArgs e)
        {
            // Toggle microphone mute/unmute
            System.Diagnostics.Debug.WriteLine("Microphone button clicked");
            // Add your microphone toggle logic here
        }

        private void MutedMicrophoneBtn_Click(object sender, RoutedEventArgs e)
        {
            // Handle muted microphone control
            System.Diagnostics.Debug.WriteLine("Muted microphone button clicked");
            // Add your muted microphone logic here
        }

    }
}
