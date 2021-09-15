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
        /// Processor specification, which has been gathered from <see cref="Information.CPUID"/> will compared to this.
        /// </summary>
        public const string ProcessorSpecification = "AMD Ryzen 5 2600 Six-Core Processor            ";

        /// <summary>
        /// Compare collected CPU name from <see cref="CPUID"/> to expected
        /// </summary>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(ProcessorSpecification)]
        public void TestProcessorSpecification(string expected)
        {
            Information.CPUID cpuid = new();

            Assert.True(cpuid.GetProcessorNameString() is null);
            
            Assert.True(cpuid.ProcessorSpecification.CompareTo(expected) == 0);
        }
    }
}
