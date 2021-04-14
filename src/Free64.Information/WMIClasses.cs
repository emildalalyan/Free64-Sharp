using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Free64.Information
{
    public sealed partial class WMI : IInformationSource
    {
        /// <summary>
        /// Abstract <see langword="class"/>, representing WMI classes
        /// </summary>
        public abstract class WMIClass
        {
            /// <summary>
            /// Gather information from class
            /// </summary>
            /// <param name="scope"></param>
            /// <returns><see cref="Exception"/>? instance</returns>
            public abstract Exception Initialize();

            /// <summary>
            /// It is scope, that needed for connection with WMI
            /// </summary>
            public virtual ManagementScope Scope { get; init; }
        }

        #region Windows Management Instrumentation Classes

        /// <summary>
        /// Public class, that contains information from Win32_ComputerSystemProduct
        /// </summary>
        public class Win32_ComputerSystemProduct : WMIClass
        {
            public Win32_ComputerSystemProduct(ManagementScope scope)
            {
                Scope = scope;
            }

            /// <summary>
            /// Computer UUID
            /// </summary>
            public string UUID { get; private set; }

            /// <summary>
            /// Computer system product name
            /// </summary>
            public string SystemProductName { get; private set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT UUID, Name FROM Win32_ComputerSystemProduct";

            /// <summary>
            /// Collect information from Win32_OperatingSystem class...
            /// </summary>
            /// <returns><see cref="Exception"/></returns>
            public override Exception Initialize()
            {
                Trace.WriteLine("module> Collecting information from Win32_ComputerSystemProduct class...");
                try
                {
                    if (Scope is null) throw new ArgumentNullException(nameof(Scope), "Scope is not provided. Execution has stopped.");
                    using (ManagementObjectSearcher MobjSearcher = new(Scope, new SelectQuery(Query)))
                    {
                        using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                        {
                            foreach (ManagementObject a in MobjCollection)
                            {
                                UUID = a.GetPropertyValue("UUID") as string ?? string.Empty;
                                SystemProductName = a.GetPropertyValue("Name") as string ?? string.Empty;

                                if (SystemProductName == "System Product Name") SystemProductName = string.Empty;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"module> [Win32_ComputerSystemProduct] {e.Message}...");
                    return e;
                }

                return null;
            }
        }

        /// <summary>
        /// Public class, that contains information from Win32_OperatingSystem
        /// </summary>
        public class Win32_OperatingSystem : WMIClass
        {
            public Win32_OperatingSystem(ManagementScope scope)
            {
                Scope = scope;
            }

            /// <summary>
            /// Operating System Caption
            /// </summary>
            public string Caption { get; private set; }

            /// <summary>
            /// Computer Name in OS
            /// </summary>
            public string ComputerSystemName { get; private set; }

            /// <summary>
            /// Operating System Build Number
            /// </summary>
            public ushort BuildNumber { get; private set; }

            /// <summary>
            /// Operating System Version
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// Operating System Install Date
            /// </summary>
            public DateTime InstallDate { get; private set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Caption, CSName, BuildNumber, Version, InstallDate FROM Win32_OperatingSystem";

            /// <summary>
            /// Collect information from Win32_OperatingSystem class...
            /// </summary>
            /// <returns><see cref="Exception"/></returns>
            public override Exception Initialize()
            {
                Trace.WriteLine("module> Collecting information from Win32_OperatingSystem class...");
                try
                {
                    if (Scope is null) throw new ArgumentNullException(nameof(Scope), "Scope is not provided. Execution has stopped.");
                    using (ManagementObjectSearcher MobjSearcher = new ManagementObjectSearcher(Scope, new SelectQuery(Query)))
                    {
                        using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                        {
                            foreach (ManagementObject a in MobjCollection)
                            {
                                string caption = a.GetPropertyValue("Caption") as string ?? string.Empty;
                                string installDate = a.GetPropertyValue("InstallDate") as string ?? string.Empty;
                                Caption = (!caption.StartsWith("Windows")) ? string.Join(" ", caption.Split(' ').Skip(1)) : caption;
                                ComputerSystemName = a.GetPropertyValue("CSName") as string ?? string.Empty;
                                _ = ushort.TryParse(a.GetPropertyValue("BuildNumber") as string, out ushort build);
                                BuildNumber = build;
                                Version = a.GetPropertyValue("Version") as string ?? string.Empty;
                                InstallDate = (installDate.Length > 0) ? DateTime.ParseExact(installDate.Split('.')[0], "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture) : new DateTime(0);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"module> [Win32_OperatingSystem] {e.Message}...");
                    return e;
                }

                return null;
            }
        }

        /// <summary>
        /// Public class, that contains information from Win32_BaseBoard
        /// </summary>
        public class Win32_BaseBoard : WMIClass
        {
            public Win32_BaseBoard(ManagementScope scope)
            {
                Scope = scope;
            }

            /// <summary>
            /// Motherboard Serial Number
            /// </summary>
            public string SerialNumber { get; private set; }

            /// <summary>
            /// Motherboard manufacturer
            /// </summary>
            public string Manufacturer { get; private set; }

            /// <summary>
            /// Motherboard Name
            /// </summary>
            public string Product { get; private set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Product, Manufacturer, SerialNumber FROM Win32_BaseBoard";

            /// <summary>
            /// Collect information from Win32_BaseBoard class...
            /// </summary>
            /// <returns><see cref="Exception"/></returns>
            public override Exception Initialize()
            {
                Trace.WriteLine("module> Collecting information from Win32_BaseBoard class...");

                try
                {
                    if (Scope is null) throw new ArgumentNullException(nameof(Scope), "Scope is not provided. Execution has stopped.");
                    using (ManagementObjectSearcher MobjSearcher = new ManagementObjectSearcher(Scope, new SelectQuery(Query)))
                    {
                        using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                        {
                            foreach (ManagementObject a in MobjCollection)
                            {
                                Manufacturer = ((a.GetPropertyValue("Manufacturer") as string) ?? string.Empty)
                                    .Replace("LENOVO", "Lenovo")
                                    .Replace("COMPUTER", "Computer")
                                    .Replace("INC.", "Inc.");
                                Product = a.GetPropertyValue("Product") as string ?? string.Empty;
                                SerialNumber = a.GetPropertyValue("SerialNumber") as string ?? string.Empty;

                                break; // There can only be one motherboard, so we're breaking loop.
                            }
                        }
                    }

                    // Fix for some Lenovo Motherboards, that are returning model number in Product string
                    if (int.TryParse(Product, out _)) Product = $"Model {Product}";
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"module> [Win32_BaseBoard] {e.Message}...");
                    return e;
                }

                return null;
            }
        }

        /// <summary>
        /// Public class, that contains information from Win32_CacheMemory
        /// </summary>
        public class Win32_CacheMemory : WMIClass
        {
            public Win32_CacheMemory(ManagementScope scope)
            {
                Scope = scope;
            }

            /// <summary>
            /// Array, storing size of different levels of cache.
            /// Unit of measure is <see cref="byte"/>.
            /// </summary>
            public uint[] CacheMemory { get; private set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT InstalledSize, DeviceID FROM Win32_CacheMemory";

            /// <summary>
            /// Collect information from Win32_CacheMemory class...
            /// </summary>
            /// <returns><see cref="Exception"/></returns>
            public override Exception Initialize()
            {
                Trace.WriteLine("module> Collecting information from Win32_CacheMemory class...");
                try
                {
                    if (Scope is null) throw new ArgumentNullException(nameof(Scope), "Scope is not provided. Execution has stopped.");
                    using (ManagementObjectSearcher MobjSearcher = new ManagementObjectSearcher(Scope, new SelectQuery(Query)))
                    {
                        using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                        {
                            CacheMemory = new uint[MobjCollection.Count];

                            foreach (ManagementObject a in MobjCollection)
                            {
                                uint DeviceID = Convert.ToUInt32((a.GetPropertyValue("DeviceID") as string ?? string.Empty).Replace("Cache Memory ", ""));
                                CacheMemory[DeviceID] = Convert.ToUInt32(a.GetPropertyValue("InstalledSize")) * 1024;
                            }
                        }
                    }

                    // Fix for some CPUs (such as Intel Atom)
                    // Where L1 Cache Size bigger than L2, although it is lie.
                    if (CacheMemory.Length >= 2 && CacheMemory[0] > CacheMemory[1])
                    {
                        (CacheMemory[0], CacheMemory[1]) = (CacheMemory[1], CacheMemory[0]);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"module> [Win32_CacheMemory] {e.Message}...");
                    return e;
                }

                return null;
            }
        }

        /// <summary>
        /// Public class, that contains information from Win32_Processor
        /// </summary>
        public class Win32_Processor : WMIClass
        {
            public Win32_Processor(ManagementScope scope)
            {
                Scope = scope;
            }

            /// <summary>
            /// Name of the CPU
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// CPU Specification (Original name, returned by WMI)
            /// </summary>
            public string Specification { get; private set; }

            /// <summary>
            /// Processor Stepping
            /// </summary>
            public byte Stepping { get; private set; }

            /// <summary>
            /// Processor Manufacturer
            /// </summary>
            public string Manufacturer { get; private set; }

            /// <summary>
            /// CPU Socket Designation
            /// </summary>
            public string SocketDesignation { get; private set; }

            /// <summary>
            /// Processor Maximum Clock Speed
            /// </summary>
            public ushort MaxClockSpeed { get; private set; }

            /// <summary>
            /// CPU Number of Cores
            /// </summary>
            public ushort NumberOfCores { get; private set; }

            /// <summary>
            /// Number of Logical Processors
            /// </summary>
            public ushort NumberOfLogicalProcessors { get; private set; }

            /// <summary>
            /// Number of processors in a system.
            /// </summary>
            public byte NumberOfProcessors { get; private set; }

            /// <summary>
            /// WQL Query
            /// </summary>
            public const string Query = "SELECT Name, Stepping, Manufacturer, SocketDesignation, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors FROM Win32_Processor";

            /// <summary>
            /// Collect information from Win32_Processor class...
            /// </summary>
            /// <returns><see cref="Exception"/></returns>
            public override Exception Initialize()
            {
                Trace.WriteLine("module> Collecting information from Win32_Processor class...");
                try
                {
                    if (Scope is null) throw new ArgumentNullException(nameof(Scope), "Scope is not provided. Execution has stopped.");
                    using (ManagementObjectSearcher ManObjSearcher = new ManagementObjectSearcher(Scope, new SelectQuery(Query)))
                    {
                        using (ManagementObjectCollection ManObjCollection = ManObjSearcher.Get())
                        {
                            NumberOfProcessors = (byte)ManObjCollection.Count;
                            foreach (ManagementObject a in ManObjCollection)
                            {
                                string socketDesignation = a.GetPropertyValue("SocketDesignation") as string ?? string.Empty;
                                string CPUName = a.GetPropertyValue("Name") as string ?? string.Empty;
                                Name = CPUName
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
                                if(byte.TryParse(a.GetPropertyValue("Stepping") as string, out byte stepping))
                                {
                                    Stepping = stepping;
                                }
                                Manufacturer = a.GetPropertyValue("Manufacturer") as string ?? string.Empty;
                                if (socketDesignation.CompareTo("CPU") == 0) SocketDesignation = "";
                                else SocketDesignation = socketDesignation.StartsWith("Socket") ? socketDesignation : $"Socket {socketDesignation}";
                                MaxClockSpeed = Convert.ToUInt16(a.GetPropertyValue("MaxClockSpeed"));
                                Specification = CPUName;
                                NumberOfCores = Convert.ToUInt16(a.GetPropertyValue("NumberOfCores"));
                                NumberOfLogicalProcessors = Convert.ToUInt16(a.GetPropertyValue("NumberOfLogicalProcessors"));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"module> [Win32_Processor] {e.Message}...");
                    return e;
                }

                return null;
            }
        }
        #endregion
    }
}
