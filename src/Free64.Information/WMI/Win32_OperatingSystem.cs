using System;
using System.Management;
using System.Linq;
using System.Globalization;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_OperatingSystem
        /// </summary>
        public class Win32_OperatingSystem : WMIClass
        {
            /// <summary>
            /// <b>Operating System</b> caption
            /// </summary>
            public string Caption { get; private set; }

            /// <summary>
            /// Computer name in <b>Operating System</b>
            /// </summary>
            public string ComputerSystemName { get; private set; }

            /// <summary>
            /// <b>Operating System</b> build number
            /// </summary>
            public ushort BuildNumber { get; private set; }

            /// <summary>
            /// <b>Operating System</b> version
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// <b>Operating System</b> install date/time
            /// </summary>
            public DateTime? InstallDate { get; private set; }

            /// <summary>
            /// <b>Operating System</b> kernel build type string.
            /// </summary>
            public string BuildType { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT Caption, CSName, BuildNumber, Version, InstallDate, BuildType FROM Win32_OperatingSystem";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher MobjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection MobjCollection = MobjSearcher.Get();

                foreach (ManagementObject a in MobjCollection)
                {
                    string caption = a.GetPropertyValue("Caption") as string ?? string.Empty;
                    string installDate = a.GetPropertyValue("InstallDate") as string ?? string.Empty;
                    Caption = (!caption.StartsWith("Windows")) ? string.Join(" ", caption.Split(' ').Skip(1)) : caption;
                    ComputerSystemName = a.GetPropertyValue("CSName") as string ?? string.Empty;

                    if (!ushort.TryParse(a.GetPropertyValue("BuildNumber") as string, out ushort build)) throw new DataException(DataException.ExceptionType.InvalidData);
                    BuildNumber = build;
                        
                    Version = a.GetPropertyValue("Version") as string ?? string.Empty;
                    InstallDate = (installDate.Length > 0) ? DateTime.ParseExact(installDate.Split('.')[0], "yyyyMMddHHmmss", CultureInfo.InvariantCulture) : null;
                    BuildType = a.GetPropertyValue("BuildType") as string ?? string.Empty;
                }
            }

            public override string WmiClassName => "Win32_OperatingSystem";
        }
    }
}