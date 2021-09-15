using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_BIOS
        /// </summary>
        public class Win32_BIOS : WMIClass
        {
            /// <summary>
            /// Manufacturer of the <b>BIOS</b>/<b>UEFI</b> software
            /// </summary>
            public string Manufacturer { get; private set; }

            /// <summary>
            /// Release date-time of the <b>BIOS</b>/<b>UEFI</b> software
            /// </summary>
            public DateTime? ReleaseDate { get; private set; }

            /// <summary>
            /// Version of the <b>BIOS</b>/<b>UEFI</b> software
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT ReleaseDate, Manufacturer, SMBIOSBIOSVersion FROM Win32_BIOS";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher MobjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection MobjCollection = MobjSearcher.Get();

                foreach (ManagementObject a in MobjCollection)
                {
                    Manufacturer = a.GetPropertyValue("Manufacturer") as string ?? string.Empty;

                    ReleaseDate = a.GetPropertyValue("ReleaseDate") is not string rdate || rdate.Length < 1 ? null : DateTime.ParseExact(rdate.Split('.')[0], "yyyyMMddhhmmss", CultureInfo.InvariantCulture);

                    Version = a.GetPropertyValue("SMBIOSBIOSVersion") as string ?? string.Empty;
                }
            }

            public override string WmiClassName => "Win32_BIOS";
        }
    }
}