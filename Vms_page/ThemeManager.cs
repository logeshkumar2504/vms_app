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
                    appResources["MenuHoverColor"] = new SolidColorBrush(Color.FromRgb(0xE8, 0xE8, 0xED)); // Subtle gray for hover in light mode
                    // DatePicker colors for light mode
                    appResources["DatePickerBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    appResources["DatePickerTextColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00));
                    // Alarm Level Colors for light mode
                    appResources["AlarmCriticalColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x3B, 0x30));
                    appResources["AlarmHighColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0x95, 0x00));
                    appResources["AlarmMediumColor"] = new SolidColorBrush(Color.FromRgb(0xFF, 0xD6, 0x0A));
                    appResources["AlarmLowColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xFF));
                    appResources["AlarmInfoColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xFF));
                    // Button Hover States for light mode
                    appResources["ButtonHoverColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x5B, 0xD8));
                    appResources["ButtonPressedColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x4A, 0xB8));
                    break;
                    
                case "Dark":
                    // Cursor Dark Mode Color Palette
                    appResources["PrimaryColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC)); // #007ACC - Focus Blue
                    appResources["SecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6)); // #569CD6 - Keywords Blue
                    appResources["SuccessColor"] = new SolidColorBrush(Color.FromRgb(0x6A, 0x99, 0x55)); // #6A9955 - Comments Green
                    appResources["WarningColor"] = new SolidColorBrush(Color.FromRgb(0xCE, 0x91, 0x78)); // #CE9178 - Strings Orange
                    appResources["ErrorColor"] = new SolidColorBrush(Color.FromRgb(0xF4, 0x87, 0x71)); // #F48771 - Error Red
                    appResources["BackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x1E)); // #1E1E1E - Editor Background
                    appResources["SurfaceColor"] = new SolidColorBrush(Color.FromRgb(0x25, 0x25, 0x26)); // #252526 - Sidebar Background
                    appResources["SidebarColor"] = new SolidColorBrush(Color.FromRgb(0x25, 0x25, 0x26)); // #252526 - Sidebar Background
                    appResources["TopBarColor"] = new SolidColorBrush(Color.FromRgb(0x3C, 0x3C, 0x3C)); // #3C3C3C - Title Bar
                    appResources["CardBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x2D, 0x2D, 0x30)); // #2D2D30 - Input/Card Background
                    appResources["TextPrimaryColor"] = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD4)); // #D4D4D4 - Editor Foreground
                    appResources["TextSecondaryColor"] = new SolidColorBrush(Color.FromRgb(0x85, 0x85, 0x85)); // #858585 - Line Numbers
                    appResources["BorderColor"] = new SolidColorBrush(Color.FromRgb(0x3E, 0x3E, 0x42)); // #3E3E42 - Panel Border
                    appResources["MenuHoverColor"] = new SolidColorBrush(Color.FromRgb(0x2A, 0x2D, 0x2E)); // #2A2D2E - Hover Background
                    // DatePicker colors for dark mode
                    appResources["DatePickerBackgroundColor"] = new SolidColorBrush(Color.FromRgb(0x3C, 0x3C, 0x3C));
                    appResources["DatePickerTextColor"] = new SolidColorBrush(Color.FromRgb(0xD4, 0xD4, 0xD4));
                    // Alarm Level Colors (using Cursor palette variants)
                    appResources["AlarmCriticalColor"] = new SolidColorBrush(Color.FromRgb(0xF4, 0x87, 0x71)); // Error Red
                    appResources["AlarmHighColor"] = new SolidColorBrush(Color.FromRgb(0xCE, 0x91, 0x78)); // Orange
                    appResources["AlarmMediumColor"] = new SolidColorBrush(Color.FromRgb(0xDC, 0xDC, 0xAA)); // Yellow
                    appResources["AlarmLowColor"] = new SolidColorBrush(Color.FromRgb(0x4E, 0xC9, 0xB0)); // Teal
                    appResources["AlarmInfoColor"] = new SolidColorBrush(Color.FromRgb(0x00, 0x7A, 0xCC)); // Info Blue
                    // Button Hover States
                    appResources["ButtonHoverColor"] = new SolidColorBrush(Color.FromRgb(0x11, 0x77, 0xBB)); // #1177BB - Button Hover
                    appResources["ButtonPressedColor"] = new SolidColorBrush(Color.FromRgb(0x0E, 0x63, 0x9C)); // #0E639C - Button Pressed
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

