using System;
using System.Runtime.Versioning;
using Xunit;

namespace Free64.Information.Tests
{
    /// <summary>
    /// Class intended to testing <see cref="Information.CPUID"/>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class CPUID
    {
        /// <summary>
        /// Compare collected CPU name from <see cref="CPUID"/> to expected
        /// </summary>
        /// <param name="expected"></param>
        [Theory]
        [InlineData("AMD Ryzen 5 2600 Six-Core Processor            ")]
        public void TestProcessorName(string expected)
        {
            Information.CPUID cpuid = new();

            Assert.True(cpuid.GetProcessorNameString() is null);
            
            Assert.True(cpuid.ProcessorSpecification.CompareTo(expected) == 0);
        }
    }
}
