using System;
using System.Management;
using System.Collections.Generic;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Public class, that contains information from Win32_CacheMemory
        /// </summary>
        public class Win32_CacheMemory : WMIClass
        {
            /// <summary>
            /// Providing information about cache level
            /// </summary>
            public readonly struct CacheLevel
            {
                /// <summary>
                /// Indicating size of cache level.
                /// Unit of measure is <see cref="byte"/>.
                /// </summary>
                public uint Size { get; init; }

                /// <summary>
                /// Indicating associativity of cache level
                /// </summary>
                public string Associativity { get; init; }
            }

            /// <summary>
            /// Provides strings corresponding to cache level associativity
            /// </summary>
            private readonly Dictionary<ushort, string> CacheAssociativityStrings = new()
            {
                { 3, "Direct Mapped" },
                { 4, "2-way Set-Associative" },
                { 5, "4-way Set-Associative" },
                { 6, "Fully Associative" },
                { 7, "8-way Set-Associative" },
                { 8, "16-way Set-Associative" }
            };

            /// <summary>
            /// Array, storing information of different levels of cache.
            /// </summary>
            public CacheLevel[] CacheInformation { get; private set; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public override string Query => "SELECT InstalledSize, DeviceID, Associativity FROM Win32_CacheMemory";

            protected override void InitializationBody()
            {
                using ManagementObjectSearcher MobjSearcher = new(Scopes.Cimv2, new SelectQuery(Query));

                using ManagementObjectCollection MobjCollection = MobjSearcher.Get();

                CacheInformation = new CacheLevel[MobjCollection.Count];

                foreach (ManagementObject a in MobjCollection)
                {
                    uint deviceId = uint.Parse((a.GetPropertyValue("DeviceID") as string ?? string.Empty).Replace("Cache Memory ", string.Empty));
                    ushort assoc = (ushort)a.GetPropertyValue("Associativity");
                    CacheInformation[deviceId] = new()
                    {
                        Size = Convert.ToUInt32(a.GetPropertyValue("InstalledSize")) * 1024,
                        Associativity = assoc != 0 && CacheAssociativityStrings.ContainsKey(assoc) ? CacheAssociativityStrings[assoc] : string.Empty
                    };
                }

                // Fix for some CPUs (such as Intel Atom)
                // Where L1 Cache Size bigger than L2, although it is lie.
                if (CacheInformation.Length >= 2 && CacheInformation[0].Size > CacheInformation[1].Size) (CacheInformation[0], CacheInformation[1]) = (CacheInformation[1], CacheInformation[0]);
            }

            public override string WmiClassName => "Win32_CacheMemory";
        }
    }
}