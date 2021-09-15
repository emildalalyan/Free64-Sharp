using System.Runtime.InteropServices;

namespace Free64.Information
{
    /// <summary>
    /// Class, which help to determine <b>current operating system platform</b>.
    /// </summary>
    public static class Platform
    {
        /// <summary>
        /// Gets string, describing current operating system platform.
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformString()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return "windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return "macos";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return "linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) return "freebsd";
            
            return "generic";
        }
    }
}
