using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Forms;
using System.Globalization;

namespace Free64.Common
{
    /// <summary>
    /// Class, containing various extensions for doing some things.
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// This function uses <see cref="WindowsAPI"/> for hiding maximize and minimize buttons in WPF window.
        /// </summary>
        /// <param name="window"></param>
        public static void HideAdditionalButtons(this Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            WindowsAPI.SetWindowLong(handle, WindowsAPI.WindowParameters.GWL_STYLE, WindowsAPI.GetWindowLong(handle, WindowsAPI.WindowParameters.GWL_STYLE) & ~WindowsAPI.WindowStyles.WS_MAXIMIZEBOX & ~WindowsAPI.WindowStyles.WS_MINIMIZEBOX);
        }

        /// <summary>
        /// Get <b>Unix Time</b> (how many seconds have passed from 1970-1-1 0:00).
        /// </summary>
        /// <returns></returns>
        public static double GetUnixTime(this DateTime datetime) => datetime.Subtract(DateTime.UnixEpoch).TotalSeconds;

        /// <summary>
        /// Enumeration representing data units of measure
        /// </summary>
        public enum DataUnitsOfMeasure
        {
            B, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        /// <summary>
        /// Converts <see cref="uint"/> to readable size of bytes
        /// </summary>
        /// <param name="Bytes"></param>
        public static string SizeInBytes(this uint bytes) => SizeInBytes((ulong)bytes);

        /// <summary>
        /// Converts <see cref="ulong"/> to readable size of bytes
        /// </summary>
        /// <param name="bytes"></param>
        public static string SizeInBytes(this ulong bytes)
        {
            if (bytes < 1024) return $"{bytes} B";

            uint power = (uint)Math.Floor(Math.Log(bytes, 1024));
            return $"{bytes / Math.Pow(1024, power)} {Enum.GetName(typeof(DataUnitsOfMeasure), power)}";
        }

        /// <summary>
        /// Close all <see cref="Form"/>s in the <see cref="FormCollection"/>
        /// </summary>
        public static void DisposeAll(this FormCollection collection)
        {
            for (int i = 0; i < collection.Count; ++i)
            {
                collection[i]?.Dispose();
            }
        }

        /// <summary>
        /// If <see cref="string"/> is <see langword="null"/>, then returns "N/A", otherwise returns <see cref="string"/>
        /// </summary>
        /// <param name="Str"></param>
#nullable enable
        public static string IfNullReturnNA(this string? str) => str switch
        {
            null or "" => "N/A",
            _ => str
        };
#nullable disable

        /// <summary>
        /// If <see cref="ushort"/> value equals 0, then returns "N/A", otherwise returns <see cref="ushort"/> converted to <see cref="string"/>
        /// </summary>
        /// <param name="Value"></param>
        public static string IfZeroReturnNA(this ushort value) => value switch
        {
            0 => "N/A",
            _ => value.ToString(CultureInfo.InvariantCulture)
        };
    }
}
