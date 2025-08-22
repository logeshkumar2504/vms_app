using System.Windows;
using System.Windows.Input;
using System.Windows.Controls; // Added for TextBlock
using System; // Added for EventArgs
using System.Runtime.InteropServices; // Added for IntPtr

namespace Vms_page
{
    public partial class LiveViewWindow : Window
    {

        
        public LiveViewWindow()
        {
            InitializeComponent();
            
            // Ensure window respects taskbar
            this.SourceInitialized += LiveViewWindow_SourceInitialized;
        }



        // Allow window dragging
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Grid View Change Methods
        private void View1x1_Click(object sender, RoutedEventArgs e)
        {
            // Show 1x1 view (single camera)
            View1x1Grid.Visibility = Visibility.Visible;
            View2x2Grid.Visibility = Visibility.Collapsed;
            View3x3Grid.Visibility = Visibility.Collapsed;
            View4x4Grid.Visibility = Visibility.Collapsed;
            View8x8Grid.Visibility = Visibility.Collapsed;
            
            // Update button styles
            View1x1Button.Background = System.Windows.Media.Brushes.DarkGreen;
            View2x2Button.Background = System.Windows.Media.Brushes.Blue;
            View3x3Button.Background = System.Windows.Media.Brushes.Purple;
            View4x4Button.Background = System.Windows.Media.Brushes.Orange;
            View8x8Button.Background = System.Windows.Media.Brushes.Brown;
        }

        private void View2x2_Click(object sender, RoutedEventArgs e)
        {
            // Show 2x2 view (four cameras)
            View1x1Grid.Visibility = Visibility.Collapsed;
            View2x2Grid.Visibility = Visibility.Visible;
            View3x3Grid.Visibility = Visibility.Collapsed;
            View4x4Grid.Visibility = Visibility.Collapsed;
            View8x8Grid.Visibility = Visibility.Collapsed;
            
            // Update button styles
            View1x1Button.Background = System.Windows.Media.Brushes.Green;
            View2x2Button.Background = System.Windows.Media.Brushes.DarkBlue;
            View3x3Button.Background = System.Windows.Media.Brushes.Purple;
            View4x4Button.Background = System.Windows.Media.Brushes.Orange;
            View8x8Button.Background = System.Windows.Media.Brushes.Brown;
        }

        private void View3x3_Click(object sender, RoutedEventArgs e)
        {
            // Show 3x3 view (nine cameras)
            View1x1Grid.Visibility = Visibility.Collapsed;
            View2x2Grid.Visibility = Visibility.Collapsed;
            View3x3Grid.Visibility = Visibility.Visible;
            View4x4Grid.Visibility = Visibility.Collapsed;
            View8x8Grid.Visibility = Visibility.Collapsed;
            
            // Update button styles
            View1x1Button.Background = System.Windows.Media.Brushes.Green;
            View2x2Button.Background = System.Windows.Media.Brushes.Blue;
            View3x3Button.Background = System.Windows.Media.Brushes.DarkViolet;
            View4x4Button.Background = System.Windows.Media.Brushes.Orange;
            View8x8Button.Background = System.Windows.Media.Brushes.Brown;
        }

        private void View4x4_Click(object sender, RoutedEventArgs e)
        {
            // Show 4x4 view (sixteen cameras)
            View1x1Grid.Visibility = Visibility.Collapsed;
            View2x2Grid.Visibility = Visibility.Collapsed;
            View3x3Grid.Visibility = Visibility.Collapsed;
            View4x4Grid.Visibility = Visibility.Visible;
            View8x8Grid.Visibility = Visibility.Collapsed;
            
            // Update button styles
            View1x1Button.Background = System.Windows.Media.Brushes.Green;
            View2x2Button.Background = System.Windows.Media.Brushes.Blue;
            View3x3Button.Background = System.Windows.Media.Brushes.Purple;
            View4x4Button.Background = System.Windows.Media.Brushes.OrangeRed;
            View8x8Button.Background = System.Windows.Media.Brushes.Brown;
        }

        private void View8x8_Click(object sender, RoutedEventArgs e)
        {
            // Show 8x8 view (sixty-four cameras)
            View1x1Grid.Visibility = Visibility.Collapsed;
            View2x2Grid.Visibility = Visibility.Collapsed;
            View3x3Grid.Visibility = Visibility.Collapsed;
            View4x4Grid.Visibility = Visibility.Collapsed;
            View8x8Grid.Visibility = Visibility.Visible;
            
            // Update button styles
            View1x1Button.Background = System.Windows.Media.Brushes.Blue;
            View2x2Button.Background = System.Windows.Media.Brushes.Blue;
            View3x3Button.Background = System.Windows.Media.Brushes.Purple;
            View4x4Button.Background = System.Windows.Media.Brushes.Orange;
            View8x8Button.Background = System.Windows.Media.Brushes.SaddleBrown;
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
