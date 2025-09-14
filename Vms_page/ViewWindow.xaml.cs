using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Vms_page
{
    public partial class ViewWindow : Window
    {
        public ViewWindow()
        {
            InitializeComponent();
            
            // Ensure window respects taskbar
            this.SourceInitialized += ViewWindow_SourceInitialized;
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addViewPopup = new AddViewPopup();
            addViewPopup.Owner = this;
            addViewPopup.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No views available to edit", "No Data", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No views available to delete", "No Data", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search views...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search views...";
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Enable window dragging
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ViewWindow_SourceInitialized(object sender, EventArgs e)
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
