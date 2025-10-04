using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vms_page
{
    public partial class UserManagementWindow : Window
    {
        private ObservableCollection<UserModel> _users;
        private List<UserModel> _selectedUsers;

        public UserManagementWindow()
        {
            InitializeComponent();
            
            // Apply the current theme
            var currentTheme = ThemeManager.GetCurrentTheme();
            ThemeManager.ApplyTheme(currentTheme);
            
            // Initialize data
            InitializeData();
            
            // Set placeholder text for search box
            SearchTextBox.Text = "Search Keywords";
            SearchTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            
            // Add focus events to handle placeholder behavior
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            SearchTextBox.LostFocus += SearchTextBox_LostFocus;
        }

        private void InitializeData()
        {
            _selectedUsers = new List<UserModel>();
            
            // Initialize user data matching the image
            _users = new ObservableCollection<UserModel>
            {
                new UserModel { Username = "admin", UserLevel = "Super Administrator" },
                new UserModel { Username = "logesh", UserLevel = "Administrator" }
            };
            
            UserDataGrid.ItemsSource = _users;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addUserPopup = new AddUserPopup();
            addUserPopup.Owner = this;
            addUserPopup.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUsers.Count == 0)
            {
                MessageBox.Show("Please select users to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_selectedUsers.Count} selected user(s)?", 
                                       "Delete Users", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                foreach (var user in _selectedUsers.ToList())
                {
                    _users.Remove(user);
                }
                _selectedUsers.Clear();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Search Keywords")
                {
                    textBox.Foreground = System.Windows.Media.Brushes.Gray;
                }
                else
                {
                    // Use theme-aware text color
                    var currentTheme = ThemeManager.GetCurrentTheme();
                    if (currentTheme == "Dark")
                    {
                        textBox.Foreground = System.Windows.Media.Brushes.White;
                    }
                    else
                    {
                        textBox.Foreground = System.Windows.Media.Brushes.Black;
                    }
                }
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Search Keywords")
            {
                textBox.Text = "";
                // Use theme-aware text color
                var currentTheme = ThemeManager.GetCurrentTheme();
                if (currentTheme == "Dark")
                {
                    textBox.Foreground = System.Windows.Media.Brushes.White;
                }
                else
                {
                    textBox.Foreground = System.Windows.Media.Brushes.Black;
                }
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search Keywords";
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Checkbox event handlers
        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _selectedUsers.Clear();
            foreach (UserModel user in _users)
            {
                _selectedUsers.Add(user);
            }
        }

        private void SelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _selectedUsers.Clear();
        }

        private void RowCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox?.Tag is UserModel user && !_selectedUsers.Contains(user))
            {
                _selectedUsers.Add(user);
            }
        }

        private void RowCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox?.Tag is UserModel user)
            {
                _selectedUsers.Remove(user);
            }
        }

        // Operation button event handlers
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is UserModel user)
            {
                MessageBox.Show($"Edit user: {user.Username}", "Edit User", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is UserModel user)
            {
                MessageBox.Show($"Change password for user: {user.Username}", "Change Password", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is UserModel user)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", 
                                           "Delete User", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    _users.Remove(user);
                    _selectedUsers.Remove(user);
                }
            }
        }

        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        // Top bar event handlers
        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            // Lock/Unlock functionality
            MessageBox.Show("Lock/Unlock functionality not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Main menu functionality
            MessageBox.Show("Main menu functionality not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle theme popup
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = !ThemePopup.IsOpen;
            }
        }

        private void LightMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Light");
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = false;
            }
        }

        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.ApplyTheme("Dark");
            if (ThemePopup != null)
            {
                ThemePopup.IsOpen = false;
            }
        }
    }

    // User model class
    public class UserModel
    {
        public string Username { get; set; }
        public string UserLevel { get; set; }
    }
}
