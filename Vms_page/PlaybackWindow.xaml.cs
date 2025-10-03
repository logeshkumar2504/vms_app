using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace Vms_page
{
    public partial class PlaybackWindow : Window
    {
        private readonly string[] _gridLayouts = new[] { "1x1", "2x2", "2x3", "3x3", "4x4" };
        private int _currentLayoutIndex = 1; // default to 2x2
        private double _timelineWindowStartHour = 7.5; // 07:30 start view
        private double _timelineWindowHours = 6.0; // show 6 hours per page
        
        // Local view variables
        private int _localCurrentLayoutIndex = 1; // default to 2x2
        private double _localTimelineWindowStartHour = 7.5; // 07:30 start view
        private double _localTimelineWindowHours = 6.0; // show 6 hours per page
        public PlaybackWindow()
        {
            InitializeComponent();
            
            // Apply current theme
            string currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);
            
            // Initialize with Device view active
            SetActiveView(true);

            // Ensure default camera grid is built with CCTV icons visible on first open
            if (PlaybackVideoGrid != null)
            {
                ApplyGridLayout(_gridLayouts[_currentLayoutIndex]);
            }

            // Initialize Local view grid
            if (LocalPlaybackVideoGrid != null)
            {
                ApplyLocalGridLayout(_gridLayouts[_localCurrentLayoutIndex]);
            }

            // Initialize Local playback date/time fields
            InitializeLocalPlaybackDateTimeFields();

            // Hook calendar selection to main layout date field
            if (PlaybackSidebarCalendar != null)
            {
                PlaybackSidebarCalendar.SelectedDatesChanged -= PlaybackSidebarCalendar_SelectedDatesChanged;
                PlaybackSidebarCalendar.SelectedDatesChanged += PlaybackSidebarCalendar_SelectedDatesChanged;
            }

            InitializePlaybackDateTimeFields();

            // Build initial timeline scale
            RebuildTimelineScale();
            SyncTimeFieldToSlider();
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

        private void InitializePlaybackDateTimeFields()
        {
            if (this.FindName("PlaybackTimeTextBox") is TextBox timeBox && string.IsNullOrWhiteSpace(timeBox.Text))
            {
                timeBox.Text = "00:00:00";
            }

            var date = PlaybackSidebarCalendar != null && PlaybackSidebarCalendar.SelectedDate.HasValue
                ? PlaybackSidebarCalendar.SelectedDate.Value
                : System.DateTime.Today;

            if (this.FindName("PlaybackDateTextBox") is TextBox dateBox)
            {
                dateBox.Text = date.ToString("yyyy-MM-dd");
            }
        }

        private void InitializeLocalPlaybackDateTimeFields()
        {
            if (this.FindName("LocalPlaybackTimeTextBox") is TextBox timeBox && string.IsNullOrWhiteSpace(timeBox.Text))
            {
                timeBox.Text = "00:00:00";
            }

            var date = PlaybackSidebarCalendar != null && PlaybackSidebarCalendar.SelectedDate.HasValue
                ? PlaybackSidebarCalendar.SelectedDate.Value
                : System.DateTime.Today;

            if (this.FindName("LocalPlaybackDateTextBox") is TextBox dateBox)
            {
                dateBox.Text = date.ToString("yyyy-MM-dd");
            }
        }

        private void PlaybackSidebarCalendar_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            var date = PlaybackSidebarCalendar != null && PlaybackSidebarCalendar.SelectedDate.HasValue
                ? PlaybackSidebarCalendar.SelectedDate.Value
                : System.DateTime.Today;
            if (this.FindName("PlaybackDateTextBox") is TextBox dateBox)
            {
                dateBox.Text = date.ToString("yyyy-MM-dd");
            }
            // Also update Local view date
            if (this.FindName("LocalPlaybackDateTextBox") is TextBox localDateBox)
            {
                localDateBox.Text = date.ToString("yyyy-MM-dd");
            }
        }

        private void RebuildTimelineScale()
        {
            // Timeline scale removed - no longer needed
        }

        private void LeftTimeArrowBtn_Click(object sender, RoutedEventArgs e)
        {
            _timelineWindowStartHour = (_timelineWindowStartHour - 1 + 24) % 24; // scroll left by 1 hour
            RebuildTimelineScale();
        }

        private void RightTimeArrowBtn_Click(object sender, RoutedEventArgs e)
        {
            _timelineWindowStartHour = (_timelineWindowStartHour + 1) % 24; // scroll right by 1 hour
            RebuildTimelineScale();
        }

        // Time textbox validation: allow only HH:mm:ss and sync to slider
        private void PlaybackTimeTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow digits and ':' only
            foreach (char c in e.Text)
            {
                if (!(char.IsDigit(c) || c == ':'))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void PlaybackTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (!System.TimeSpan.TryParse(tb.Text, out var ts))
                {
                    // Reset to current slider time if invalid
                    double hours = 0;
                    if (this.FindName("TimelineSlider") is Slider s) hours = s.Value;
                    tb.Text = new System.DateTime(1,1,1).AddHours(hours).ToString("HH:mm:ss");
                    return;
                }

                // Snap to 30-minute increments
                double newHours = System.Math.Round((ts.TotalHours % 24.0) * 2.0) / 2.0;
                if (this.FindName("TimelineSlider") is Slider slider)
                {
                    slider.Value = newHours;
                }
                else
                {
                    tb.Text = new System.DateTime(1,1,1).AddHours(newHours).ToString("HH:mm:ss");
                }
            }
        }

        private void LocalButton_Click(object sender, RoutedEventArgs e)
        {
            SetActiveView(false);
        }

        // Grid layout button - show dropdown if present; otherwise cycle
        private void GridLayout_Click(object sender, RoutedEventArgs e)
        {
            if (this.FindName("GridLayoutDropdownPopup") is System.Windows.Controls.Primitives.Popup popup)
            {
                popup.IsOpen = !popup.IsOpen;
            }
            else
            {
                _currentLayoutIndex = (_currentLayoutIndex + 1) % _gridLayouts.Length;
                ApplyGridLayout(_gridLayouts[_currentLayoutIndex]);
            }
        }

        // Apply selected layout to playback grid
        private void SelectLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is string layout && PlaybackVideoGrid != null)
            {
                ApplyGridLayout(layout);
            }
        }

        private void ApplyGridLayout(string layout)
        {
            // Parse layout e.g., "2x3"
            var parts = layout.Split('x');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int rows) || !int.TryParse(parts[1], out int cols) || rows <= 0 || cols <= 0)
            {
                return;
            }

            // Rebuild grid
            PlaybackVideoGrid.Children.Clear();
            PlaybackVideoGrid.RowDefinitions.Clear();
            PlaybackVideoGrid.ColumnDefinitions.Clear();

            for (int r = 0; r < rows; r++)
                PlaybackVideoGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) });

            for (int c = 0; c < cols; c++)
                PlaybackVideoGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) });

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var border = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                        BorderThickness = new Thickness(1),
                        Cursor = Cursors.Hand
                    };

                    // Center CCTV icon
                    var cellGrid = new Grid();
                    var iconSize = GetOptimalIconSize(rows, cols);
                    var cameraImage = new Image
                    {
                        Source = new BitmapImage(new System.Uri("pack://application:,,,/Assets/Icons/cctv.png")),
                        Width = iconSize,
                        Height = iconSize,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    cellGrid.Children.Add(cameraImage);
                    border.Child = cellGrid;

                    // Selection handler
                    border.MouseLeftButtonDown += Playback_VideoFeed_Click;

                    if (r == rows - 1 && c == cols - 1)
                    {
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                    }

                    Grid.SetRow(border, r);
                    Grid.SetColumn(border, c);
                    PlaybackVideoGrid.Children.Add(border);
                }
            }
        }

        private void Playback_VideoFeed_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                ClearPlaybackSelection();
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
            }
        }

        private void ClearPlaybackSelection()
        {
            if (PlaybackVideoGrid == null) return;
            foreach (var child in PlaybackVideoGrid.Children)
            {
                if (child is Border b)
                {
                    b.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
                }
            }
        }

        private double GetOptimalIconSize(int rows, int cols)
        {
            int total = rows * cols;
            if (total <= 1) return 80;
            if (total <= 4) return 60;
            if (total <= 9) return 40;
            if (total <= 16) return 30;
            return 20;
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

        // Player toolbar handlers (stubs to integrate later)
        private void CloseAll_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for closing all feeds
        }

        private void Snapshot_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for taking snapshot
        }

        private void Switch_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for switch logic
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for previous frame/clip
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                btn.Content = (string)btn.Content == "▶" ? "⏸" : "▶";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (PlayPauseBtn != null) PlayPauseBtn.Content = "▶";
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for next frame/clip
        }

        private void SpeedSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            // Value binding updates label via XAML
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for download
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for fullscreen
        }

        private void TimelineSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is System.Windows.Controls.Slider s)
            {
                double snapped = System.Math.Round(s.Value * 2.0) / 2.0;
                if (System.Math.Abs(snapped - s.Value) > 0.0001)
                {
                    s.Value = snapped;
                }
                // Update Time textbox live
                if (this.FindName("PlaybackTimeTextBox") is TextBox timeBox)
                {
                    var ts = System.TimeSpan.FromHours(snapped);
                    timeBox.Text = new System.DateTime(1,1,1).Add(ts).ToString("HH:mm:ss");
                }

                // Auto-pan the scale to keep thumb within the window
                double windowEnd = (_timelineWindowStartHour + _timelineWindowHours) % 24.0;
                bool isWrapped = _timelineWindowStartHour > windowEnd; // window crosses midnight
                bool needsPan = false;

                if (!isWrapped)
                {
                    if (snapped < _timelineWindowStartHour + 0.1) { _timelineWindowStartHour = System.Math.Max(0, snapped - _timelineWindowHours / 2); needsPan = true; }
                    else if (snapped > windowEnd - 0.1) { _timelineWindowStartHour = System.Math.Max(0, System.Math.Min(24 - _timelineWindowHours, snapped - _timelineWindowHours / 2)); needsPan = true; }
                }
                else
                {
                    // Wrapped: if value not in [start..24) U [0..end]
                    bool inWindow = (snapped >= _timelineWindowStartHour) || (snapped <= windowEnd);
                    if (!inWindow)
                    {
                        _timelineWindowStartHour = System.Math.Max(0, System.Math.Min(24 - _timelineWindowHours, snapped - _timelineWindowHours / 2));
                        needsPan = true;
                    }
                }

                if (needsPan) RebuildTimelineScale();
            }
        }

        // Click on track to jump slider to that position and sync time field
        private void TimelineTrackArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.FindName("TimelineSlider") is Slider slider && sender is Grid grid)
            {
                var pos = e.GetPosition(grid).X;
                if (grid.ActualWidth <= 0) return;
                double ratio = System.Math.Max(0, System.Math.Min(1, pos / grid.ActualWidth));
                double value = slider.Minimum + ratio * (slider.Maximum - slider.Minimum);
                // snap to 30 minutes
                value = System.Math.Round(value * 2.0) / 2.0;
                slider.Value = value;
                SyncTimeFieldToSlider();
            }
        }

        // Full view button - show all 24 hours
        private void FullViewBtn_Click(object sender, RoutedEventArgs e)
        {
            _timelineWindowStartHour = 0;
            _timelineWindowHours = 24;
            RebuildTimelineScale();
        }

        // Zoom in button - show 2 hours
        private void ZoomInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_timelineWindowHours > 2)
            {
                double currentCenter = _timelineWindowStartHour + _timelineWindowHours / 2;
                _timelineWindowHours = 2;
                _timelineWindowStartHour = (currentCenter - _timelineWindowHours / 2 + 24) % 24;
                RebuildTimelineScale();
            }
        }

        // Zoom out button - show 6 hours
        private void ZoomOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_timelineWindowHours < 24)
            {
                double currentCenter = _timelineWindowStartHour + _timelineWindowHours / 2;
                _timelineWindowHours = System.Math.Min(24, _timelineWindowHours + 2);
                _timelineWindowStartHour = (currentCenter - _timelineWindowHours / 2 + 24) % 24;
                RebuildTimelineScale();
            }
        }

        // Show time tooltip when hovering over slider
        private void TimelineSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Slider slider && this.FindName("TimelineToolTipText") is TextBlock tooltipText)
            {
                // Get mouse position relative to the slider
                var position = e.GetPosition(slider);
                var sliderWidth = slider.ActualWidth;
                
                if (sliderWidth > 0)
                {
                    // Calculate the time based on mouse position
                    double ratio = System.Math.Max(0, System.Math.Min(1, position.X / sliderWidth));
                    double timeInHours = slider.Minimum + ratio * (slider.Maximum - slider.Minimum);
                    
                    // Convert to time format and update tooltip
                    var time = System.TimeSpan.FromHours(timeInHours);
                    tooltipText.Text = new System.DateTime(1, 1, 1).Add(time).ToString("HH:mm:ss");
                }
            }
        }

        // Hide tooltip when mouse leaves slider
        private void TimelineSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            // Tooltip will automatically hide when mouse leaves
        }

        private void ToggleRightSidebarButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the right sidebar column between visible and collapsed
            if (RightSidebar != null && RightSidebarColumn != null && ToggleRightSidebarButton != null)
            {
                if (RightSidebar.Visibility == Visibility.Visible)
                {
                    RightSidebar.Visibility = Visibility.Collapsed;
                    RightSidebarColumn.Width = new GridLength(0);
                    ToggleRightSidebarButton.Content = "◀"; // show arrow pointing left to reopen
                }
                else
                {
                    RightSidebar.Visibility = Visibility.Visible;
                    RightSidebarColumn.Width = new GridLength(260);
                    ToggleRightSidebarButton.Content = "▶"; // arrow pointing to collapse
                }
            }
        }

        private void SyncTimeFieldToSlider()
        {
            if (this.FindName("TimelineSlider") is Slider s && this.FindName("PlaybackTimeTextBox") is TextBox tb)
            {
                double snapped = System.Math.Round(s.Value * 2.0) / 2.0;
                var ts = System.TimeSpan.FromHours(snapped);
                tb.Text = new System.DateTime(1,1,1).Add(ts).ToString("HH:mm:ss");
            }
        }

        // Local View Methods - copied and adapted from Device view
        private void LocalGridLayout_Click(object sender, RoutedEventArgs e)
        {
            if (this.FindName("LocalGridLayoutDropdownPopup") is System.Windows.Controls.Primitives.Popup popup)
            {
                popup.IsOpen = !popup.IsOpen;
            }
            else
            {
                _localCurrentLayoutIndex = (_localCurrentLayoutIndex + 1) % _gridLayouts.Length;
                ApplyLocalGridLayout(_gridLayouts[_localCurrentLayoutIndex]);
            }
        }

        private void LocalSelectLayout_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is string layout && LocalPlaybackVideoGrid != null)
            {
                ApplyLocalGridLayout(layout);
            }
        }

        private void ApplyLocalGridLayout(string layout)
        {
            // Parse layout e.g., "2x3"
            var parts = layout.Split('x');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int rows) || !int.TryParse(parts[1], out int cols) || rows <= 0 || cols <= 0)
            {
                return;
            }

            // Rebuild grid
            LocalPlaybackVideoGrid.Children.Clear();
            LocalPlaybackVideoGrid.RowDefinitions.Clear();
            LocalPlaybackVideoGrid.ColumnDefinitions.Clear();

            for (int r = 0; r < rows; r++)
                LocalPlaybackVideoGrid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) });

            for (int c = 0; c < cols; c++)
                LocalPlaybackVideoGrid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star) });

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var border = new Border
                    {
                        Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                        BorderThickness = new Thickness(1),
                        Cursor = Cursors.Hand
                    };

                    // Center CCTV icon for local playback
                    var cellGrid = new Grid();
                    var iconSize = GetOptimalIconSize(rows, cols);
                    var fileImage = new Image
                    {
                        Source = new BitmapImage(new System.Uri("pack://application:,,,/Assets/Icons/cctv.png")),
                        Width = iconSize,
                        Height = iconSize,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    cellGrid.Children.Add(fileImage);
                    border.Child = cellGrid;

                    // Selection handler
                    border.MouseLeftButtonDown += LocalPlayback_VideoFeed_Click;

                    if (r == rows - 1 && c == cols - 1)
                    {
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                    }

                    Grid.SetRow(border, r);
                    Grid.SetColumn(border, c);
                    LocalPlaybackVideoGrid.Children.Add(border);
                }
            }
        }

        private void LocalPlayback_VideoFeed_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                ClearLocalPlaybackSelection();
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
            }
        }

        private void ClearLocalPlaybackSelection()
        {
            if (LocalPlaybackVideoGrid == null) return;
            foreach (var child in LocalPlaybackVideoGrid.Children)
            {
                if (child is Border b)
                {
                    b.BorderBrush = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
                }
            }
        }

        // Local view control handlers
        private void LocalCloseAll_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for closing all local feeds
        }

        private void LocalSnapshot_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for taking snapshot of local playback
        }

        private void LocalSwitch_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for switch logic
        }

        private void LocalPrev_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for previous frame/clip
        }

        private void LocalPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                btn.Content = (string)btn.Content == "▶" ? "⏸" : "▶";
            }
        }

        private void LocalStop_Click(object sender, RoutedEventArgs e)
        {
            if (LocalPlayPauseBtn != null) LocalPlayPauseBtn.Content = "▶";
        }

        private void LocalNext_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for next frame/clip
        }

        private void LocalDownload_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for download
        }

        private void LocalFullscreen_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for fullscreen
        }

        // Local timeline handlers
        private void LocalLeftTimeArrowBtn_Click(object sender, RoutedEventArgs e)
        {
            _localTimelineWindowStartHour = (_localTimelineWindowStartHour - 1 + 24) % 24; // scroll left by 1 hour
            RebuildLocalTimelineScale();
        }

        private void LocalRightTimeArrowBtn_Click(object sender, RoutedEventArgs e)
        {
            _localTimelineWindowStartHour = (_localTimelineWindowStartHour + 1) % 24; // scroll right by 1 hour
            RebuildLocalTimelineScale();
        }

        private void RebuildLocalTimelineScale()
        {
            // Timeline scale removed - no longer needed
        }

        private void LocalTimelineSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is System.Windows.Controls.Slider s)
            {
                double snapped = System.Math.Round(s.Value * 2.0) / 2.0;
                if (System.Math.Abs(snapped - s.Value) > 0.0001)
                {
                    s.Value = snapped;
                }
                // Update Time textbox live
                if (this.FindName("LocalPlaybackTimeTextBox") is TextBox timeBox)
                {
                    var ts = System.TimeSpan.FromHours(snapped);
                    timeBox.Text = new System.DateTime(1,1,1).Add(ts).ToString("HH:mm:ss");
                }

                // Auto-pan the scale to keep thumb within the window
                double windowEnd = (_localTimelineWindowStartHour + _localTimelineWindowHours) % 24.0;
                bool isWrapped = _localTimelineWindowStartHour > windowEnd; // window crosses midnight
                bool needsPan = false;

                if (!isWrapped)
                {
                    if (snapped < _localTimelineWindowStartHour + 0.1) { _localTimelineWindowStartHour = System.Math.Max(0, snapped - _localTimelineWindowHours / 2); needsPan = true; }
                    else if (snapped > windowEnd - 0.1) { _localTimelineWindowStartHour = System.Math.Max(0, System.Math.Min(24 - _localTimelineWindowHours, snapped - _localTimelineWindowHours / 2)); needsPan = true; }
                }
                else
                {
                    // Wrapped: if value not in [start..24) U [0..end]
                    bool inWindow = (snapped >= _localTimelineWindowStartHour) || (snapped <= windowEnd);
                    if (!inWindow)
                    {
                        _localTimelineWindowStartHour = System.Math.Max(0, System.Math.Min(24 - _localTimelineWindowHours, snapped - _localTimelineWindowHours / 2));
                        needsPan = true;
                    }
                }

                if (needsPan) RebuildLocalTimelineScale();
            }
        }

        private void LocalTimelineTrackArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.FindName("LocalTimelineSlider") is Slider slider && sender is Grid grid)
            {
                var pos = e.GetPosition(grid).X;
                if (grid.ActualWidth <= 0) return;
                double ratio = System.Math.Max(0, System.Math.Min(1, pos / grid.ActualWidth));
                double value = slider.Minimum + ratio * (slider.Maximum - slider.Minimum);
                // snap to 30 minutes
                value = System.Math.Round(value * 2.0) / 2.0;
                slider.Value = value;
                SyncLocalTimeFieldToSlider();
            }
        }

        private void LocalFullViewBtn_Click(object sender, RoutedEventArgs e)
        {
            _localTimelineWindowStartHour = 0;
            _localTimelineWindowHours = 24;
            RebuildLocalTimelineScale();
        }

        private void LocalZoomInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_localTimelineWindowHours > 2)
            {
                double currentCenter = _localTimelineWindowStartHour + _localTimelineWindowHours / 2;
                _localTimelineWindowHours = 2;
                _localTimelineWindowStartHour = (currentCenter - _localTimelineWindowHours / 2 + 24) % 24;
                RebuildLocalTimelineScale();
            }
        }

        private void LocalZoomOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_localTimelineWindowHours < 24)
            {
                double currentCenter = _localTimelineWindowStartHour + _localTimelineWindowHours / 2;
                _localTimelineWindowHours = System.Math.Min(24, _localTimelineWindowHours + 2);
                _localTimelineWindowStartHour = (currentCenter - _localTimelineWindowHours / 2 + 24) % 24;
                RebuildLocalTimelineScale();
            }
        }

        private void LocalTimelineSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Slider slider && this.FindName("LocalTimelineToolTipText") is TextBlock tooltipText)
            {
                // Get mouse position relative to the slider
                var position = e.GetPosition(slider);
                var sliderWidth = slider.ActualWidth;
                
                if (sliderWidth > 0)
                {
                    // Calculate the time based on mouse position
                    double ratio = System.Math.Max(0, System.Math.Min(1, position.X / sliderWidth));
                    double timeInHours = slider.Minimum + ratio * (slider.Maximum - slider.Minimum);
                    
                    // Convert to time format and update tooltip
                    var time = System.TimeSpan.FromHours(timeInHours);
                    tooltipText.Text = new System.DateTime(1, 1, 1).Add(time).ToString("HH:mm:ss");
                }
            }
        }

        private void LocalTimelineSlider_MouseLeave(object sender, MouseEventArgs e)
        {
            // Tooltip will automatically hide when mouse leaves
        }

        private void LocalPlaybackTimeTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow digits and ':' only
            foreach (char c in e.Text)
            {
                if (!(char.IsDigit(c) || c == ':'))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void LocalPlaybackTimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (!System.TimeSpan.TryParse(tb.Text, out var ts))
                {
                    // Reset to current slider time if invalid
                    double hours = 0;
                    if (this.FindName("LocalTimelineSlider") is Slider s) hours = s.Value;
                    tb.Text = new System.DateTime(1,1,1).AddHours(hours).ToString("HH:mm:ss");
                    return;
                }

                // Snap to 30-minute increments
                double newHours = System.Math.Round((ts.TotalHours % 24.0) * 2.0) / 2.0;
                if (this.FindName("LocalTimelineSlider") is Slider slider)
                {
                    slider.Value = newHours;
                }
                else
                {
                    tb.Text = new System.DateTime(1,1,1).AddHours(newHours).ToString("HH:mm:ss");
                }
            }
        }

        private void SyncLocalTimeFieldToSlider()
        {
            if (this.FindName("LocalTimelineSlider") is Slider s && this.FindName("LocalPlaybackTimeTextBox") is TextBox tb)
            {
                double snapped = System.Math.Round(s.Value * 2.0) / 2.0;
                var ts = System.TimeSpan.FromHours(snapped);
                tb.Text = new System.DateTime(1,1,1).Add(ts).ToString("HH:mm:ss");
            }
        }
    }
}

