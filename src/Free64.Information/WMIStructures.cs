using System;

namespace Free64.Information
{
    public partial class WMI : IInformationSource
    {
        /// <summary>
        /// Public struct, that contain information from Win32_OperatingSystem
        /// </summary>
        public struct Win32_OperatingSystem_Information
        {
            /// <summary>
            /// Operating System Caption
            /// </summary>
            public string Caption { get; internal set; }

            /// <summary>
            /// Computer Name in OS
            /// </summary>
            public string ComputerSystemName { get; internal set; }

            /// <summary>
            /// Operating System Build Number
            /// </summary>
            public string BuildNumber { get; internal set; }

            /// <summary>
            /// Operating System Version
            /// </summary>
            public string Version { get; internal set; }

            /// <summary>
            /// Operating System Install Date
            /// </summary>
            public DateTime InstallDate { get; internal set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Caption, CSName, BuildNumber, Version, InstallDate FROM Win32_OperatingSystem";
        }

        /// <summary>
        /// Public struct, that contain information from Win32_BaseBoard
        /// </summary>
        public struct Win32_BaseBoard_Information
        {
            /// <summary>
            /// Motherboard Serial Number
            /// </summary>
            public string SerialNumber { get; internal set; }

            /// <summary>
            /// Motherboard manufacturer
            /// </summary>
            public string Manufacturer { get; internal set; }

            /// <summary>
            /// Motherboard Name
            /// </summary>
            public string Product { get; internal set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Product, Manufacturer, SerialNumber FROM Win32_BaseBoard";
        }

        /// <summary>
        /// Public struct, that contain information from Win32_CacheMemory
        /// </summary>
        public struct Win32_CacheMemory_Information
        {
            /// <summary>
            /// Array, storing size of different levels of cache.
            /// Unit of measure is byte.
            /// </summary>
            public uint[] CacheMemory { get; internal set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT InstalledSize, DeviceID FROM Win32_CacheMemory";
        }

        /// <summary>
        /// Public struct, that contain information from Win32_Processor
        /// </summary>
        public struct Win32_Processor_Information
        {
            /// <summary>
            /// Name of the CPU
            /// </summary>
            public string Name { get; internal set; }

            /// <summary>
            /// CPU Specification (Original name, returned by WMI)
            /// </summary>
            public string Specification { get; internal set; }

            /// <summary>
            /// Processor Stepping
            /// </summary>
            public string Stepping { get; internal set; }

            /// <summary>
            /// Processor Manufacturer
            /// </summary>
            public string Manufacturer { get; internal set; }

            /// <summary>
            /// CPU Socket Designation
            /// </summary>
            public string SocketDesignation { get; internal set; }

            /// <summary>
            /// Processor Maximum Clock Speed
            /// </summary>
            public ushort MaxClockSpeed { get; internal set; }

            /// <summary>
            /// CPU Number of Cores
            /// </summary>
            public ushort NumberOfCores { get; internal set; }

            /// <summary>
            /// Number of Logical Processors
            /// </summary>
            public ushort NumberOfLogicalProcessors { get; internal set; }

            /// <summary>
            /// Number of processors in a system.
            /// </summary>
            public byte NumberOfProcessors { get; internal set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Name, Stepping, Manufacturer, SocketDesignation, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor";
        }
    }
}
