using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Free64.Information
{
    /// <summary>
    /// Class intended to gathering information from WMI (Windows Management Instrumentation)
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed partial class WMI : IInformationSource
    {
        private readonly ManagementScope ScopeCIMv2 = new("root\\CIMV2");
        
        private readonly ManagementScope ScopeWMI = new("root\\WMI");

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
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            OperatingSystem = new Win32_OperatingSystem(ScopeCIMv2);
            Processor = new Win32_Processor(ScopeCIMv2);
            CacheMemory = new Win32_CacheMemory(ScopeCIMv2);
            Motherboard = new Win32_BaseBoard(ScopeCIMv2);
            ComputerSystemProduct = new Win32_ComputerSystemProduct(ScopeCIMv2);
        }

        /// <summary>
        /// <see cref="WMI"/> constructor
        /// </summary>
        public WMI(bool Initialize = false)
        {
            Reset();
            if (Initialize) this.Initialize();
        }

        /// <summary>
        /// Initialize <see cref="WMI"/> class
        /// </summary>
        /// <returns><see cref="List{T}" /> of <see cref="Exception"/>s</returns>
        public List<Exception> Initialize()
        {
            Trace.WriteLine("Initializing Free64.Information.WMI class...");

            return new()
            {
                OperatingSystem.Initialize(),
                Processor.Initialize(),
                CacheMemory.Initialize(),
                Motherboard.Initialize(),
                ComputerSystemProduct.Initialize()
            };
        }
    }
}