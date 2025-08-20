using System;
using System.Runtime.InteropServices;

namespace Vms_page
{
    // Shared native methods to control window behavior
    internal static class NativeMethods
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_APPWINDOW = 0x40000;
        public const int WS_EX_TOOLWINDOW = 0x80;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
