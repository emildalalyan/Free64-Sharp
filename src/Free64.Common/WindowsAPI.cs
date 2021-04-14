using System;
using System.Runtime.InteropServices;

namespace Free64.Common
{
    /// <summary>
    /// Class, providing part of Windows API for .NET
    /// </summary>
    public static class WindowsAPI
    {
        /// <summary>
        /// Class, that contains imported external methods
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            public static extern int GetWindowLong(IntPtr hWnd, Parameters nIndex);

            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            public static extern int SetWindowLong(IntPtr hWnd, Parameters nIndex, int dwNewLong);
        }

        /// <summary>
        /// Windows API GetWindowLong function.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public static int GetWindowLong(IntPtr Handle, Parameters Parameters) => NativeMethods.GetWindowLong(Handle, Parameters);

        /// <summary>
        /// Windows API SetWindowLong function.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        public static int SetWindowLong(IntPtr Handle, Parameters Parameters, int NewLong) => NativeMethods.SetWindowLong(Handle, Parameters, NewLong);

        /// <summary>
        /// Enumeration of parameters, which can be written in extra window memory
        /// </summary>
        public enum Parameters
        {
            GWL_EXSTYLE = -20,
            GWL_STYLE = -16,
            GWL_HINSTANCE = -6,
            GWL_ID = -12,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWL_MSGRESULT = 0
        }

        /// <summary>
        /// Window styles, which can be written into extra window memory.
        /// </summary>
        public enum WindowStyles
        {
            WS_BORDER = 0x00800000,
            WS_CAPTION = 0x00C00000,
            WS_CHILD = 0x40000000,
            WS_CHILDWINDOW = 0x40000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_DISABLED = 0x08000000,
            WS_DLGFRAME = 0x00400000,
            WS_GROUP = 0x00020000,
            WS_HSCROLL = 0x00100000,
            WS_ICONIC = 0x20000000,
            WS_MAXIMIZE = 0x01000000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_OVERLAPPED = 0x00000000,
            WS_SIZEBOX = 0x00040000,
            WS_SYSMENU = 0x00080000,
            WS_TABSTOP = 0x00010000,
            WS_THICKFRAME = 0x00040000,
            WS_TILED = 0x00000000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x00200000
        }
    }
}
