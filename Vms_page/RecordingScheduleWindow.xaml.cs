using System;
using System.Windows;
using System.Windows.Interop;
using System.Collections.ObjectModel;

namespace Vms_page
{
	public partial class RecordingScheduleWindow : Window
	{
		public ObservableCollection<int> HourBlocks { get; } = new ObservableCollection<int>();
		public RecordingScheduleWindow()
		{
			InitializeComponent();
			this.SourceInitialized += RecordingScheduleWindow_SourceInitialized;
			for (int i = 0; i < 24; i++) HourBlocks.Add(i);
			DataContext = this;
		}

		private void RecordingScheduleWindow_SourceInitialized(object? sender, EventArgs e)
		{
			var helper = new WindowInteropHelper(this);
			var source = HwndSource.FromHwnd(helper.Handle);
			if (source?.Handle != IntPtr.Zero)
			{
				var style = NativeMethods.GetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE);
				style |= NativeMethods.WS_EX_APPWINDOW;
				style &= ~NativeMethods.WS_EX_TOOLWINDOW;
				NativeMethods.SetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE, style);
			}
		}

		private void KeywordTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (KeywordTextBox.Text == "Enter Keywords")
			{
				KeywordTextBox.Text = string.Empty;
				KeywordTextBox.Foreground = System.Windows.Media.Brushes.White;
			}
		}

		private void KeywordTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(KeywordTextBox.Text))
			{
				KeywordTextBox.Text = "Enter Keywords";
				KeywordTextBox.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(128,128,128));
			}
		}
	}
}


