using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Vms_page
{
    public partial class EMapWindow : Window
    {
        private bool isEditMode = false;

        public EMapWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            ThemeManager.ApplyTheme(ThemeManager.GetCurrentTheme());
            
            // Default to Edit Map mode on load
            Loaded += (s, e) =>
            {
                isEditMode = true;
                UpdateButtonStyles();
                Sidebar.Visibility = Visibility.Visible;
                SidebarColumn.Width = new GridLength(200);
                InnerNavbar.Visibility = Visibility.Visible;
            };
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to Map view
            isEditMode = false;
            UpdateButtonStyles();
            Sidebar.Visibility = Visibility.Collapsed;
            SidebarColumn.Width = new GridLength(0);
        }

        private void EditMapButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to Edit Map view
            isEditMode = true;
            UpdateButtonStyles();
            Sidebar.Visibility = Visibility.Visible;
            SidebarColumn.Width = new GridLength(200);
            InnerNavbar.Visibility = Visibility.Visible;
        }

        private void UpdateButtonStyles()
        {
            if (isEditMode)
            {
                // Edit Map is active
                MapButton.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
                EditMapButton.Foreground = (System.Windows.Media.Brush)FindResource("PrimaryColor");
                InnerNavbar.Visibility = Visibility.Visible;
            }
            else
            {
                // Map is active
                MapButton.Foreground = (System.Windows.Media.Brush)FindResource("PrimaryColor");
                EditMapButton.Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryColor");
                InnerNavbar.Visibility = Visibility.Collapsed;
            }
        }

        private void EditResourceButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle edit resource functionality
            MessageBox.Show("Edit resource functionality will be implemented here.", "Edit Resource", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteResourceButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle delete resource functionality
            MessageBox.Show("Delete resource functionality will be implemented here.", "Delete Resource", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PictureManagementButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new PictureManagementWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        // Animation now started in XAML with EventTrigger

        private void AddMapMainButton_Click(object sender, RoutedEventArgs e)
        {
            AddMapPopup.Visibility = Visibility.Visible;
        }

        private void AddMapCloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddMapPopup.Visibility = Visibility.Collapsed;
        }

        private void AddMap_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AddMapPopup.Visibility = Visibility.Collapsed;
        }

        private void AddMap_AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle Add Map submission here
            AddMapPopup.Visibility = Visibility.Collapsed;
        }

        private void AddMap_AddPictureButton_Click(object sender, RoutedEventArgs e)
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

                    AddMap_PreviewImage.Source = bitmap;
                    AddMap_PreviewImage.Visibility = Visibility.Visible;
                    AddMap_AddPrompt.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    MessageBox.Show("Failed to load image.", "Add Map", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
