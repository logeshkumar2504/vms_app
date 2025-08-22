using System.Windows;
using System.Windows.Input;
using System.Windows.Controls; // Added for TextBlock
using System; // Added for EventArgs
using System.Windows.Interop; // Added for WindowInteropHelper
using System.Runtime.InteropServices; // Added for DllImport

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
            // TODO: Implement edit functionality
            MessageBox.Show("Edit button clicked", "Edit View", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement delete functionality
            var result = MessageBox.Show("Are you sure you want to delete the selected view?", "Delete View", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // TODO: Implement actual deletion logic
                MessageBox.Show("View deleted successfully", "Delete View", MessageBoxButton.OK, MessageBoxImage.Information);
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
