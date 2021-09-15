using System;
using System.Management;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_BaseBoard
        /// </summary>
        public class Win32_BaseBoard : WMIClass
        {
            /// <summary>
            /// <b>Motherboard</b> unique serial number
            /// </summary>
            public string SerialNumber { get; private set; }

            /// <summary>
            /// Manufacturer of <b>motherboard</b>
            /// </summary>
            public string Manufacturer { get; private set; }

            /// <summary>
            /// Name of the <b>motherboard</b>
            /// </summary>
            public string Product { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT Product, Manufacturer, SerialNumber FROM Win32_BaseBoard";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher MobjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection MobjCollection = MobjSearcher.Get();

                foreach (ManagementObject a in MobjCollection)
                {
                    Manufacturer = ((a.GetPropertyValue("Manufacturer") as string) ?? string.Empty)
                        .Replace("LENOVO", "Lenovo")
                        .Replace("COMPUTER", "Computer")
                        .Replace("INC.", "Inc.");
                    Product = a.GetPropertyValue("Product") as string ?? string.Empty;
                    SerialNumber = a.GetPropertyValue("SerialNumber") as string ?? string.Empty;
                }

                // Fix for some Lenovo Motherboards, that are returning model number in Product string
                if (int.TryParse(Product, out _)) Product = $"Model {Product}";
            }

            public override string WmiClassName => "Win32_BaseBoard";
        }
    }
}