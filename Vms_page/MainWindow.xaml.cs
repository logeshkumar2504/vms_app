using System.Windows;
using System.Windows.Media;

namespace GuardStationUI
{
    public partial class MainWindow : Window
    {
        private bool isDarkMode = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AccessControl_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Access Control Clicked");
        }

        private void LPR_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("LPR Clicked");
        }

        private void PeopleCounting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("People Counting Clicked");
        }

        private void VCA_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("VCA Clicked");
        }

        private void FaceRecognition_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Face Recognition Clicked");
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

        private void DarkModeButton_Click(object sender, RoutedEventArgs e)
        {
            isDarkMode = !isDarkMode;
            ToggleDarkMode(isDarkMode);
        }

        private void ToggleDarkMode(bool dark)
        {
            if (dark)
            {
                this.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1B1E2B"));
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }
        }
    }
}
