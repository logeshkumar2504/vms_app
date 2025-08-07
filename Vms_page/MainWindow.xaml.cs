using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuardStationUI
{
    public class MenuItemModel
    {
        public string Name { get; set; }
        public string Group { get; set; } // "Basic" or "Smart"
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItemModel> BasicMenuItems { get; set; } = new();
        public ObservableCollection<MenuItemModel> SmartMenuItems { get; set; } = new();
        public ObservableCollection<MenuItemModel> DroppedItems { get; set; } = new();
        private bool isDarkMode = true;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            // Initialize menu items
            BasicMenuItems.Add(new MenuItemModel { Name = "User Management", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Recording Schedule", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "View", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Alarm Records", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Device Management", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Alarm Configuration", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "System Configuration", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Sequence Resource", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Live View", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Operation Log", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Playback", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Audio", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Video Wall", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "E-map", Group = "Basic" });
            BasicMenuItems.Add(new MenuItemModel { Name = "Recording Schdual", Group = "Basic" });
            SmartMenuItems.Add(new MenuItemModel { Name = "Face Recognition", Group = "Smart" });
            SmartMenuItems.Add(new MenuItemModel { Name = "Access Control", Group = "Smart" });
            SmartMenuItems.Add(new MenuItemModel { Name = "LPR", Group = "Smart" });
            SmartMenuItems.Add(new MenuItemModel { Name = "People Counting", Group = "Smart" });
            SmartMenuItems.Add(new MenuItemModel { Name = "VCA", Group = "Smart" });
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var button = sender as Button;
                if (button?.Tag is MenuItemModel item)
                {
                    DragDrop.DoDragDrop(button, item, DragDropEffects.Move);
                }
            }
        }

        private void DropTargetGrid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MenuItemModel)))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void DropTargetGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MenuItemModel)))
            {
                var item = (MenuItemModel)e.Data.GetData(typeof(MenuItemModel));
                if (!DroppedItems.Contains(item))
                {
                    DroppedItems.Add(item);
                    if (item.Group == "Basic")
                        BasicMenuItems.Remove(item);
                    else if (item.Group == "Smart")
                        SmartMenuItems.Remove(item);
                }
            }
        }

        private void DroppedItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is MenuItemModel item)
            {
                DroppedItems.Remove(item);
                if (item.Group == "Basic")
                    BasicMenuItems.Add(item);
                else if (item.Group == "Smart")
                    SmartMenuItems.Add(item);
            }
        }
    }
}
// This code is part of a WPF application that implements a main window with buttons for various functionalities.