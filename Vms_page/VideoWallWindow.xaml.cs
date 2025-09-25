using System.Windows;

namespace Vms_page
{
    public partial class VideoWallWindow : Window
    {
        public VideoWallWindow()
        {
            InitializeComponent();
            SetActiveButton("Operation");
        }

        private void SetActiveButton(string button)
        {
            var primary = (Style)FindResource("PrimaryButtonStyle");
            var secondary = (Style)FindResource("SecondaryButtonStyle");

            if (button == "Operation")
            {
                OperationButton.Style = primary;
                ScreenControlButton.Style = secondary;
                AddVideoWallButton.Visibility = Visibility.Visible;
            }
            else
            {
                OperationButton.Style = secondary;
                ScreenControlButton.Style = primary;
                AddVideoWallButton.Visibility = Visibility.Collapsed;
            }
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton("Operation");
        }

        private void ScreenControlButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton("ScreenControl");
        }

        private void AddVideoWallButton_Click(object sender, RoutedEventArgs e)
        {
            AddVideoWallPopup.IsOpen = true;
        }

        private void AddDxVideoWall_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DxVideoWallDialog
            {
                Owner = this
            };
            if (dialog.ShowDialog() == true)
            {
                // You can consume dialog data here if needed
                // e.g., dialog.VideoWallName, dialog.SizeX, dialog.SizeY, dialog.Resolution, dialog.AutoBind
            }
        }

        private void AddDecodingCardVideoWall_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DxVideoWallDialog
            {
                Owner = this,
                Title = "Decoding Card Video Wall Info"
            };
            dialog.ShowDialog();
        }

        private void OperationOneButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation 1 executed.", "Video Wall", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OperationTwoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Operation 2 executed.", "Video Wall", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}


