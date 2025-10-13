using System.Windows;
using System.Windows.Media;

namespace Vms_page
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            // Apply theme-specific colors to the APPLICATION resources (global)
            var appResources = Application.Current.Resources;
            
            switch (themeName)
            {
                case "Light":
                    appResources["PrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xFF));
                    appResources["SecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x58, 0x56, 0xD6));
                    appResources["SuccessColor"] = new SolidColorBrush(Color.FromRgb(0x34, 0xC7, 0x59));
                    appResources["WarningColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                    appResources["ErrorColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x3B, 0x30));
                    appResources["BackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xF2, 0xF2, 0xF7));
                    appResources["SurfaceColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["SidebarColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["TopBarColor"] = new SolidColorBrush(Color.FromRgb(0xF5, 0xF5, 0xF5)); // White with grey merged
                    appResources["CardBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xFA, 0xFA, 0xFA)); // White with grey mixed
                    appResources["TextPrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x1C, 0x1C, 0x1E));
                    appResources["TextSecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x8E, 0x8E, 0x93));
                    appResources["BorderColor"] = new SolidColorBrush(Color.FromRgb(0xE5, 0xE5, 0xEA));
                    appResources["MenuHoverColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    // DatePicker colors for light mode
                    appResources["DatePickerBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["DatePickerTextColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                    break;
                    
                case "Dark":
                    // User-specified dark palette (updated to blue accent)
                    appResources["PrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x25, 0x63, 0xEB)); // #2563EB
                    appResources["SecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x5E, 0x5C, 0xE6));
                    appResources["SuccessColor"] = new SolidColorBrush(Color.FromRgb(0x30, 0xD1, 0x58));
                    appResources["WarningColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x9F, 0x0A));
                    appResources["ErrorColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x45, 0x3A));
                    appResources["BackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x0F, 0x0F, 0x23)); // Darker background
                    appResources["SurfaceColor"] = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x2E)); // Slightly lighter surface
                    appResources["SidebarColor"] = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x38)); // Sidebar color #242438
                    appResources["TopBarColor"] = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x38)); // Same as CardBackgroundColor
                    appResources["CardBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x2A, 0x2A, 0x3E)); // Different color for cards
                    appResources["TextPrimaryColor"] = new SolidColorBrush(Color.FromRgb(0xE2, 0xE8, 0xF0)); // Brighter text for better readability
                    appResources["TextSecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x94, 0xA3, 0xB8));
                    appResources["BorderColor"] = new SolidColorBrush(Color.FromRgb(0x3B, 0x3B, 0x4F)); // Subtle border for cards
                    appResources["MenuHoverColor"] = new SolidColorBrush(Color.FromRgb(0x4A, 0x4A, 0x5E)); // Hover color for menu items and cards
                    // DatePicker colors for dark mode
                    appResources["DatePickerBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                    appResources["DatePickerTextColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    // Alarm Level Colors
                    appResources["AlarmCriticalColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x45, 0x3A));
                    appResources["AlarmHighColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x9F, 0x0A));
                    appResources["AlarmMediumColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xD6, 0x0A));
                    appResources["AlarmLowColor"] = new SolidColorBrush(Color.FromRgb(0x64, 0xD2, 0xFF));
                    appResources["AlarmInfoColor"] = new SolidColorBrush(Color.FromRgb(0x25, 0x63, 0xEB));
                    // Button Hover States
                    appResources["ButtonHoverColor"] = new SolidColorBrush(Color.FromRgb(0x5E, 0x5C, 0xE6));
                    appResources["ButtonPressedColor"] = new SolidColorBrush(Color.FromRgb(0x4A, 0x4A, 0xBF));
                    break;
                    
                case "Blue":
                    appResources["PrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xFF));
                    appResources["SecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x0F, 0x5B, 0xD8));
                    appResources["SuccessColor"] = new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81));
                    appResources["WarningColor"] = new SolidColorBrush(Color.FromRgb(0xF5, 0x9E, 0x0B));
                    appResources["ErrorColor"] = new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44));
                    appResources["BackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xF0, 0xF8, 0xFF));
                    appResources["SurfaceColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["SidebarColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["TextPrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x1E, 0x3A, 0x8A));
                    appResources["TextSecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8B));
                    appResources["BorderColor"] = new SolidColorBrush(Color.FromRgb(0xE2, 0xE8, 0xF0));
                    // DatePicker colors for blue theme (light-like defaults)
                    appResources["DatePickerBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["DatePickerTextColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                    break;
            }
            
            // Store the current 
            //  name for future windows
            appResources["CurrentTheme"] = themeName;
        }

        public static string GetCurrentTheme()
        {
            var appResources = Application.Current.Resources;
            if (appResources.Contains("CurrentTheme") && appResources["CurrentTheme"] is string theme)
            {
                return theme;
            }
            return "Light"; // Default theme
        }
    }
}

