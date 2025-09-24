using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vms_page
{
	public partial class LprWindow : Window
	{
		// Common date range for Alarm Records
		private DateTime _startDate = DateTime.Today;
		private DateTime _endDate = DateTime.Today;

		// Pass Thru Records date range
		private DateTime _passThruStartDate = DateTime.Today;
		private DateTime _passThruEndDate = DateTime.Today;

		// Fullscreen state
		private WindowState _prevWindowState;
		private WindowStyle _prevWindowStyle;
		private ResizeMode _prevResizeMode;
		private bool _isFullscreen = false;

		public LprWindow()
		{
			InitializeComponent();

			// Default placeholders/foregrounds
			if (VideoChannelInput != null && string.IsNullOrWhiteSpace(VideoChannelInput.Text))
			{
				VideoChannelInput.Text = "Enter channel name";
				VideoChannelInput.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
			}
			if (PlateNoInput != null)
			{
				PlateNoInput.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
			}
			if (PassThruPlateNoInput != null)
			{
				PassThruPlateNoInput.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
			}

			UpdateDateRangeLabel();
			InitializePassThruDateTimeInputs();
			UpdatePassThruDateRangeLabel();
			SetLayout(1);
		}

		private void NavigationButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn && btn.Tag is string tag)
			{
				// Left sidebar
				if (LeftSidebar != null)
				{
					LeftSidebar.Visibility = (tag == "Realtime Monitoring" || tag == "Plate Library Management" || tag == "Monitoring Task" || tag == "Alarm Records" || tag == "Pass Thru Records") ? Visibility.Visible : Visibility.Collapsed;
				}
				if (VideoChannelSection != null) VideoChannelSection.Visibility = tag == "Realtime Monitoring" ? Visibility.Visible : Visibility.Collapsed;
				if (VehicleLibrarySection != null) VehicleLibrarySection.Visibility = tag == "Plate Library Management" ? Visibility.Visible : Visibility.Collapsed;
				if (MonitoringTaskSection != null) MonitoringTaskSection.Visibility = tag == "Monitoring Task" ? Visibility.Visible : Visibility.Collapsed;
				if (AlarmRecordsSection != null) AlarmRecordsSection.Visibility = tag == "Alarm Records" ? Visibility.Visible : Visibility.Collapsed;
				if (PassThruRecordsSection != null) PassThruRecordsSection.Visibility = tag == "Pass Thru Records" ? Visibility.Visible : Visibility.Collapsed;

				// Right views
				if (RealtimeView != null) RealtimeView.Visibility = tag == "Realtime Monitoring" ? Visibility.Visible : Visibility.Collapsed;
				if (VehicleLibraryView != null) VehicleLibraryView.Visibility = tag == "Plate Library Management" ? Visibility.Visible : Visibility.Collapsed;
				if (MonitoringTaskView != null) MonitoringTaskView.Visibility = tag == "Monitoring Task" ? Visibility.Visible : Visibility.Collapsed;
				if (AlarmRecordsView != null) AlarmRecordsView.Visibility = tag == "Alarm Records" ? Visibility.Visible : Visibility.Collapsed;
				if (PassThruRecordsView != null) PassThruRecordsView.Visibility = tag == "Pass Thru Records" ? Visibility.Visible : Visibility.Collapsed;

				Title = $"LPR - {tag}";
			}
		}

		// Alarm Records quick picks
		private void QuickPickToday_Click(object sender, MouseButtonEventArgs e)
		{
			var today = DateTime.Today;
			_startDate = today;
			_endDate = today;
			UpdateDateRangeLabel();
		}

		private void QuickPick3_Click(object sender, MouseButtonEventArgs e)
		{
			_endDate = DateTime.Today;
			_startDate = DateTime.Today.AddDays(-2);
			UpdateDateRangeLabel();
		}

		private void QuickPick7_Click(object sender, MouseButtonEventArgs e)
		{
			_endDate = DateTime.Today;
			_startDate = DateTime.Today.AddDays(-6);
			UpdateDateRangeLabel();
		}

		private void QuickPick30_Click(object sender, MouseButtonEventArgs e)
		{
			_endDate = DateTime.Today;
			_startDate = DateTime.Today.AddDays(-29);
			UpdateDateRangeLabel();
		}

		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			if (AlarmTypeCombo != null) AlarmTypeCombo.SelectedIndex = 0;
			_startDate = DateTime.Today;
			_endDate = DateTime.Today;
			UpdateDateRangeLabel();
			if (AlarmRecordsList != null) AlarmRecordsList.Items.Clear();
		}

		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			if (AlarmRecordsList == null) return;
			AlarmRecordsList.Items.Clear();
			for (int i = 0; i < 10; i++)
			{
				AlarmRecordsList.Items.Add(new { Time = DateTime.Now.AddMinutes(-i).ToString("yyyy-MM-dd HH:mm:ss"), PlateNo = $"XYZ{i:000}", PlateColor = "Blue", VehicleColor = "Black", Cause = "Match" });
			}
		}

		private void UpdateDateRangeLabel()
		{
			if (DateRangeLabel != null)
			{
				var startStr = _startDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0).ToString("yyyy-MM-dd HH:mm:ss");
				var endStr = _endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
				DateRangeLabel.Text = $"{startStr} ~ {endStr}";
			}
		}

		// Camera grid interactions
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
			if (CameraTilesHost == null || sender is not FrameworkElement fe || fe.Tag is not string tag) return;
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
					Source = new BitmapImage(new Uri("/Assets/Icons/cctv.png", UriKind.Relative)),
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

		// Input helpers
		private void VideoChannelInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb)
			{
				if (tb.Text == "Enter channel name")
				{
					tb.Text = string.Empty;
					tb.Foreground = (Brush)Application.Current.Resources["TextPrimaryColor"];
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
					tb.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
				}
			}
		}

		private void MonitoringTaskChannelInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb)
			{
				if (tb.Text == "Enter channel name")
				{
					tb.Text = string.Empty;
					tb.Foreground = (Brush)Application.Current.Resources["TextPrimaryColor"];
					tb.CaretIndex = tb.Text.Length;
				}
			}
		}

		private void MonitoringTaskChannelInput_LostFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb)
			{
				if (string.IsNullOrWhiteSpace(tb.Text))
				{
					tb.Text = "Enter channel name";
					tb.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
				}
			}
		}

		private void PlateNoInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb && tb.Text == "Enter plate number")
			{
				tb.Text = string.Empty;
				tb.Foreground = (Brush)Application.Current.Resources["TextPrimaryColor"];
				tb.CaretIndex = 0;
			}
		}

		private void PlateNoInput_LostFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
			{
				tb.Text = "Enter plate number";
				tb.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
			}
		}

		// Fullscreen
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

		private void LprWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && _isFullscreen)
			{
				ExitGridOnlyFullscreen();
				_isFullscreen = false;
				this.PreviewKeyDown -= LprWindow_PreviewKeyDown;
				e.Handled = true;
			}
		}

		// Pass Thru Records
		private void PassThruQuickPickToday_Click(object sender, MouseButtonEventArgs e)
		{
			var today = DateTime.Today;
			_passThruStartDate = today;
			_passThruEndDate = today;
			InitializePassThruDateTimeInputs();
			UpdatePassThruDateRangeLabel();
		}

		private void PassThruQuickPick7_Click(object sender, MouseButtonEventArgs e)
		{
			_passThruEndDate = DateTime.Today;
			_passThruStartDate = DateTime.Today.AddDays(-6);
			if (PassThruStartDateTimeInput != null) PassThruStartDateTimeInput.Text = _passThruStartDate.ToString("yyyy-MM-dd HH:mm");
			if (PassThruEndDateTimeInput != null) PassThruEndDateTimeInput.Text = _passThruEndDate.ToString("yyyy-MM-dd HH:mm");
			UpdatePassThruDateRangeLabel();
		}

		private void PassThruQuickPick30_Click(object sender, MouseButtonEventArgs e)
		{
			_passThruEndDate = DateTime.Today;
			_passThruStartDate = DateTime.Today.AddDays(-29);
			if (PassThruStartDateTimeInput != null) PassThruStartDateTimeInput.Text = _passThruStartDate.ToString("yyyy-MM-dd HH:mm");
			if (PassThruEndDateTimeInput != null) PassThruEndDateTimeInput.Text = _passThruEndDate.ToString("yyyy-MM-dd HH:mm");
			UpdatePassThruDateRangeLabel();
		}

		private void PassThruResetButton_Click(object sender, RoutedEventArgs e)
		{
			if (PassThruPlateColorCombo != null) PassThruPlateColorCombo.SelectedIndex = 0;
			if (PassThruVehicleColorCombo != null) PassThruVehicleColorCombo.SelectedIndex = 0;
			if (PassThruDirectionCombo != null) PassThruDirectionCombo.SelectedIndex = 0;
			InitializePassThruDateTimeInputs();
			UpdatePassThruRangeFromInputs();
			if (PassThruRecordsList != null) PassThruRecordsList.Items.Clear();
		}

		private void PassThruSearchButton_Click(object sender, RoutedEventArgs e)
		{
			if (PassThruRecordsList == null) return;
			PassThruRecordsList.Items.Clear();
			for (int i = 0; i < 10; i++)
			{
				PassThruRecordsList.Items.Add(new {
					Time = DateTime.Now.AddMinutes(-i).ToString("yyyy-MM-dd HH:mm:ss"),
					PlateNo = $"ABC{i:000}",
					PlateColor = "Blue",
					VehicleColor = "Black",
					Direction = i % 2 == 0 ? "In" : "Out",
					Channel = $"Channel {(i % 5) + 1}"
				});
			}
		}

		private void UpdatePassThruDateRangeLabel()
		{
			if (PassThruDateRangeLabel != null)
			{
				var startStr = _passThruStartDate.Date.AddHours(0).AddMinutes(0).AddSeconds(0).ToString("yyyy-MM-dd HH:mm:ss");
				var endStr = _passThruEndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
				PassThruDateRangeLabel.Text = $"{startStr} ~ {endStr}";
			}
		}

		private void PassThruPlateNoInput_GotFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb && tb.Text == "Enter plate number")
			{
				tb.Text = string.Empty;
				tb.Foreground = (Brush)Application.Current.Resources["TextPrimaryColor"];
				tb.CaretIndex = 0;
			}
		}

		private void PassThruPlateNoInput_LostFocus(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
			{
				tb.Text = "Enter plate number";
				tb.Foreground = (Brush)Application.Current.Resources["TextSecondaryColor"];
			}
		}

		private void InitializePassThruDateTimeInputs()
		{
			var start = DateTime.Today.AddHours(0).AddMinutes(0);
			var end = DateTime.Today.AddHours(23).AddMinutes(59);
			_passThruStartDate = start;
			_passThruEndDate = end;
			if (PassThruStartDateTimeInput != null) PassThruStartDateTimeInput.Text = start.ToString("yyyy-MM-dd HH:mm");
			if (PassThruEndDateTimeInput != null) PassThruEndDateTimeInput.Text = end.ToString("yyyy-MM-dd HH:mm");
		}

		private void UpdatePassThruRangeFromInputs()
		{
			if (TryParseDateTime(PassThruStartDateTimeInput?.Text, out var s)) _passThruStartDate = s;
			if (TryParseDateTime(PassThruEndDateTimeInput?.Text, out var e)) _passThruEndDate = e;
			UpdatePassThruDateRangeLabel();
		}

		private static bool TryParseDateTime(string? text, out DateTime value)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (DateTime.TryParseExact(text.Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
				{
					return true;
				}
			}
			value = DateTime.Today;
			return false;
		}

		private void PassThruDateTimeInput_LostFocus(object sender, RoutedEventArgs e)
		{
			UpdatePassThruRangeFromInputs();
		}
	}
}






