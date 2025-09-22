using System.Windows;

namespace Vms_page
{
	public partial class VcaBlankWindow : Window
	{
		public VcaBlankWindow()
		{
			InitializeComponent();
			ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
			SetActiveTab("Realtime Monitoring");
		}

		private void SetActiveTab(string tab)
		{
			// Button visual state via Tag for template trigger
			RealtimeMonitoringBtn.Tag = tab == "Realtime Monitoring" ? "Active" : null;
			BehaviourSearchBtn.Tag = tab == "Behaviour Search" ? "Active" : null;
			HeatMapBtn.Tag = tab == "Heat Map" ? "Active" : null;

			// Content visibility
			RealtimeMonitoringView.Visibility = tab == "Realtime Monitoring" ? Visibility.Visible : Visibility.Collapsed;
			BehaviourSearchView.Visibility = tab == "Behaviour Search" ? Visibility.Visible : Visibility.Collapsed;
			HeatMapView.Visibility = tab == "Heat Map" ? Visibility.Visible : Visibility.Collapsed;
		}

		private void NavigationButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is System.Windows.Controls.Button btn && btn.Content is string text)
			{
				SetActiveTab(text);
			}
		}

		private void CameraTile_Click(object sender, RoutedEventArgs e)
		{
			if (sender is System.Windows.Controls.Border border)
			{
				// Deselect all tiles first to ensure single selection
				DeselectAllTiles();

				// Select clicked tile
				var selectedColor = System.Windows.Media.Color.FromRgb(0xFF, 0x95, 0x00);
				border.BorderBrush = new System.Windows.Media.SolidColorBrush(selectedColor);
				border.BorderThickness = new Thickness(2);
			}
		}

		private void DeselectAllTiles()
		{
			var normalColor = System.Windows.Media.Color.FromRgb(0x33, 0x33, 0x33);
			void ResetTile(string name)
			{
				if (this.FindName(name) is System.Windows.Controls.Border tile)
				{
					tile.BorderBrush = new System.Windows.Media.SolidColorBrush(normalColor);
					tile.BorderThickness = new Thickness(1);
				}
			}

			ResetTile("Tile00");
			ResetTile("Tile01");
			ResetTile("Tile10");
			ResetTile("Tile11");
		}

		private void RM_SingleView_Click(object sender, RoutedEventArgs e)
		{
			// Collapse all but top-left tile and make it fill the area
			if (FindName("Tile00") is System.Windows.Controls.Border t00 &&
				FindName("Tile01") is System.Windows.Controls.Border t01 &&
				FindName("Tile10") is System.Windows.Controls.Border t10 &&
				FindName("Tile11") is System.Windows.Controls.Border t11)
			{
				// Place t00 to span all rows/cols
				System.Windows.Controls.Grid.SetRowSpan(t00, 2);
				System.Windows.Controls.Grid.SetColumnSpan(t00, 2);
				t01.Visibility = Visibility.Collapsed;
				t10.Visibility = Visibility.Collapsed;
				t11.Visibility = Visibility.Collapsed;
			}
		}

		private void RM_GridView_Click(object sender, RoutedEventArgs e)
		{
			// Restore 2x2 grid
			if (FindName("Tile00") is System.Windows.Controls.Border t00 &&
				FindName("Tile01") is System.Windows.Controls.Border t01 &&
				FindName("Tile10") is System.Windows.Controls.Border t10 &&
				FindName("Tile11") is System.Windows.Controls.Border t11)
			{
				System.Windows.Controls.Grid.SetRowSpan(t00, 1);
				System.Windows.Controls.Grid.SetColumnSpan(t00, 1);
				t01.Visibility = Visibility.Visible;
				t10.Visibility = Visibility.Visible;
				t11.Visibility = Visibility.Visible;
			}
		}

        private bool isFullscreen = false;
        private Visibility prevTopBarVisibility;
        private Visibility prevSidebarVisibility;
        private Visibility prevBottomBarVisibility;
        private System.Windows.Thickness prevContentMargin;
        private System.Windows.GridLength prevSidebarWidth;
        private WindowState prevWindowState;
        private WindowStyle prevWindowStyle;
        private ResizeMode prevResizeMode;
        private bool prevTopmost;
        private double prevLeft;
        private double prevTop;
        private double prevWidth;
        private double prevHeight;

		private void RM_Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			ToggleCameraFullscreen(true);
		}

		private void ToggleCameraFullscreen(bool fromButton)
		{
			if (!isFullscreen)
			{
				prevTopBarVisibility = TopNavBar.Visibility;
				prevSidebarVisibility = RM_Sidebar.Visibility;
				prevBottomBarVisibility = RM_BottomBar.Visibility;
                prevContentMargin = RM_ContentDock.Margin;
                prevSidebarWidth = RM_SidebarColumn.Width;
                prevWindowState = this.WindowState;
                prevWindowStyle = this.WindowStyle;
                prevResizeMode = this.ResizeMode;
                prevTopmost = this.Topmost;
                prevLeft = this.Left;
                prevTop = this.Top;
                prevWidth = this.Width;
                prevHeight = this.Height;

				TopNavBar.Visibility = Visibility.Collapsed;
				RM_Sidebar.Visibility = Visibility.Collapsed;
				RM_BottomBar.Visibility = Visibility.Collapsed;
                // Fill the entire window with the camera grid
                RM_ContentDock.Margin = new Thickness(0);
                RM_GridFrame.BorderThickness = new Thickness(0);
                RM_SidebarColumn.Width = new GridLength(0);
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
                this.Topmost = true;
                this.Left = 0;
                this.Top = 0;
                this.Width = SystemParameters.PrimaryScreenWidth;
                this.Height = SystemParameters.PrimaryScreenHeight;
				isFullscreen = true;
			}
			else
			{
				TopNavBar.Visibility = prevTopBarVisibility;
				RM_Sidebar.Visibility = prevSidebarVisibility;
				RM_BottomBar.Visibility = prevBottomBarVisibility;
                RM_ContentDock.Margin = prevContentMargin;
                RM_GridFrame.BorderThickness = new Thickness(0);
                RM_SidebarColumn.Width = prevSidebarWidth;
                this.WindowStyle = prevWindowStyle;
                this.ResizeMode = prevResizeMode;
                this.Topmost = prevTopmost;
                this.Left = prevLeft;
                this.Top = prevTop;
                this.Width = prevWidth;
                this.Height = prevHeight;
                this.WindowState = prevWindowState;
				isFullscreen = false;
			}
		}

		protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (e.Key == System.Windows.Input.Key.Escape && isFullscreen)
			{
				ToggleCameraFullscreen(false);
				e.Handled = true;
			}
		}

		private void ChannelSearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			if (ChannelSearchTextBox.Text == "Enter Keywords")
			{
				ChannelSearchTextBox.Text = string.Empty;
				ChannelSearchTextBox.Foreground = (System.Windows.Media.SolidColorBrush)FindResource("TextPrimaryColor");
			}
		}

		private void ChannelSearchTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(ChannelSearchTextBox.Text))
			{
				ChannelSearchTextBox.Text = "Enter Keywords";
				ChannelSearchTextBox.Foreground = (System.Windows.Media.SolidColorBrush)FindResource("TextSecondaryColor");
			}
		}
	}
}


