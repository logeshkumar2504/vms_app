using System.Windows;
using System.Windows.Input;

namespace Vms_page
{
    public partial class PlaybackWindow : Window
    {
        public PlaybackWindow()
        {
            InitializeComponent();
            
            // Apply current theme
            string currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);
            
            // Initialize with Device view active
            SetActiveView(true);
        }

        private void SetActiveView(bool isDevice)
        {
            if (isDevice)
            {
                DeviceButton.Style = (Style)FindResource("ActiveNavbarButtonStyle");
                LocalButton.Style = (Style)FindResource("NavbarButtonStyle");
                // Sidebar: show View and Video Channel; hide Sequence Resources
                if (VideoIconBorder != null) VideoIconBorder.Visibility = Visibility.Visible;
                if (ViewIconBorder != null) ViewIconBorder.Visibility = Visibility.Visible;
                if (SequenceResourcesIconBorder != null) SequenceResourcesIconBorder.Visibility = Visibility.Collapsed;
                DeviceView.Visibility = Visibility.Visible;
                LocalView.Visibility = Visibility.Collapsed;
            }
            else
            {
                LocalButton.Style = (Style)FindResource("ActiveNavbarButtonStyle");
                DeviceButton.Style = (Style)FindResource("NavbarButtonStyle");
                // Sidebar: show only View
                if (VideoIconBorder != null) VideoIconBorder.Visibility = Visibility.Collapsed;
                if (SequenceResourcesIconBorder != null) SequenceResourcesIconBorder.Visibility = Visibility.Collapsed;
                if (ViewIconBorder != null) ViewIconBorder.Visibility = Visibility.Visible;
                DeviceView.Visibility = Visibility.Collapsed;
                LocalView.Visibility = Visibility.Visible;
            }
        }

        private void DeviceButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveView(true);
        }

        private void LocalButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveView(false);
        }

        private void VideoIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (VideoLabel != null)
            {
                VideoLabel.Opacity = 1;
            }
            if (VideoIconBorder != null)
            {
                VideoIconBorder.Width = 130;
            }
        }

        private void VideoIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (VideoLabel != null)
            {
                VideoLabel.Opacity = 0;
            }
            if (VideoIconBorder != null)
            {
                VideoIconBorder.Width = 32;
            }
        }

        private void ViewIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ViewLabel != null)
            {
                ViewLabel.Opacity = 1;
            }
            if (ViewIconBorder != null)
            {
                ViewIconBorder.Width = 130;
            }
        }

        private void ViewIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ViewLabel != null)
            {
                ViewLabel.Opacity = 0;
            }
            if (ViewIconBorder != null)
            {
                ViewIconBorder.Width = 32;
            }
        }

        private void SequenceResourcesIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (SequenceResourcesLabel != null)
            {
                SequenceResourcesLabel.Opacity = 1;
            }
            if (SequenceResourcesIconBorder != null)
            {
                SequenceResourcesIconBorder.Width = 130;
            }
        }

        private void SequenceResourcesIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SequenceResourcesLabel != null)
            {
                SequenceResourcesLabel.Opacity = 0;
            }
            if (SequenceResourcesIconBorder != null)
            {
                SequenceResourcesIconBorder.Width = 32;
            }
        }
    }
}
