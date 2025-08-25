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
        private Border selectedCameraCell = null;
        private int selectedCameraNumber = 0;

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
                    // Update icon color
                    if (grid.Children[0] is TextBlock icon)
                    {
                        icon.Foreground = Brushes.White;
                    }
                    
                    // Update label color
                    if (grid.Children[1] is TextBlock label)
                    {
                        label.Foreground = Brushes.White;
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
            selectedCameraCell = null;
            selectedCameraNumber = 0;
            UpdateSelectedCameraText();

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
            // Calculate appropriate font sizes based on grid size - Made smaller
            int iconSize = totalRows * totalColumns <= 4 ? 36 : 
                          totalRows * totalColumns <= 9 ? 24 : 
                          totalRows * totalColumns <= 16 ? 18 : 12;
            
            int textSize = totalRows * totalColumns <= 4 ? 14 : 
                          totalRows * totalColumns <= 9 ? 10 : 
                          totalRows * totalColumns <= 16 ? 8 : 6;

            var border = new Border
            {
                Style = (Style)FindResource("CameraCellStyle"),
                Margin = new Thickness(totalRows * totalColumns <= 4 ? 8 : 
                                     totalRows * totalColumns <= 9 ? 6 : 
                                     totalRows * totalColumns <= 16 ? 4 : 2)
            };

            var grid = new Grid();
            
            var icon = new TextBlock
            {
                Text = "📹",
                FontSize = iconSize,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var label = new TextBlock
            {
                Text = $"Camera {cameraNumber}",
                FontSize = textSize,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, iconSize * 0.25, 0, 0),
                FontWeight = FontWeights.Medium
            };

            grid.Children.Add(icon);
            grid.Children.Add(label);
            border.Child = grid;

            // Add click event for camera selection
            border.MouseLeftButtonDown += (sender, e) => OnCameraCellClicked(border, cameraNumber);

            return border;
        }

        private void OnCameraCellClicked(Border cameraCell, int cameraNumber)
        {
            // Deselect previously selected camera
            if (selectedCameraCell != null)
            {
                selectedCameraCell.Style = (Style)FindResource("CameraCellStyle");
            }

            // Select new camera
            selectedCameraCell = cameraCell;
            selectedCameraNumber = cameraNumber;
            cameraCell.Style = (Style)FindResource("SelectedCameraCellStyle");

            // Update selected camera text
            UpdateSelectedCameraText();
        }

        private void UpdateSelectedCameraText()
        {
            // Camera selection display removed - no action needed
            // This method is kept for future use if needed
        }

        // Grid View Change Methods
        private void View1x1_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(1, 1);
            UpdateButtonStyles(1, 1);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        private void View2x2_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(2, 2);
            UpdateButtonStyles(2, 2);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        private void View3x3_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(3, 3);
            UpdateButtonStyles(3, 3);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        private void View4x4_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(4, 4);
            UpdateButtonStyles(4, 4);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        private void View5x5_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(5, 5);
            UpdateButtonStyles(5, 5);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        private void View8x8_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(8, 8);
            UpdateButtonStyles(8, 8);
            ViewDropdownPopup.IsOpen = false; // Close dropdown after selection
        }

        // Dropdown Toggle Method
        private void ViewDropdownButton_Click(object sender, RoutedEventArgs e)
        {
            ViewDropdownPopup.IsOpen = !ViewDropdownPopup.IsOpen;
        }

        private void UpdateButtonStyles(int rows, int columns)
        {
            // Update the View dropdown button to show current selection
            if (rows == 1 && columns == 1)
                ViewDropdownButton.Content = "View (1×1)";
            else if (rows == 2 && columns == 2)
                ViewDropdownButton.Content = "View (2×2)";
            else if (rows == 3 && columns == 3)
                ViewDropdownButton.Content = "View (3×3)";
            else if (rows == 4 && columns == 4)
                ViewDropdownButton.Content = "View (4×4)";
            else if (rows == 5 && columns == 5)
                ViewDropdownButton.Content = "View (5×5)";
            else if (rows == 8 && columns == 8)
                ViewDropdownButton.Content = "View (8×8)";
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
