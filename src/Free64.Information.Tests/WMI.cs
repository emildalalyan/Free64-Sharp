using System;
using System.Runtime.Versioning;
using Xunit;

namespace Free64.Information.Tests
{
    /// <summary>
    /// Class intended to testing <see cref="Information.WMI"/>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WMI
    {
        /// <summary>
        /// Test <see cref="Information.WMI.WMIClass"/>es instances, it is null or not.
        /// </summary>
        [Fact]
        public void TestClassInstances()
        {
            Information.WMI wmi = new();

            CheckInstances();

            wmi.Initialize();

            CheckInstances();

            void CheckInstances()
            {
                Assert.False(wmi.Processor is null);
                Assert.False(wmi.CacheMemory is null);
                Assert.False(wmi.Motherboard is null);
                Assert.False(wmi.OperatingSystem is null);
                Assert.False(wmi.ComputerSystemProduct is null);
            }
        }

        /// <summary>
        /// Test <see cref="Information.WMI"/> exceptions
        /// </summary>
        [Fact]
        public void TestExceptions()
        {
            foreach (Exception e in new Information.WMI().Initialize())
            {
                Assert.True(e is null);
            }
        }
    }
}
