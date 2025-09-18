using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vms_page
{
    public partial class FaceRecognitionWindow : Window
    {
        public FaceRecognitionWindow()
        {
            InitializeComponent();
            // Ensure placeholder is set if empty on load
            if (VideoChannelInput != null && string.IsNullOrWhiteSpace(VideoChannelInput.Text))
            {
                VideoChannelInput.Text = "Enter channel name";
                VideoChannelInput.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
            }
            // Default to single view at startup
            SetLayout(1);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is string tag)
            {
                // Show sidebar only for Realtime Monitoring
                if (LeftSidebar != null)
                {
                    LeftSidebar.Visibility = tag == "Realtime Monitoring" ? Visibility.Visible : Visibility.Collapsed;
                }

                // Toggle main views
                if (RealtimeGrid != null && EmptyState != null)
                {
                    var isRealtime = tag == "Realtime Monitoring";
                    RealtimeGrid.Visibility = isRealtime ? Visibility.Visible : Visibility.Collapsed;
                    EmptyState.Visibility = isRealtime ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        private void VideoFeed_Click(object sender, MouseButtonEventArgs e)
        {
            // Placeholder handler for video tiles
        }

        private void SetLayout_Click(object sender, RoutedEventArgs e)
        {
            if (CameraTilesHost == null || sender is not FrameworkElement fe || fe.Tag is not string tag)
                return;

            int tileCount = tag switch
            {
                "1" => 1,
                "2" => 2,
                "4" => 4,
                _ => 4
            };

            SetLayout(tileCount);
        }

        private void SetLayout(int tileCount)
        {
            if (CameraTilesHost == null) return;

            switch (tileCount)
            {
                case 1:
                    CameraTilesHost.Rows = 1;
                    CameraTilesHost.Columns = 1;
                    break;
                case 2:
                    CameraTilesHost.Rows = 1;
                    CameraTilesHost.Columns = 2;
                    break;
                default:
                    CameraTilesHost.Rows = 2;
                    CameraTilesHost.Columns = 2;
                    break;
            }

            CameraTilesHost.Children.Clear();
            for (int i = 0; i < tileCount; i++)
            {
                var border = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                    BorderThickness = new Thickness(1),
                    Cursor = Cursors.Hand
                };

                var grid = new Grid();
                var image = new Image
                {
                    Source = new BitmapImage(new System.Uri("/Assets/Icons/cctv.png", System.UriKind.Relative)),
                    Width = 60,
                    Height = 60,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Uniform
                };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                grid.Children.Add(image);
                border.Child = grid;
                border.MouseLeftButtonDown += VideoFeed_Click;
                CameraTilesHost.Children.Add(border);
            }
        }

        private void VideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (tb.Text == "Enter channel name")
                {
                    tb.Text = string.Empty;
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextPrimaryColor"];
                }
            }
        }

        private void VideoChannelInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = "Enter channel name";
                    tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
                }
            }
        }
    }
}


