using System;
using System.Management;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_ComputerSystemProduct
        /// </summary>
        public class Win32_ComputerSystemProduct : WMIClass
        {
            /// <summary>
            /// Computer <b>U</b>niversally <b>U</b>nique <b>ID</b>entifier (UUID) 
            /// </summary>
            public string UUID { get; private set; }

            /// <summary>
            /// Computer system product name
            /// </summary>
            public string SystemProductName { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT UUID, Name FROM Win32_ComputerSystemProduct";

            public override string WmiClassName => "Win32_ComputerSystemProduct";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher MobjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection MobjCollection = MobjSearcher.Get();

                foreach (ManagementObject a in MobjCollection)
                {
                    UUID = a.GetPropertyValue("UUID") as string ?? string.Empty;
                    SystemProductName = a.GetPropertyValue("Name") as string ?? string.Empty;

                    if (SystemProductName == "System Product Name") SystemProductName = string.Empty;
                }
            }
        }
    }
}