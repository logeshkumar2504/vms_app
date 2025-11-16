using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vms_page
{
    public partial class AddUserPopup : Window
    {
        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>();
        private static readonly HttpClient Http = new HttpClient();

        public AddUserPopup()
        {
            InitializeComponent();
            
            // Apply the current theme
            var currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);

            // Bind for roles
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Ensure text is visible in input fields after window is fully loaded
            SetInputFieldTextColors();
            // Load roles from API
            _ = LoadRolesAsync();
        }

        private void SetInputFieldTextColors()
        {
            // Force white text color for all input fields regardless of theme
            var textColor = Brushes.White;
            var fontSize = 10.0;
            var fontFamily = new FontFamily("Segoe UI");
            
            // Set text color for all input fields
            if (UsernameTextBox != null)
            {
                UsernameTextBox.Foreground = textColor;
                UsernameTextBox.FontSize = fontSize;
                UsernameTextBox.FontFamily = fontFamily;
                UsernameTextBox.InvalidateVisual();
            }
            
            if (PasswordBox != null)
            {
                PasswordBox.Foreground = textColor;
                PasswordBox.FontSize = fontSize;
                PasswordBox.FontFamily = fontFamily;
                PasswordBox.InvalidateVisual();
            }
            
            if (ConfirmPasswordBox != null)
            {
                ConfirmPasswordBox.Foreground = textColor;
                ConfirmPasswordBox.FontSize = fontSize;
                ConfirmPasswordBox.FontFamily = fontFamily;
                ConfirmPasswordBox.InvalidateVisual();
            }
            
            if (UserLevelComboBox != null)
            {
                UserLevelComboBox.Foreground = textColor;
                UserLevelComboBox.FontSize = fontSize;
                UserLevelComboBox.FontFamily = fontFamily;
                UserLevelComboBox.InvalidateVisual();
            }
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var response = await Http.GetAsync("http://localhost:8081/roles");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<RoleDto[]>(json, options) ?? [];
                Roles.Clear();
                foreach (var item in items)
                {
                    if (!string.IsNullOrWhiteSpace(item.roles) && !Roles.Contains(item.roles))
                    {
                        Roles.Add(item.roles);
                    }
                }
                if (Roles.Count > 0)
                {
                    UserLevelComboBox.SelectedIndex = 0;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to load roles.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Invalid roles response.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private record RoleDto
        {
            public int r_id { get; init; }
            public string roles { get; init; }
            public int user_id { get; init; }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                UpdatePasswordStrength(passwordBox.Password);
            }
        }

        private void UpdatePasswordStrength(string password)
        {
            // Reset all borders to default
            WeakBorder.Background = new SolidColorBrush(Color.FromRgb(0xE5, 0xE5, 0xEA));
            MediumBorder.Background = new SolidColorBrush(Color.FromRgb(0xE5, 0xE5, 0xEA));
            StrongBorder.Background = new SolidColorBrush(Color.FromRgb(0xE5, 0xE5, 0xEA));

            if (string.IsNullOrEmpty(password))
                return;

            int strength = CalculatePasswordStrength(password);

            if (strength >= 1)
            {
                WeakBorder.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0x3B, 0x30)); // Red
            }
            if (strength >= 2)
            {
                MediumBorder.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00)); // Orange
            }
            if (strength >= 3)
            {
                StrongBorder.Background = new SolidColorBrush(Color.FromRgb(0x34, 0xC7, 0x59)); // Green
            }
        }

        private int CalculatePasswordStrength(string password)
        {
            int score = 0;

            if (password.Length >= 6) score++;
            if (password.Length >= 8) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"[^a-zA-Z0-9]")) score++;

            if (score <= 2) return 1; // Weak
            if (score <= 4) return 2; // Medium
            return 3; // Strong
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                UsernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordBox.Focus();
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                ConfirmPasswordBox.Focus();
                return;
            }

            // Get selected values
            var username = UsernameTextBox.Text.Trim();
            var userLevel = UserLevelComboBox.SelectedItem?.ToString() ?? "Administrator";

            // Show success message
            MessageBox.Show($"User '{username}' has been added successfully!\n\n" +
                          $"User Level: {userLevel}\n" +
                          $"Password Strength: {GetPasswordStrengthText(PasswordBox.Password)}", 
                          "User Added", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);

            // Close the popup
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GetPasswordStrengthText(string password)
        {
            int strength = CalculatePasswordStrength(password);
            switch (strength)
            {
                case 1: return "Weak";
                case 2: return "Medium";
                case 3: return "Strong";
                default: return "Very Weak";
            }
        }

        private void SelectAllPermissions_Click(object sender, RoutedEventArgs e)
        {
            // Find all checkboxes in the permissions list and check them
            var scrollViewer = FindName("PermissionsScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                SetAllCheckBoxes(scrollViewer, true);
            }
        }

        private void ClearAllPermissions_Click(object sender, RoutedEventArgs e)
        {
            // Find all checkboxes in the permissions list and uncheck them
            var scrollViewer = FindName("PermissionsScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                SetAllCheckBoxes(scrollViewer, false);
            }
        }

        private void SetAllCheckBoxes(DependencyObject parent, bool isChecked)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = isChecked;
                }
                else
                {
                    SetAllCheckBoxes(child, isChecked);
                }
            }
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Foreground = Brushes.White;
                textBox.FontSize = 10;
                textBox.FontFamily = new FontFamily("Segoe UI");
                textBox.InvalidateVisual();
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.Foreground = Brushes.White;
                passwordBox.FontSize = 10;
                passwordBox.FontFamily = new FontFamily("Segoe UI");
                passwordBox.InvalidateVisual();
            }
        }

        private void ConfirmPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.Foreground = Brushes.White;
                passwordBox.FontSize = 10;
                passwordBox.FontFamily = new FontFamily("Segoe UI");
                passwordBox.InvalidateVisual();
            }
        }
    }
}
