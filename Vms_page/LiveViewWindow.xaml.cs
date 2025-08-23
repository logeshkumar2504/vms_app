using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Vms_page
{
    public partial class LiveViewWindow : Window
    {
        private int currentRows = 1;
        private int currentColumns = 1;
        private List<Border> cameraCells = new List<Border>();

        public LiveViewWindow()
        {
            InitializeComponent();
            
            // Ensure window respects taskbar
            this.SourceInitialized += LiveViewWindow_SourceInitialized;
            
            // Initialize with default 1x1 grid
            GenerateDynamicGrid(1, 1);
            
            // Set initial active button
            UpdateButtonStyles(1, 1);
            
            // Subscribe to theme changes
            this.Loaded += LiveViewWindow_Loaded;
        }

        private void LiveViewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh camera cells to ensure proper theme colors
            RefreshCameraCellColors();
        }

        private void RefreshCameraCellColors()
        {
            foreach (var cell in cameraCells)
            {
                if (cell.Child is Grid grid && grid.Children.Count >= 2)
                {
                    // Update background
                    cell.Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"];
                    
                    // Update icon color
                    if (grid.Children[0] is TextBlock icon)
                    {
                        icon.Foreground = (SolidColorBrush)Application.Current.Resources["TextSecondaryColor"];
                    }
                    
                    // Update label color
                    if (grid.Children[1] is TextBlock label)
                    {
                        label.Foreground = (SolidColorBrush)Application.Current.Resources["TextPrimaryColor"];
                    }
                }
            }
        }

        // Allow window dragging
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Dynamic Grid Generation
        private void GenerateDynamicGrid(int rows, int columns)
        {
            currentRows = rows;
            currentColumns = columns;
            
            // Clear existing grid
            DynamicGridContainer.Children.Clear();
            DynamicGridContainer.RowDefinitions.Clear();
            DynamicGridContainer.ColumnDefinitions.Clear();
            cameraCells.Clear();

            // Create grid definitions
            for (int i = 0; i < rows; i++)
            {
                DynamicGridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            
            for (int j = 0; j < columns; j++)
            {
                DynamicGridContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create camera cells
            int cameraNumber = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var cameraCell = CreateCameraCell(cameraNumber, rows, columns);
                    Grid.SetRow(cameraCell, i);
                    Grid.SetColumn(cameraCell, j);
                    DynamicGridContainer.Children.Add(cameraCell);
                    cameraCells.Add(cameraCell);
                    cameraNumber++;
                }
            }
            
            // Ensure camera cells use current theme colors
            RefreshCameraCellColors();
        }

        private Border CreateCameraCell(int cameraNumber, int totalRows, int totalColumns)
        {
            // Calculate appropriate font sizes based on grid size
            int iconSize = totalRows * totalColumns <= 4 ? 40 : 
                          totalRows * totalColumns <= 9 ? 24 : 
                          totalRows * totalColumns <= 16 ? 16 : 12;
            
            int textSize = totalRows * totalColumns <= 4 ? 14 : 
                          totalRows * totalColumns <= 9 ? 10 : 
                          totalRows * totalColumns <= 16 ? 8 : 6;

            var border = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                CornerRadius = new CornerRadius(totalRows * totalColumns <= 4 ? 8 : 
                                             totalRows * totalColumns <= 9 ? 6 : 
                                             totalRows * totalColumns <= 16 ? 4 : 2),
                Margin = new Thickness(totalRows * totalColumns <= 4 ? 8 : 
                                     totalRows * totalColumns <= 9 ? 4 : 
                                     totalRows * totalColumns <= 16 ? 2 : 1),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 2,
                    Opacity = 0.2,
                    BlurRadius = 4
                }
            };

            var grid = new Grid();
            
            var icon = new TextBlock
            {
                Text = "📹",
                FontSize = iconSize,
                Foreground = (SolidColorBrush)Application.Current.Resources["TextSecondaryColor"],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var label = new TextBlock
            {
                Text = cameraNumber.ToString(),
                FontSize = textSize,
                Foreground = (SolidColorBrush)Application.Current.Resources["TextPrimaryColor"],
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, iconSize * 0.4, 0, 0)
            };

            grid.Children.Add(icon);
            grid.Children.Add(label);
            border.Child = grid;

            return border;
        }

        // Grid View Change Methods
        private void View1x1_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(1, 1);
            UpdateButtonStyles(1, 1);
        }

        private void View2x2_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(2, 2);
            UpdateButtonStyles(2, 2);
        }

        private void View3x3_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(3, 3);
            UpdateButtonStyles(3, 3);
        }

        private void View4x4_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(4, 4);
            UpdateButtonStyles(4, 4);
        }

        private void View8x8_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(8, 8);
            UpdateButtonStyles(8, 8);
        }

        private void UpdateButtonStyles(int rows, int columns)
        {
            // Reset all buttons to default style
            View1x1Button.Style = (Style)FindResource("GridButtonStyle");
            View2x2Button.Style = (Style)FindResource("GridButtonStyle");
            View3x3Button.Style = (Style)FindResource("GridButtonStyle");
            View4x4Button.Style = (Style)FindResource("GridButtonStyle");
            View8x8Button.Style = (Style)FindResource("GridButtonStyle");

            // Set active button style
            if (rows == 1 && columns == 1)
                View1x1Button.Style = (Style)FindResource("ActiveGridButtonStyle");
            else if (rows == 2 && columns == 2)
                View2x2Button.Style = (Style)FindResource("ActiveGridButtonStyle");
            else if (rows == 3 && columns == 3)
                View3x3Button.Style = (Style)FindResource("ActiveGridButtonStyle");
            else if (rows == 4 && columns == 4)
                View4x4Button.Style = (Style)FindResource("ActiveGridButtonStyle");
            else if (rows == 8 && columns == 8)
                View8x8Button.Style = (Style)FindResource("ActiveGridButtonStyle");
        }

        private void LiveViewWindow_SourceInitialized(object sender, EventArgs e)
        {
            // Ensure window respects taskbar by setting appropriate window style
            // This prevents the window from covering the taskbar
            var helper = new System.Windows.Interop.WindowInteropHelper(this);
            var source = System.Windows.Interop.HwndSource.FromHwnd(helper.Handle);
            if (source?.Handle != IntPtr.Zero)
            {
                // Set window style to respect taskbar
                var style = NativeMethods.GetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE);
                style |= NativeMethods.WS_EX_APPWINDOW;
                style &= ~NativeMethods.WS_EX_TOOLWINDOW;
                NativeMethods.SetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE, style);
            }
        }
    }
}
