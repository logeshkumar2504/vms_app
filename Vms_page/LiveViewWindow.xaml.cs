using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

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
            else
                ViewDropdownButton.Content = $"View ({rows}×{columns})";
        }

        // Customize Button Click Event
        private void CustomizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle the dropdown popup
            CustomGridDropdownPopup.IsOpen = !CustomGridDropdownPopup.IsOpen;
        }

        // Apply Custom Grid Layout
        private void ApplyCustomGridLayout(int rows, int columns, List<GridCellInfo> customLayout)
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

            // Create camera cells based on custom layout
            int cameraNumber = 1;
            foreach (var cellInfo in customLayout)
            {
                var cameraCell = CreateCustomCameraCell(cameraNumber, cellInfo);
                Grid.SetRow(cameraCell, cellInfo.Row);
                Grid.SetColumn(cameraCell, cellInfo.Column);
                Grid.SetRowSpan(cameraCell, cellInfo.RowSpan);
                Grid.SetColumnSpan(cameraCell, cellInfo.ColumnSpan);
                DynamicGridContainer.Children.Add(cameraCell);
                cameraCells.Add(cameraCell);
                cameraNumber++;
            }
            
            // Ensure camera cells use current theme colors
            RefreshCameraCellColors();
        }

        private Border CreateCustomCameraCell(int cameraNumber, GridCellInfo cellInfo)
        {
            // Calculate appropriate font sizes based on grid size
            int iconSize = cellInfo.RowSpan * cellInfo.ColumnSpan <= 4 ? 36 : 
                          cellInfo.RowSpan * cellInfo.ColumnSpan <= 9 ? 24 : 
                          cellInfo.RowSpan * cellInfo.ColumnSpan <= 16 ? 18 : 12;
            
            int textSize = cellInfo.RowSpan * cellInfo.ColumnSpan <= 4 ? 14 : 
                          cellInfo.RowSpan * cellInfo.ColumnSpan <= 9 ? 10 : 
                          cellInfo.RowSpan * cellInfo.ColumnSpan <= 16 ? 8 : 6;

            var border = new Border
            {
                Style = (Style)FindResource("CameraCellStyle"),
                Margin = new Thickness(cellInfo.RowSpan * cellInfo.ColumnSpan <= 4 ? 8 : 
                                     cellInfo.RowSpan * cellInfo.ColumnSpan <= 9 ? 6 : 
                                     cellInfo.RowSpan * cellInfo.ColumnSpan <= 16 ? 4 : 2)
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

        // Grid selection methods for dropdown popup
        private void Grid1x1_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(1, 1);
            UpdateButtonStyles(1, 1);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid3Col_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(1, 3);
            UpdateButtonStyles(1, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid2x2_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(2, 2);
            UpdateButtonStyles(2, 2);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid4Pane_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(2, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(2, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid5Pane_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(2, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 2, RowSpan = 2, ColumnSpan = 1 }
            });
            UpdateButtonStyles(2, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid6Pane_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(2, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(2, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid7Pane_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(2, 4, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 3, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 3, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(2, 4);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid8Pane_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(2, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 3 },
                new GridCellInfo { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(2, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid3x3_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(3, 3);
            UpdateButtonStyles(3, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid10Top_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(3, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 3 },
                new GridCellInfo { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 2, Column = 0, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 2, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 2, Column = 2, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(3, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid10Left_Click(object sender, RoutedEventArgs e)
        {
            ApplyCustomGridLayout(3, 3, new List<GridCellInfo>
            {
                new GridCellInfo { Row = 0, Column = 0, RowSpan = 3, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 2, Column = 1, RowSpan = 1, ColumnSpan = 1 },
                new GridCellInfo { Row = 2, Column = 2, RowSpan = 1, ColumnSpan = 1 }
            });
            UpdateButtonStyles(3, 3);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid13Pane_Click(object sender, RoutedEventArgs e)
        {
            var layout = new List<GridCellInfo>();
            
            // Add outer cells
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((i == 0 || i == 4 || j == 0 || j == 4))
                    {
                        layout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                    }
                }
            }
            
            // Add center merged cell
            layout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 3, ColumnSpan = 3 });
            
            ApplyCustomGridLayout(5, 5, layout);
            UpdateButtonStyles(5, 5);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid4x4_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(4, 4);
            UpdateButtonStyles(4, 4);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid25_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(5, 5);
            UpdateButtonStyles(5, 5);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid32_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(8, 4);
            UpdateButtonStyles(8, 4);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid36_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(6, 6);
            UpdateButtonStyles(6, 6);
            CustomGridDropdownPopup.IsOpen = false;
        }

        private void Grid64_Click(object sender, RoutedEventArgs e)
        {
            GenerateDynamicGrid(8, 8);
            UpdateButtonStyles(8, 8);
            CustomGridDropdownPopup.IsOpen = false;
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
