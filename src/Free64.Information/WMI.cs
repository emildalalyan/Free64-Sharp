using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Free64.Information
{
    public partial class WMI : IInformationSource
    {
        private readonly ManagementScope COM_CIMV2 = new ManagementScope("root\\CIMV2");
        private readonly ManagementScope COM_WMI = new ManagementScope("root\\WMI");

        /// <summary>
        /// Get ManagementObject property value
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="Property"></param>
        /// <returns name="string"></returns>
        private string GetPropertyValue(ManagementObject Object, string Property)
        {
            try { return Object.GetPropertyValue(Property).ToString(); }
            catch { return ""; }
        }

        /// <summary>
        /// Instance of Free64.Debug.Debug
        /// </summary>
        private readonly Debug.Debug Debug;

        private Win32_OperatingSystem_Information __OperatingSystem;

        /// <summary>
        /// Information from Win32_OperatingSystem
        /// </summary>
        public Win32_OperatingSystem_Information OperatingSystem => __OperatingSystem;

        private Win32_Processor_Information __Processor;

        /// <summary>
        /// Information from Win32_Processor
        /// </summary>
        public Win32_Processor_Information Processor => __Processor;

        private Win32_CacheMemory_Information __CacheMemory;

        /// <summary>
        /// Information from Win32_CacheMemory
        /// </summary>
        public Win32_CacheMemory_Information CacheMemory => __CacheMemory;

        private Win32_BaseBoard_Information __BaseBoard;

        /// <summary>
        /// Information from Win32_BaseBoard
        /// </summary>
        public Win32_BaseBoard_Information Motherboard => __BaseBoard;

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            __OperatingSystem = new Win32_OperatingSystem_Information();
            __Processor = new Win32_Processor_Information();
            __CacheMemory = new Win32_CacheMemory_Information();
            __BaseBoard = new Win32_BaseBoard_Information();
        }

        /// <summary>
        /// <see cref="WMI"/> constructor
        /// </summary>
        public WMI(bool Initialize, Debug.Debug DebugInstance)
        {
            Reset();
            Debug = DebugInstance;
            if (Initialize) this.Initialize();
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
        /// <returns><see cref="List{T}" /> of <see cref="Message"/>s</returns>
        public List<Message> Initialize()
        {
            return new List<Message>
            {
                Win32_OperatingSystem(),
                Win32_Processor(),
                Win32_CacheMemory(),
                Win32_BaseBoard()
            };
        }


        /// <summary>
        /// Collect information from Win32_OperatingSystem class...
        /// </summary>
        /// <returns><see cref="Message"/></returns>
        public Message Win32_OperatingSystem()
        {
            if(Debug != null) Debug.SendMessage("Collecting information from Win32_OperatingSystem class...");
            try
            {
                using(ManagementObjectSearcher ManObjSearcher = new ManagementObjectSearcher(COM_CIMV2, new SelectQuery(Win32_OperatingSystem_Information.Query)))
                {
                    using(ManagementObjectCollection MobjCollection = ManObjSearcher.Get())
                    {
                        foreach (ManagementObject a in MobjCollection)
                        {
                            string Caption = GetPropertyValue(a, "Caption");
                            string InstallDate = GetPropertyValue(a, "InstallDate");
                            __OperatingSystem.Caption = (!Caption.StartsWith("Windows")) ? String.Join(" ", Caption.Split(' ').Skip(1)) : Caption;
                            __OperatingSystem.ComputerSystemName = GetPropertyValue(a, "CSName");
                            __OperatingSystem.BuildNumber = GetPropertyValue(a, "BuildNumber");
                            __OperatingSystem.Version = GetPropertyValue(a, "Version");
                            __OperatingSystem.InstallDate = (InstallDate.Length > 0) ? DateTime.ParseExact(InstallDate.Split('.')[0], "yyyyMMddHHmmFF", System.Globalization.CultureInfo.InvariantCulture) : new DateTime(0);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (Debug != null) Debug.SendMessage($"[Win32_OperatingSystem] {e.Message}...");
                return new Message
                {
                    Exception = e,
                    Class = "Win32_OperatingSystem",
                    Successful = false
                };
            }

            return new Message
            {
                Exception = null,
                Class = "Win32_OperatingSystem",
                Successful = true
            };
        }

        /// <summary>
        /// Collect information from Win32_BaseBoard class...
        /// </summary>
        /// <returns><see cref="Message"/></returns>
        public Message Win32_BaseBoard()
        {
            if (Debug != null) Debug.SendMessage("Collecting information from Win32_BaseBoard class...");
            try
            {
                using (ManagementObjectSearcher MobjSearcher = new ManagementObjectSearcher(COM_CIMV2, new SelectQuery(Win32_BaseBoard_Information.Query)))
                {
                    using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                    {
                        foreach (ManagementObject a in MobjCollection)
                        {
                            __BaseBoard.Manufacturer = GetPropertyValue(a, "Manufacturer")  .Replace("LENOVO", "Lenovo")
                                                                                            .Replace("COMPUTER", "Computer")
                                                                                            .Replace("INC.", "Inc.");
                            __BaseBoard.Product = GetPropertyValue(a, "Product");
                            __BaseBoard.SerialNumber = GetPropertyValue(a, "SerialNumber");
                            
                            break; // There can only be one motherboard, so we breaking loop.
                        }
                    }
                }

                //Fix for some Lenovo Motherboards, that are returning model number in Product string
                if (int.TryParse(__BaseBoard.Product, out _)) __BaseBoard.Product = $"Model {__BaseBoard.Product}";
            }
            catch (Exception e)
            {
                if (Debug != null) Debug.SendMessage($"[Win32_BaseBoard] {e.Message}...");
                return new Message
                {
                    Exception = e,
                    Class = "Win32_BaseBoard",
                    Successful = false
                };
            }

            return new Message
            {
                Exception = null,
                Class = "Win32_BaseBoard",
                Successful = true
            };
        }

        /// <summary>
        /// Collect information from Win32_CacheMemory class...
        /// </summary>
        /// <returns><see cref="Message"/></returns>
        public Message Win32_CacheMemory()
        {
            if (Debug != null) Debug.SendMessage("Collecting information from Win32_CacheMemory class...");
            try
            {
                using(ManagementObjectSearcher MobjSearcher = new ManagementObjectSearcher(COM_CIMV2, new SelectQuery(Win32_CacheMemory_Information.Query)))
                {
                    using (ManagementObjectCollection MobjCollection = MobjSearcher.Get())
                    {
                        __CacheMemory.CacheMemory = new uint[MobjCollection.Count];

                        foreach (ManagementObject a in MobjCollection)
                        {
                            uint DeviceID = Convert.ToUInt32(GetPropertyValue(a, "DeviceID").Replace("Cache Memory ", ""));
                            __CacheMemory.CacheMemory[DeviceID] = Convert.ToUInt32(GetPropertyValue(a, "InstalledSize")) * 1024;
                        }
                    }
                }

                // Fix for some CPUs (such as Intel Atom)
                // Where L1 Cache Size bigger than L2, although it is lie.
                if (CacheMemory.CacheMemory.Length >= 2)
                {
                    if(CacheMemory.CacheMemory[0] > CacheMemory.CacheMemory[1])
                    {
                        uint InstalledSize = __CacheMemory.CacheMemory[0];
                        __CacheMemory.CacheMemory[0] = CacheMemory.CacheMemory[1];
                        __CacheMemory.CacheMemory[1] = InstalledSize;
                    }
                }
            }
            catch (Exception e)
            {
                if (Debug != null) Debug.SendMessage($"[Win32_CacheMemory] {e.Message}...");
                return new Message
                {
                    Exception = e,
                    Class = "Win32_CacheMemory",
                    Successful = false
                };
            }

            return new Message
            {
                Exception = null,
                Class = "Win32_CacheMemory",
                Successful = true
            };
        }

        /// <summary>
        /// Collect information from Win32_Processor class...
        /// </summary>
        /// <returns><see cref="Message"/></returns>
        public Message Win32_Processor()
        {
            if (Debug != null) Debug.SendMessage("Collecting information from Win32_Processor class...");
            try
            {
                using (ManagementObjectSearcher ManObjSearcher = new ManagementObjectSearcher(COM_CIMV2, new SelectQuery(Win32_Processor_Information.Query)))
                {
                    using (ManagementObjectCollection ManObjCollection = ManObjSearcher.Get())
                    {
                        __Processor.NumberOfProcessors = (byte)ManObjCollection.Count;
                        foreach (ManagementObject a in ManObjCollection)
                        {
                            string SocketDesignation = GetPropertyValue(a, "SocketDesignation");
                            string CPUName = GetPropertyValue(a, "Name");
                            __Processor.Name = CPUName.Replace("Quad-Core Processor", "")
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
                                                    .Split(new string[]{ "with" }, StringSplitOptions.RemoveEmptyEntries)[0]
                                                    .Trim();
                            __Processor.Stepping = GetPropertyValue(a, "Stepping");
                            __Processor.Manufacturer = GetPropertyValue(a, "Manufacturer");
                            if (SocketDesignation.CompareTo("CPU") == 0) __Processor.SocketDesignation = "";
                            else __Processor.SocketDesignation = ((SocketDesignation.StartsWith("Socket")) ? SocketDesignation : $"Socket {SocketDesignation}");
                            __Processor.MaxClockSpeed = Convert.ToUInt16(GetPropertyValue(a, "MaxClockSpeed"));
                            __Processor.Specification = CPUName;
                            __Processor.NumberOfCores = Convert.ToUInt16(GetPropertyValue(a, "NumberOfCores"));
                            __Processor.NumberOfLogicalProcessors = Convert.ToUInt16(GetPropertyValue(a, "NumberOfLogicalProcessors"));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (Debug != null) Debug.SendMessage($"[Win32_Processor] {e.Message}...");
                return new Message
                {
                    Class = "Win32_Processor",
                    Exception = e,
                    Successful = false
                };
            }

            return new Message
            {
                Class = "Win32_Processor",
                Exception = null,
                Successful = true
            };
        }
    }
}