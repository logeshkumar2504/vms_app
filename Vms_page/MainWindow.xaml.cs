using System.Windows;

namespace GuardStationUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("Main Window Loaded");
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
    }
}
