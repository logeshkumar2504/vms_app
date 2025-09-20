using System.Configuration;
using System.Data;
using System.Windows;

namespace Vms_page
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                
                // Apply dark theme by default
                ThemeManager.ApplyTheme("Dark");
            }
            catch (System.Exception ex)
            {
                // Log the error and continue with default theme
                System.Diagnostics.Debug.WriteLine($"Error applying theme: {ex.Message}");
            }
        }
    }
}
