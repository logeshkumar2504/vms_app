using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace Vms_page
{
    public partial class PlaybackWindow : Window
    {
        private readonly string[] _gridLayouts = new[] { "1x1", "2x2", "2x3", "3x3", "4x4" };
        private int _currentLayoutIndex = 1; // default to 2x2
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
            // Convert slider value (hours) to HH:mm:ss
            // If a time label exists in future, we can update it here (removed per latest UI)

            // Expand the filled selection width under the thumb to mimic CCTV tracker
            if (sender is System.Windows.Controls.Slider s)
            {
                var template = s.Template;
                if (template != null)
                {
                    if (template.FindName("PART_Selection", s) is System.Windows.Controls.Border sel)
                    {
                        double ratio = (s.Value - s.Minimum) / (s.Maximum - s.Minimum);
                        sel.Width = ratio * s.ActualWidth;
                    }
                }
            }
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
    }
}
