using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Vms_page
{
    public partial class AddMapWindow : Window
    {
        public AddMapWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Here you can validate and return data
            this.DialogResult = true;
            this.Close();
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
                    MessageBox.Show("Failed to load image.", "Add Map", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}


