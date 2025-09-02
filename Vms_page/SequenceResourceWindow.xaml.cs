using System.Windows;
using System.Windows.Input;

namespace Vms_page
{
    public partial class SequenceResourceWindow : Window
    {
        public SequenceResourceWindow()
        {
            InitializeComponent();

            // Ensure window respects taskbar
            this.SourceInitialized += SequenceResourceWindow_SourceInitialized;
        }

        private void SequenceResourceWindow_SourceInitialized(object? sender, System.EventArgs e)
        {
            var helper = new System.Windows.Interop.WindowInteropHelper(this);
            var source = System.Windows.Interop.HwndSource.FromHwnd(helper.Handle);
            if (source?.Handle != System.IntPtr.Zero)
            {
                var style = NativeMethods.GetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE);
                style |= NativeMethods.WS_EX_APPWINDOW;
                style &= ~NativeMethods.WS_EX_TOOLWINDOW;
                NativeMethods.SetWindowLong(source.Handle, NativeMethods.GWL_EXSTYLE, style);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var popup = new AddSequenceResourcePopup
            {
                Owner = this
            };
            popup.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit sequence clicked", "Sequence", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete sequence clicked", "Sequence", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}


