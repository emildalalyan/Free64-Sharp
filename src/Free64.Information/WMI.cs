using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Free64.Information
{
    /// <summary>
    /// Class intended to gathering information from <b>W</b>indows <b>M</b>anagement <b>I</b>nstrumentation.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed partial class WMI : IMultiThreadedInformationSource
    {
        /// <summary>
        /// Storages all neccessary <see cref="ManagementScope"/>s
        /// </summary>
        public static class Scopes
        {
            /// <summary>
            /// Represents "<b>root\CIMV2</b>" <see cref="ManagementScope"/>
            /// </summary>
            public static readonly ManagementScope Cimv2 = new("root\\CIMV2");

            /// <summary>
            /// Represents "<b>root\WMI</b>" <see cref="ManagementScope"/>
            /// </summary>
            public static readonly ManagementScope Wmi = new("root\\WMI");
        }

        /// <summary>
        /// Information from Win32_OperatingSystem class
        /// </summary>
        public Win32_OperatingSystem OperatingSystem { get; private set; }
        
        /// <summary>
        /// Information from Win32_Processor class
        /// </summary>
        public Win32_Processor Processor { get; private set; }

        /// <summary>
        /// Information from Win32_CacheMemory class
        /// </summary>
        public Win32_CacheMemory CacheMemory { get; private set; }

        /// <summary>
        /// Information from Win32_BaseBoard class
        /// </summary>
        public Win32_BaseBoard Motherboard { get; private set; }

        /// <summary>
        /// Information from Win32_ComputerSystemProduct class
        /// </summary>
        public Win32_ComputerSystemProduct ComputerSystemProduct { get; private set; }

        /// <summary>
        /// Information from Win32_BIOS class
        /// </summary>
        public Win32_BIOS Bios { get; private set; }

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            OperatingSystem =           new();
            Processor =                 new();
            CacheMemory =               new();
            Motherboard =               new();
            ComputerSystemProduct =     new();
            Bios =                      new();
        }

        /// <summary>
        /// <see cref="WMI"/> constructor
        /// </summary>
        public WMI(bool initialize = false)
        {
            Reset();
            if (initialize) this.Initialize();
        }

        /// <summary>
        /// Initialize <see cref="WMI"/> class (gather <b>all</b> information)
        /// </summary>
        /// <returns><see cref="List{T}" />, where T <see langword="is"/> <see cref="Exception"/></returns>
#nullable enable
        public Exception?[] Initialize()
        {
            Trace.WriteLine("Initializing Free64.Information.WMI class...");

            return new Exception?[]
            {
                OperatingSystem.Initialize(),
                Processor.Initialize(),
                CacheMemory.Initialize(),
                Motherboard.Initialize(),
                ComputerSystemProduct.Initialize(),
                Bios.Initialize()
            };
        }
#nullable disable

        /// <summary>
        /// Asynchronous initialize <see cref="WMI"/> class (gather <b>all</b> information)
        /// </summary>
        /// <returns><see cref="Array"/> of <see cref="Task"/>s</returns>
#nullable enable
        public Task<Exception?>[] InitializeAsync()
        {
            Trace.WriteLine("Initializing Free64.Information.WMI class (async mode)...");

            return new Task<Exception?>[]
            {
                OperatingSystem.InitializeAsync(),
                Processor.InitializeAsync(),
                CacheMemory.InitializeAsync(),  
                Motherboard.InitializeAsync(),
                ComputerSystemProduct.InitializeAsync(),
                Bios.InitializeAsync()
            };
        }
#nullable disable
    }
}