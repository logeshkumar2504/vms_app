# Cursor Dark Mode Color Palette

This document describes the complete Cursor dark mode color palette that has been applied to the VMS application.

## Overview

The entire application now uses the **Cursor IDE Dark Mode** color palette, which is based on Visual Studio Code's Dark+ theme. This provides a comfortable, modern dark interface with excellent readability and contrast.

---

## Core Background Colors

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **Editor Background** | `#1E1E1E` | rgb(30, 30, 30) | Main application background |
| **Sidebar Background** | `#252526` | rgb(37, 37, 38) | Sidebar and surface elements |
| **Title Bar** | `#3C3C3C` | rgb(60, 60, 60) | Top bar and title bars |
| **Card/Input Background** | `#2D2D30` | rgb(45, 45, 48) | Cards, inputs, and secondary surfaces |
| **Panel Border** | `#3E3E42` | rgb(62, 62, 66) | Borders and separators |

---

## Text Colors

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **Primary Text** | `#D4D4D4` | rgb(212, 212, 212) | Main text, labels, content |
| **Secondary Text** | `#858585` | rgb(133, 133, 133) | Hints, placeholders, line numbers |

---

## Accent & Action Colors

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **Primary/Focus Blue** | `#007ACC` | rgb(0, 122, 204) | Primary buttons, focus indicators, links |
| **Keyword Blue** | `#569CD6` | rgb(86, 156, 214) | Secondary actions, highlights |
| **Success Green** | `#6A9955` | rgb(106, 153, 85) | Success messages, positive actions |
| **Warning Orange** | `#CE9178` | rgb(206, 145, 120) | Warnings, alerts |
| **Error Red** | `#F48771` | rgb(244, 135, 113) | Errors, critical alerts |

---

## Interactive States

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **Hover Background** | `#2A2D2E` | rgb(42, 45, 46) | Menu items, cards on hover |
| **Button Hover** | `#1177BB` | rgb(17, 119, 187) | Button hover state |
| **Button Pressed** | `#0E639C` | rgb(14, 99, 156) | Button pressed/active state |

---

## Alarm Level Colors

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **Critical** | `#F48771` | rgb(244, 135, 113) | Critical alarms |
| **High** | `#CE9178` | rgb(206, 145, 120) | High priority alarms |
| **Medium** | `#DCDCAA` | rgb(220, 220, 170) | Medium priority alarms |
| **Low** | `#4EC9B0` | rgb(78, 201, 176) | Low priority alarms |
| **Info** | `#007ACC` | rgb(0, 122, 204) | Informational messages |

---

## Additional UI Colors

| Color Name | Hex Code | RGB | Usage |
|------------|----------|-----|-------|
| **DatePicker Background** | `#3C3C3C` | rgb(60, 60, 60) | DatePicker control background |
| **DatePicker Text** | `#D4D4D4` | rgb(212, 212, 212) | DatePicker text color |

---

## Color Mapping Reference

Here's how Cursor colors map to your application's theme resources:

```csharp
// Cursor Color → Application Resource Key
#1E1E1E → BackgroundColor          // Editor Background
#252526 → SidebarColor/SurfaceColor // Sidebar Background
#3C3C3C → TopBarColor              // Title Bar/Activity Bar
#2D2D30 → CardBackgroundColor      // Input/Card Background
#D4D4D4 → TextPrimaryColor         // Editor Foreground
#858585 → TextSecondaryColor       // Line Numbers
#007ACC → PrimaryColor             // Focus Border/Primary Actions
#569CD6 → SecondaryColor           // Keywords/Highlights
#3E3E42 → BorderColor              // Panel Borders
#2A2D2E → MenuHoverColor           // Hover States
#1177BB → ButtonHoverColor         // Button Hover
#0E639C → ButtonPressedColor       // Button Pressed
```

---

## Implementation

The Cursor color palette has been implemented in:

1. **`ThemeManager.cs`** - Dark theme case updated with Cursor colors
2. **`App.xaml`** - Default application resources set to Cursor colors
3. **`App.xaml.cs`** - Dark theme applied on application startup

All windows and controls automatically inherit these colors through `DynamicResource` bindings, ensuring consistent theming throughout the application.

---

## Theme Switching

Users can switch between themes using the theme toggle in the top bar:
- **Dark Mode** - Uses the Cursor color palette (default)
- **Light Mode** - Light theme for bright environments
- **Blue Mode** - Alternative blue-themed mode

The dark mode will always use the Cursor color palette defined above.

---

## Benefits of the Cursor Color Palette

✅ **Professional & Modern** - Matches the look of popular development tools  
✅ **Excellent Readability** - Carefully chosen contrast ratios  
✅ **Reduced Eye Strain** - Optimal for extended use  
✅ **Consistent Experience** - Familiar to developers using VS Code/Cursor  
✅ **Well-Tested** - Colors proven across millions of users  

---

*Last Updated: October 14, 2025*
*Applied to: VMS Guard Station Application v1.0*

