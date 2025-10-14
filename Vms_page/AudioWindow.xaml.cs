using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vms_page
{
    public partial class AudioWindow : Window
    {
        public AudioWindow()
        {
            try
            {
                InitializeComponent();
                ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
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
                SetActiveTab(button.Name);
            }
        }

        private void SetActiveTab(string tabName)
        {
            // Set Tag to "Active" for the selected button, null for others (for visual state in template)
            TwoWayAudioBtn.Tag = tabName == "TwoWayAudioBtn" ? "Active" : null;
            BroadcastBtn.Tag = tabName == "BroadcastBtn" ? "Active" : null;
            AudioFileManagementBtn.Tag = tabName == "AudioFileManagementBtn" ? "Active" : null;

            // Toggle content visibility
            switch (tabName)
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

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter Keywords")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = (SolidColorBrush)Application.Current.Resources["TextPrimaryColor"];
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter Keywords";
                SearchTextBox.Foreground = (SolidColorBrush)Application.Current.Resources["TextSecondaryColor"];
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
