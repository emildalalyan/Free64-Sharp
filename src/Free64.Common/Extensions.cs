using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Free64.Common
{
    public static class Extensions
    {
        /// <summary>
        /// This function uses <see cref="WindowsAPI"/> for hiding Maximize and Minimize buttons in WPF window.
        /// </summary>
        /// <param name="window"></param>
        public static int HideAdditionalButtons(this Window Window)
        {
            IntPtr Handle = new WindowInteropHelper(Window).Handle;
            return WindowsAPI.SetWindowLong(Handle, WindowsAPI.Parameters.GWL_STYLE, (WindowsAPI.GetWindowLong(Handle, WindowsAPI.Parameters.GWL_STYLE) & ~(int)WindowsAPI.WindowStyles.WS_MAXIMIZEBOX & ~(int)WindowsAPI.WindowStyles.WS_MINIMIZEBOX));
        }

        /// <summary>
        /// If <see cref="string"/> is null, then returns "N/A", otherwise returns <see cref="string"/> itself
        /// </summary>
        /// <param name="Str"></param>
        public static string IfNullReturnNA(this string Str)
        {
            if (Str == null || Str.Length < 1) return "N/A";
            return Str;
        }

        /// <summary>
        /// If <see cref="ushort"/> less than 1, then returns "N/A", otherwise returns <see cref="ushort"/> converted to <see cref="string"/>
        /// </summary>
        /// <param name="Value"></param>
        public static string IfNullReturnNA(this ushort Value)
        {
            if (Value < 1) return "N/A";
            return Value.ToString();
        }

        /// <summary>
        /// Converts <see cref="uint"/> to readable size of bytes
        /// </summary>
        /// <param name="Bytes"></param>
        public static string SizeInBytes(this uint Bytes)
        {
            if (Bytes >= 1024 && Bytes < 1048576)
            {
                return $"{Bytes / 1024} KB";
            }
            if (Bytes >= 1048576 && Bytes < 1073741824)
            {
                return $"{Bytes / 1048576} MB";
            }
            if (Bytes >= 1073741824)
            {
                return $"{Bytes / 1073741824} GB";
            }

            return $"{Bytes} B";
        }
    }
}
