using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media; 

namespace Vms_page
{
    public partial class PeopleCountingCustomLayoutPopup : Window
    {
        private List<Border> selectedCells = new List<Border>();
        private int currentRows = 4;
        private int currentCols = 4;
        public string CustomLayout { get; private set; } = "";
        private List<CombinedCell> combinedCells = new List<CombinedCell>();
        
        public class CombinedCell
        {
            public int StartRow { get; set; }
            public int StartCol { get; set; }
            public int RowSpan { get; set; }
            public int ColSpan { get; set; }
        }

        public PeopleCountingCustomLayoutPopup()
        {
            try
            {
                InitializeComponent();
                ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
                
                // Initialize with default values
                currentRows = 4;
                currentCols = 4;
                
                InitializeGrid();
                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing custom layout popup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeGrid()
        {
            if (MainGridDesigner == null) return;
            
            MainGridDesigner.Children.Clear();
            MainGridDesigner.RowDefinitions.Clear();
            MainGridDesigner.ColumnDefinitions.Clear();

            // Create row definitions
            for (int i = 0; i < currentRows; i++)
            {
                MainGridDesigner.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Create column definitions
            for (int i = 0; i < currentCols; i++)
            {
                MainGridDesigner.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create grid cells
            for (int row = 0; row < currentRows; row++)
            {
                for (int col = 0; col < currentCols; col++)
                {
                    var cell = CreateGridCell(row, col);
                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, col);
                    MainGridDesigner.Children.Add(cell);
                }
            }
        }

        private Border CreateGridCell(int row, int col)
        {
            var border = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0.5),
                Tag = $"{row},{col}",
                Cursor = Cursors.Hand
            };

            // Create sub-grid with 2x2 cells inside each main cell
            var subGrid = new Grid();
            for (int i = 0; i < 2; i++)
            {
                subGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Add single dashed lines to show sub-divisions (no double lines)
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var subBorder = new Border
                    {
                        Background = Brushes.Transparent,
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                        BorderThickness = new Thickness(0.3),
                        Margin = new Thickness(0.2)
                    };
                    Grid.SetRow(subBorder, i);
                    Grid.SetColumn(subBorder, j);
                    subGrid.Children.Add(subBorder);
                }
            }

            border.Child = subGrid;
            border.MouseLeftButtonDown += Cell_Click;
            border.MouseEnter += Cell_MouseEnter;
            border.MouseLeave += Cell_MouseLeave;

            return border;
        }

