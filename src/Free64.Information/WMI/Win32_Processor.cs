using System;
using System.Management;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_Processor
        /// </summary>
        public class Win32_Processor : WMIClass
        {
            /// <summary>
            /// Name of <b>Central Processing Unit</b>, installed in the PC
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// <b>Central Processing Unit</b> specification (original name, unparsed)
            /// </summary>
            public string Specification { get; private set; }

            /// <summary>
            /// Represents <b>Central Processing Unit</b> stepping number
            /// </summary>
            public byte Stepping { get; private set; }

            /// <summary>
            /// Manufacturer of the <b>Central Processing Unit</b>
            /// </summary>
            public string Manufacturer { get; private set; }

            /// <summary>
            /// <b>Central Processing Unit</b> socket designation
            /// </summary>
            public string SocketDesignation { get; private set; }

            /// <summary>
            /// Maximum clock speed of the <b>Central Processing Unit</b>
            /// </summary>
            public ushort MaxClockSpeed { get; private set; }

            /// <summary>
            /// Number of the <b>Central Processing Unit</b> cores.
            /// </summary>
            public ushort NumberOfCores { get; private set; }

            /// <summary>
            /// Number of the <b>Central Processing Unit</b> threads (logical processors).
            /// </summary>
            public ushort NumberOfLogicalProcessors { get; private set; }

            /// <summary>
            /// Number of <b>Central Processing Units</b> in a system.
            /// </summary>
            public byte NumberOfProcessors { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT Name, Stepping, Manufacturer, SocketDesignation, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher ManObjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection ManObjCollection = ManObjSearcher.Get();

                NumberOfProcessors = (byte)ManObjCollection.Count;

                foreach (ManagementObject a in ManObjCollection)
                {
                    string socketDesignation = a.GetPropertyValue("SocketDesignation") as string ?? string.Empty;
                    string cpuName = a.GetPropertyValue("Name") as string ?? string.Empty;

                    Name = cpuName
                        .Replace("Quad-Core Processor", "")
                        .Replace("Six-Core Processor", "")
                        .Replace("Dual-Core Processor", "")
                        .Replace("Triple-Core Processor", "")
                        .Replace("Eight-Core Processor", "")
                        .Replace("Dual Core Processor", "")
                        .Replace("Quad Core Processor", "")
                        .Replace("12-Core Processor", "")
                        .Replace("16-Core Processor", "")
                        .Replace("24-Core Processor", "")
                        .Replace("32-Core Processor", "")
                        .Replace("64-Core Processor", "")
                        .Replace("6-Core Processor", "")
                        .Replace("8-Core Processor", "")
                        .Replace("with Radeon Vega Mobile Gfx", "")
                        .Replace("w/ Radeon Vega Mobile Gfx", "")
                        .Replace("with Radeon Vega Graphics", "")
                        .Replace("with Radeon Graphics", "")
                        .Replace("APU with Radeon(tm) HD Graphics", "")
                        .Replace("APU with Radeon(TM) HD Graphics", "")
                        .Replace("APU with AMD Radeon R2 Graphics", "")
                        .Replace("APU with AMD Radeon R3 Graphics", "")
                        .Replace("APU with AMD Radeon R4 Graphics", "")
                        .Replace("APU with AMD Radeon R5 Graphics", "")
                        .Replace("APU with Radeon(tm) R3", "")
                        .Replace("RADEON R2, 4 COMPUTE CORES 2C+2G", "")
                        .Replace("RADEON R4, 5 COMPUTE CORES 2C+3G", "")
                        .Replace("RADEON R5, 5 COMPUTE CORES 2C+3G", "")
                        .Replace("RADEON R5, 10 COMPUTE CORES 4C+6G", "")
                        .Replace("RADEON R7, 10 COMPUTE CORES 4C+6G", "")
                        .Replace("RADEON R7, 12 COMPUTE CORES 4C+8G", "")
                        .Replace("Radeon R5, 6 Compute Cores 2C+4G", "")
                        .Replace("Radeon R5, 8 Compute Cores 4C+4G", "")
                        .Replace("Radeon R6, 10 Compute Cores 4C+6G", "")
                        .Replace("Radeon R7, 10 Compute Cores 4C+6G", "")
                        .Replace("Radeon R7, 12 Compute Cores 4C+8G", "")
                        .Replace("R5, 10 Compute Cores 4C+6G", "")
                        .Replace("R7, 12 COMPUTE CORES 4C+8G", "")
                        .Split('@')[0]
                        .Replace("CPU          4", " Duo E4")
                        .Replace("CPU          6", " Duo E6")
                        .Replace("CPU          8", " Duo E8")
                        .Replace("CPU          ", "")
                        .Replace("(TM)2", "(TM) 2")
                        .Replace("   ", "")
                        .Replace(" CPU ", " ")
                        .Replace("(TM)", "™")
                        .Replace("(R)", "®")
                        .Replace("  ", " ")
                        .Split(new string[] { "with" }, StringSplitOptions.RemoveEmptyEntries)[0]
                        .Trim();

                    if (byte.TryParse(a.GetPropertyValue("Stepping") as string, out byte stepping)) Stepping = stepping;

                    Manufacturer = a.GetPropertyValue("Manufacturer") as string ?? string.Empty;

                    if (socketDesignation.CompareTo("CPU") == 0) SocketDesignation = "";
                    else SocketDesignation = socketDesignation.StartsWith("Socket") ? socketDesignation : $"Socket {socketDesignation}";

                    MaxClockSpeed = Convert.ToUInt16(a.GetPropertyValue("MaxClockSpeed"));
                    Specification = cpuName;
                    NumberOfCores = Convert.ToUInt16(a.GetPropertyValue("NumberOfCores"));
                    NumberOfLogicalProcessors = Convert.ToUInt16(a.GetPropertyValue("NumberOfLogicalProcessors"));
                }
            }

            public override string WmiClassName => "Win32_Processor";
        }
    }
}