using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;

namespace Free64.Information
{
    /// <summary>
    /// Class intended for gathering information from CPUID
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class CPUID : IInformationSource
    {
        /// <summary>
        /// Class, that contains imported external methods
        /// </summary>
        private static class NativeMethods
        {
            [DllImport("Free64.GetCPUID.dll", EntryPoint = "GetInstructionSet", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe bool GetInstructionSet(bool* arr);

            [DllImport("Free64.GetCPUID.dll", EntryPoint = "GetProcessorNameString", CallingConvention = CallingConvention.Cdecl)]
            public static extern unsafe void GetProcessorNameString(byte* value);

            /// <summary>
            /// Total count of instruction sets
            /// </summary>
            public static readonly int InstructionsCount = Enum.GetNames(typeof(InstructionSets)).Length;

            /// <summary>
            /// Length of processor name <see cref="string"/>
            /// </summary>
            public const byte ProcessorNameLength = 48;
        }

        /// <summary>
        /// Array of SIMD (Single Instruction, Multiple Data) Extensions.
        /// </summary>
        public static readonly string[] SIMDExtensions =
        {
            "MMX", "SSE", "SSE2", "SSE3", "SSSE3", "SSE4_1", "SSE4_2", "SSE4A", "AVX", "AVX2", "AMD3DNow", "AMD3DNowExt", "FMA3", "AES", "MMX_plus", "FMA4", "SHA", "VMX", "AMD_V"
        };

        /// <summary>
        /// Clear all information in class <see cref="CPUID"/>
        /// </summary>
        public void Reset()
        {
            Instructions = new InstructionSet[NativeMethods.InstructionsCount];
        }

        /// <summary>
        /// <see cref="CPUID"/> constructor
        /// </summary>
        /// <param name="Initialize"></param>
        public CPUID(bool Initialize = false)
        {
            Reset();
            if (Initialize) this.Initialize();
        }

        /// <summary>
        /// Initialize <see cref="CPUID"/> class (get information).
        /// </summary>
        /// <returns></returns>
        public List<Exception> Initialize()
        {
            Trace.WriteLine("Initializing Free64.Information.CPUID class...");
            return new()
            {
                GetInstructionSet(),
                GetProcessorNameString()
            };
        }

        /// <summary>
        /// <see cref="Array"/>, representing instruction sets of CPU
        /// </summary>
        public InstructionSet[] Instructions { get; private set; }

        /// <summary>
        /// Processor name, that was gathered from CPUID
        /// </summary>
        public string ProcessorName { get; private set; }

        /// <summary>
        /// Processor original name (specification), that was gathered from CPUID
        /// </summary>
        public string ProcessorSpecification { get; private set; }

        /// <summary>
        /// Structure, representing instruction set
        /// </summary>
        public readonly struct InstructionSet
        {
            /// <summary>
            /// Name of instruction set.
            /// </summary>
            public string Name { get; init; }

            /// <summary>
            /// Processor instruction set support 
            /// </summary>
            public bool Support { get; init; }
        }

        /// <summary>
        /// Get information about supporting of <see cref="InstructionSets"/>
        /// </summary>
        /// <returns></returns>
        public Exception GetInstructionSet()
        {
            try
            {
                Trace.WriteLine("module> Collecting instruction set support from CPUID...");
                unsafe
                {
                    fixed (bool* arr = stackalloc bool[NativeMethods.InstructionsCount])
                    {
                        if (!NativeMethods.GetInstructionSet(arr)) throw new InvalidOperationException();
                        
                        for (int i = 0; i < NativeMethods.InstructionsCount; ++i)
                        {
                            Instructions[i] = new()
                            {
                                Support = arr[i],
                                Name = Enum.GetName(typeof(InstructionSets), i)
                            };
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine($"module> [GetInstructionSet()] {e.Message}...");
                return e;
            }

            return null;
        }

        /// <summary>
        /// Get processor name string from CPUID
        /// </summary>
        /// <returns></returns>
        public Exception GetProcessorNameString()
        {
            try
            {
                Trace.WriteLine("module> Collecting processor name string from CPUID...");
                unsafe
                {
                    fixed(byte* name = stackalloc byte[NativeMethods.ProcessorNameLength])
                    {
                        NativeMethods.GetProcessorNameString(name);
                        ProcessorSpecification = Encoding.ASCII.GetString(name, NativeMethods.ProcessorNameLength);
                        ProcessorName = ProcessorSpecification
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
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"module> [GetProcessorNameString()] {e.Message}...");
                return e;
            }

            return null;
        }

        /// <summary>
        /// Enumeration of possible processor instruction sets
        /// </summary>
        public enum InstructionSets : byte
        {
            MMX,
            SSE,
            SSE2,
            SSE3,
            SSSE3,
            SSE4_1,
            SSE4_2,
            AVX,
            VMX,
            SMX,
            EIST,
            TM2,
            FMA3,
            AES,
            FPU,
            VME,
            PSE,
            TSC,
            MSR,
            PAE,
            MCE,
            APIC,
            MTRR,
            PGE,
            MCA,
            CMOV,
            PSE_36,
            Self_Snoop,
            PBE,
            AVX2,
            SHA,
            MMX_plus,
            AMD3DNow,
            AMD3DNowExt,
            SSE4A,
            FMA4,
            AMD_V
        }
    }
}