        private void Cell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    // Multi-select with Ctrl
                    if (selectedCells.Contains(border))
                    {
                        selectedCells.Remove(border);
                        border.Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"];
                    }
                    else
                    {
                        selectedCells.Add(border);
                        border.Background = new SolidColorBrush(Color.FromRgb(0x4A, 0x9E, 0xFF));
                    }
                }
                else
                {
                    // Single select
                    ClearSelection();
                    selectedCells.Add(border);
                    border.Background = new SolidColorBrush(Color.FromRgb(0x4A, 0x9E, 0xFF));
                }
            }
        }

        private void Cell_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border && !selectedCells.Contains(border))
            {
                border.Background = (SolidColorBrush)Application.Current.Resources["MenuHoverColor"];
            }
        }

        private void Cell_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border && !selectedCells.Contains(border))
            {
                border.Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"];
            }
        }

        private void ClearSelection()
        {
            foreach (var cell in selectedCells)
            {
                cell.Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"];
            }
            selectedCells.Clear();
        }

        private void LayoutTextChanged(object sender, TextChangedEventArgs e)
        {
            if (RowsTextBox != null && ColsTextBox != null)
            {
                if (int.TryParse(RowsTextBox.Text, out int rows) &&
                    int.TryParse(ColsTextBox.Text, out int cols) &&
                    rows > 0 && cols > 0 && rows <= 10 && cols <= 10) // Validate dimensions (max 10x10)
                {
                    System.Diagnostics.Debug.WriteLine($"Layout changed to {rows}x{cols}");
                    currentRows = rows;
                    currentCols = cols;
                    
                    // Clear any existing combined cells when layout changes
                    combinedCells.Clear();
                    selectedCells.Clear();
                    
                    InitializeGrid();
                    UpdatePreview();
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add functionality - This would add a new cell to the grid", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCells.Count > 0)
            {
                MessageBox.Show($"Delete functionality - This would delete {selectedCells.Count} selected cell(s)", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select cells to delete", "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CombineButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCells.Count > 1)
            {
                // Get the bounds of selected cells - works for ANY grid size
                var positions = new List<(int row, int col)>();
                foreach (var cell in selectedCells)
                {
                    if (cell.Tag is string tag)
                    {
                        var parts = tag.Split(',');
                        if (parts.Length == 2 && 
                            int.TryParse(parts[0], out int row) && 
                            int.TryParse(parts[1], out int col))
                        {
                            positions.Add((row, col));
                        }
                    }
                }

                if (positions.Count > 1)
                {
                    // Calculate the bounding rectangle - works for ANY combination
                    int minRow = positions.Min(p => p.row);
                    int maxRow = positions.Max(p => p.row);
                    int minCol = positions.Min(p => p.col);
                    int maxCol = positions.Max(p => p.col);

                    // Create combined cell - flexible for any size
                    var combinedCell = new CombinedCell
                    {
                        StartRow = minRow,
                        StartCol = minCol,
                        RowSpan = maxRow - minRow + 1,
                        ColSpan = maxCol - minCol + 1
                    };

                    System.Diagnostics.Debug.WriteLine($"Combining cells: {positions.Count} cells into {combinedCell.RowSpan}x{combinedCell.ColSpan} at ({minRow},{minCol})");

                    // Remove individual cells and add combined cell
                    foreach (var cell in selectedCells.ToList())
                    {
                        cell.Visibility = Visibility.Collapsed;
                    }

                    // Create a new combined border - works for any size
                    var combinedBorder = CreateCombinedCell(combinedCell);
                    Grid.SetRow(combinedBorder, minRow);
                    Grid.SetColumn(combinedBorder, minCol);
                    Grid.SetRowSpan(combinedBorder, combinedCell.RowSpan);
                    Grid.SetColumnSpan(combinedBorder, combinedCell.ColSpan);
                    MainGridDesigner.Children.Add(combinedBorder);

                    combinedCells.Add(combinedCell);
                    selectedCells.Clear();
                    UpdatePreview();
                    
                    System.Diagnostics.Debug.WriteLine($"Total combined cells: {combinedCells.Count}");
                }
            }
            else if (selectedCells.Count == 1)
            {
                MessageBox.Show("Please select at least 2 cells to combine", "Combine", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Please select multiple cells to combine", "Combine", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private Border CreateCombinedCell(CombinedCell combinedCell)
        {
            var border = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(0.5),
                Tag = $"combined_{combinedCell.StartRow}_{combinedCell.StartCol}",
                Cursor = Cursors.Hand
            };

            // Create a grid for the combined cell content
            var grid = new Grid();
            
            // Add a text block to show it's combined
            var textBlock = new TextBlock
            {
                Text = "📹",
                FontSize = 24,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(textBlock);

            border.Child = grid;
            border.MouseLeftButtonDown += CombinedCell_Click;

            return border;
        }

        private void CombinedCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                // Toggle selection of combined cell
                if (selectedCells.Contains(border))
                {
                    selectedCells.Remove(border);
                    border.Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"];
                }
                else
                {
                    ClearSelection();
                    selectedCells.Add(border);
                    border.Background = new SolidColorBrush(Color.FromRgb(0x4A, 0x9E, 0xFF));
                }
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (combinedCells.Count > 0)
            {
                // Remove the last combined cell
                var lastCombined = combinedCells[combinedCells.Count - 1];
                combinedCells.RemoveAt(combinedCells.Count - 1);
                
                // Remove the combined border from the grid
                var combinedBorder = MainGridDesigner.Children.OfType<Border>()
                    .FirstOrDefault(b => b.Tag?.ToString() == $"combined_{lastCombined.StartRow}_{lastCombined.StartCol}");
                
                if (combinedBorder != null)
                {
                    MainGridDesigner.Children.Remove(combinedBorder);
                }
                
                // Restore the individual cells
                for (int row = lastCombined.StartRow; row < lastCombined.StartRow + lastCombined.RowSpan; row++)
                {
                    for (int col = lastCombined.StartCol; col < lastCombined.StartCol + lastCombined.ColSpan; col++)
                    {
                        var cell = MainGridDesigner.Children.OfType<Border>()
                            .FirstOrDefault(b => b.Tag?.ToString() == $"{row},{col}");
                        if (cell != null)
                        {
                            cell.Visibility = Visibility.Visible;
                        }
                    }
                }
                
                UpdatePreview();
            }
            else
            {
                MessageBox.Show("No actions to undo", "Undo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdatePreview()
        {
            if (PreviewIcon == null) return;
            
            PreviewIcon.Children.Clear();
            PreviewIcon.RowDefinitions.Clear();
            PreviewIcon.ColumnDefinitions.Clear();

            // Create a 2x2 preview grid to match the image
            PreviewIcon.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            PreviewIcon.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            PreviewIcon.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            PreviewIcon.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Top-left cell with complex 2x2 sub-grid (like in the image)
            var topLeftBorder = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(0.5),
                Margin = new Thickness(0.5)
            };

            var topLeftSubGrid = new Grid();
            for (int i = 0; i < 2; i++)
            {
                topLeftSubGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                topLeftSubGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create 2x2 sub-grid within top-left cell
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var subBorder = new Border
                    {
                        Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                        BorderThickness = new Thickness(0.3),
                        Margin = new Thickness(0.3)
                    };
                    Grid.SetRow(subBorder, i);
                    Grid.SetColumn(subBorder, j);
                    topLeftSubGrid.Children.Add(subBorder);
                }
            }
            topLeftBorder.Child = topLeftSubGrid;
            Grid.SetRow(topLeftBorder, 0);
            Grid.SetColumn(topLeftBorder, 0);
            PreviewIcon.Children.Add(topLeftBorder);

            // Top-right cell with 1x2 sub-grid (like in the image)
            var topRightBorder = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(0.5),
                Margin = new Thickness(0.5)
            };

            var topRightSubGrid = new Grid();
            for (int i = 0; i < 2; i++)
            {
                topRightSubGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                topRightSubGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Create 1x2 sub-grid within top-right cell
            var topRightSubBorder = new Border
            {
                Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                BorderThickness = new Thickness(0.3),
                Margin = new Thickness(0.3)
            };
            Grid.SetRow(topRightSubBorder, 0);
            Grid.SetColumn(topRightSubBorder, 0);
            Grid.SetColumnSpan(topRightSubBorder, 2);
            topRightSubGrid.Children.Add(topRightSubBorder);
            topRightBorder.Child = topRightSubGrid;
            Grid.SetRow(topRightBorder, 0);
            Grid.SetColumn(topRightBorder, 1);
            PreviewIcon.Children.Add(topRightBorder);

            // Bottom cells (regular)
            for (int col = 0; col < 2; col++)
            {
                var bottomBorder = new Border
                {
                    Background = (SolidColorBrush)Application.Current.Resources["SurfaceColor"],
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)),
                    BorderThickness = new Thickness(0.5),
                    Margin = new Thickness(0.5)
                };
                Grid.SetRow(bottomBorder, 1);
                Grid.SetColumn(bottomBorder, col);
                PreviewIcon.Children.Add(bottomBorder);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Create a custom layout based on the current grid and combined cells
            // This will work for ANY grid size and ANY combination of cells
            if (combinedCells.Count > 0)
            {
                // Create a layout string that represents the combined cells
                var layoutParts = new List<string>();
                layoutParts.Add($"custom_{currentRows}x{currentCols}_combined");
                
                // Add each combined cell with its position and span
                foreach (var combinedCell in combinedCells)
                {
                    layoutParts.Add($"{combinedCell.StartRow}_{combinedCell.StartCol}_{combinedCell.RowSpan}_{combinedCell.ColSpan}");
                }
                
                CustomLayout = string.Join("_", layoutParts);
                System.Diagnostics.Debug.WriteLine($"Custom Combined Layout: {CustomLayout}");
            }
            else
            {
                // No combined cells, use regular grid - works for ANY size
                CustomLayout = $"custom_{currentRows}x{currentCols}";
                System.Diagnostics.Debug.WriteLine($"Custom Regular Layout: {CustomLayout}");
            }
            
            // Debug: Show what layout is being applied
            System.Diagnostics.Debug.WriteLine($"Final Custom Layout Applied: {CustomLayout}");
            
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
