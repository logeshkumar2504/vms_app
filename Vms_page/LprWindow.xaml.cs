using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vms_page
{
	public partial class LprWindow : Window
	{
		public LprWindow()
		{
			InitializeComponent();
			// Default placeholder and layout similar to FaceRecognition
			if (VideoChannelInput != null && string.IsNullOrWhiteSpace(VideoChannelInput.Text))
			{
				VideoChannelInput.Text = "Enter channel name";
				VideoChannelInput.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
			}
			SetLayout(1);
		}

		private void NavigationButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn && btn.Tag is string tag)
			{
				// Show sidebar only for relevant tabs (Realtime Monitoring and Library Management)
				if (LeftSidebar != null)
				{
					LeftSidebar.Visibility = (tag == "Realtime Monitoring" || tag == "Plate Library Management") ? Visibility.Visible : Visibility.Collapsed;
				}
				Title = $"LPR - {tag}";
			}
		}

		private void VideoFeed_Click(object sender, MouseButtonEventArgs e)
		{
			ClearCameraTileSelection();
			if (sender is Border border)
			{
				border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
			}
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

			// Map tileCount to rows/cols similar to FaceRecognition
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

		private void ClearCameraTileSelection()
		{
			if (CameraTilesHost == null) return;
			foreach (var child in CameraTilesHost.Children)
			{
				if (child is Border border)
				{
					border.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
				}
			}
		}

		private void VideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb)
			{
				if (tb.Text == "Enter channel name")
				{
					tb.Text = string.Empty;
					tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextPrimaryColor"];
					// Ensure caret is visible at end; prevent auto scroll to start
					tb.CaretIndex = tb.Text.Length;
				}
			}
		}

		private void VideoChannelInput_LostFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb)
			{
				if (string.IsNullOrWhiteSpace(tb.Text))
				{
					tb.Text = "Enter channel name";
					tb.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["TextSecondaryColor"];
				}
			}
		}

		private WindowState _prevWindowState;
		private WindowStyle _prevWindowStyle;
		private ResizeMode _prevResizeMode;
		private bool _isFullscreen = false;

		private void Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			if (!_isFullscreen)
			{
				_prevWindowState = this.WindowState;
				_prevWindowStyle = this.WindowStyle;
				_prevResizeMode = this.ResizeMode;
				EnterGridOnlyFullscreen();
				_isFullscreen = true;
				this.PreviewKeyDown += LprWindow_PreviewKeyDown;
			}
			else
			{
				ExitGridOnlyFullscreen();
				_isFullscreen = false;
				this.PreviewKeyDown -= LprWindow_PreviewKeyDown;
			}
		}

		private void EnterGridOnlyFullscreen()
		{
			if (TopNavBar != null) TopNavBar.Visibility = Visibility.Collapsed;
			if (LeftSidebar != null) LeftSidebar.Visibility = Visibility.Collapsed;
			if (LeftColumn != null) LeftColumn.Width = new GridLength(0);
			if (BottomBar != null) BottomBar.Visibility = Visibility.Collapsed;
			this.WindowStyle = WindowStyle.None;
			this.ResizeMode = ResizeMode.NoResize;
			this.WindowState = WindowState.Maximized;
			if (CameraGridContainer != null)
			{
				CameraGridContainer.Margin = new Thickness(0);
			}
		}

		private void ExitGridOnlyFullscreen()
		{
			if (TopNavBar != null) TopNavBar.Visibility = Visibility.Visible;
			if (LeftSidebar != null) LeftSidebar.Visibility = Visibility.Visible;
			if (LeftColumn != null) LeftColumn.Width = new GridLength(200);
			if (BottomBar != null) BottomBar.Visibility = Visibility.Visible;
			this.WindowStyle = _prevWindowStyle;
			this.ResizeMode = _prevResizeMode;
			this.WindowState = _prevWindowState;
		}

		private void LprWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Escape && _isFullscreen)
			{
				ExitGridOnlyFullscreen();
				_isFullscreen = false;
				this.PreviewKeyDown -= LprWindow_PreviewKeyDown;
				e.Handled = true;
			}
		}
	}
}


