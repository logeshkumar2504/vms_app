using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Vms_page
{
    public partial class PictureManagementWindow : Window
    {
        public PictureManagementWindow()
        {
            InitializeComponent();
            Owner = Application.Current?.MainWindow;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete action clicked.", "Picture Management", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select Picture",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All Files|*.*"
            };

            if (ofd.ShowDialog(this) == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new System.Uri(ofd.FileName);
                    bitmap.EndInit();
                    bitmap.Freeze();

                    PreviewImage.Source = bitmap;
                    PreviewImage.Visibility = Visibility.Visible;
                    AddPrompt.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("Failed to load image.", "Picture Management", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}


