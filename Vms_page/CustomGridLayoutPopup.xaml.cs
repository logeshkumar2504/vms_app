using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Vms_page
{
    public partial class CustomGridLayoutPopup : Window
    {
        private int selectedGridType = 1;
        private int selectedRows = 1;
        private int selectedColumns = 1;
        private List<GridCellInfo> customGridLayout = new List<GridCellInfo>();

        public CustomGridLayoutPopup()
        {
            InitializeComponent();
        }

        // Grid selection methods
        private void Grid1x1_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 1;
            selectedRows = 1;
            selectedColumns = 1;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid3Col_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 3;
            selectedRows = 1;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid2x2_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 4;
            selectedRows = 2;
            selectedColumns = 2;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid4Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 4;
            selectedRows = 2;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid5Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 5;
            selectedRows = 2;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 2, RowSpan = 2, ColumnSpan = 1 });
        }

        private void Grid6Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 6;
            selectedRows = 2;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid7Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 7;
            selectedRows = 2;
            selectedColumns = 4;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 2, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 2, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 3, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 3, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid8Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 8;
            selectedRows = 2;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 3 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 0, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 1, ColumnSpan = 1 });
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 2, RowSpan = 1, ColumnSpan = 1 });
        }

        private void Grid3x3_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 9;
            selectedRows = 3;
            selectedColumns = 3;
            customGridLayout.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid10Top_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 10;
            selectedRows = 3;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 1, ColumnSpan = 3 });
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i + 1, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid10Left_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 10;
            selectedRows = 3;
            selectedColumns = 3;
            customGridLayout.Clear();
            customGridLayout.Add(new GridCellInfo { Row = 0, Column = 0, RowSpan = 3, ColumnSpan = 1 });
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j + 1, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid13Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 13;
            selectedRows = 5;
            selectedColumns = 5;
            customGridLayout.Clear();
            
            // Add outer cells
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((i == 0 || i == 4 || j == 0 || j == 4))
                    {
                        customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                    }
                }
            }
            
            // Add center merged cell
            customGridLayout.Add(new GridCellInfo { Row = 1, Column = 1, RowSpan = 3, ColumnSpan = 3 });
        }

        private void Grid4x4_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 16;
            selectedRows = 4;
            selectedColumns = 4;
            customGridLayout.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid17Pane_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 17;
            selectedRows = 7;
            selectedColumns = 5;
            customGridLayout.Clear();
            
            // Add outer cells
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((i == 0 || i == 6 || j == 0 || j == 4))
                    {
                        customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                    }
                }
            }
            
            // Add center merged cell
            customGridLayout.Add(new GridCellInfo { Row = 2, Column = 1, RowSpan = 3, ColumnSpan = 3 });
        }

        private void Grid25_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 25;
            selectedRows = 5;
            selectedColumns = 5;
            customGridLayout.Clear();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid32_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 32;
            selectedRows = 8;
            selectedColumns = 4;
            customGridLayout.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid36_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 36;
            selectedRows = 6;
            selectedColumns = 6;
            customGridLayout.Clear();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Grid64_Click(object sender, RoutedEventArgs e)
        {
            selectedGridType = 64;
            selectedRows = 8;
            selectedColumns = 8;
            customGridLayout.Clear();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    customGridLayout.Add(new GridCellInfo { Row = i, Column = j, RowSpan = 1, ColumnSpan = 1 });
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Public properties to access the selected grid configuration
        public int SelectedGridType => selectedGridType;
        public int SelectedRows => selectedRows;
        public int SelectedColumns => selectedColumns;
        public List<GridCellInfo> CustomGridLayout => customGridLayout;
    }

    // Helper class to store grid cell information
    public class GridCellInfo
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
    }
}
