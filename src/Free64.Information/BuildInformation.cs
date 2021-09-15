using System;
using System.Diagnostics;
using System.Reflection;
using Free64.Information;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;

namespace Free64.Information
{
    /// <summary>
    /// Static class, which provide information about this build of <b>Free64.Information</b>.
    /// </summary>
    public static class BuildInformation
    {
        /// <summary>
        /// Class, which contain imported external methods
        /// </summary>
        internal static class NativeMethods
        {
            /// <summary>
            /// Get build date from interoped function from low-level library.
            /// </summary>
            /// <param name="datestr"></param>
            [DllImport(LowLevelInteropHelper.PathToLowLevelLibrary)]
            internal static extern int GetBuildDate(IntPtr datestr);

            /// <summary>
            /// Length of the build date string, outgoing from <see cref="GetBuildDate(IntPtr)"/>.
            /// </summary>
            internal const int BuildDateStringLength = 11+1;
        }

        /// <summary>
        /// Date, when build of <b>Free64.Information</b> was compiled. Returns <see langword="default"/> if attempt of build date gathering was failed.
        /// </summary>
        public static DateTime BuildDate
        {
            get
            {
                try
                {
                    IntPtr datestr = Marshal.AllocHGlobal(NativeMethods.BuildDateStringLength);

                    if (NativeMethods.GetBuildDate(datestr) == -1) throw new InvalidOperationException("GetBuildDate(IntPtr) returned -1.");

                    string date = Marshal.PtrToStringAnsi(datestr)?.Replace("  ", " ");

                    Marshal.FreeHGlobal(datestr);

                    if (string.IsNullOrEmpty(date)) throw new DataException(DataException.ExceptionType.ParameterNotFound);

                    return DateTime.ParseExact(date, "MMM d yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"[GetBuildDate()] Ñannot get build date, so it will be {default(DateTime).ToString("d MMMM yyyy", CultureInfo.InvariantCulture)} by default.");
                    Trace.WriteLine($"[GetBuildDate()] Message of the exception: {e.Message}");
                }

                return default;
            }
        }
    }
}