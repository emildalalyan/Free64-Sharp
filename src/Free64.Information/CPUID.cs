using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace Free64.Information
{
    /// <summary>
    /// Class intended to gathering information with x86 <b>CPUID</b> instruction.
    /// </summary>
    public sealed class CPUID : IInformationSource
    {
        /// <summary>
        /// Length of processor name <see cref="string"/>
        /// </summary>
        public const int ProcessorNameLength = 48;

        private static void GetCpuId(out uint reax, out uint rebx, out uint recx, out uint redx, uint address, uint subleaf)
        {
            (reax, rebx, recx, redx) = ((uint, uint, uint, uint))X86Base.CpuId(unchecked((int)address), unchecked((int)subleaf));
        }

        private static void GetCpuId(out uint reax, out uint rebx, out uint recx, out uint redx, uint address)
        {
            GetCpuId(out reax, out rebx, out recx, out redx, address, 0);
        }

        /// <summary>
        /// Array of common x86 instruction set extensions.
        /// </summary>
        public static readonly string[] CommonExtensions =
        {
            "MMX", "SSE", "SSE2", "SSE3", "SSSE3", "SSE4.1", "SSE4.2", "SSE4A", "AVX", "AVX2", "3DNow!", "Extended 3DNow!", "FMA3", "AES", "MMX+", "FMA4", "SHA", "VMX", "AMD-V"
        };

        /// <summary>
        /// Clear all information in class <see cref="CPUID"/>
        /// </summary>
        public void Reset()
        {
            Instructions = Array.Empty<InstructionSetExtension>();
            Capabilities = new();
            ProcessorCacheInfo = Array.Empty<CacheLevel>();
        }

        /// <summary>
        /// Creates an instance of the <see cref="CPUID"/> class.
        /// </summary>
        /// <param name="Initialize"></param>
        public CPUID(bool initialize = false)
        {
            if (!X86Base.IsSupported) throw new NotSupportedException("This CPU doesn't support CPUID instruction.");

            Reset();

            if (initialize) this.Initialize();
        }

        /// <summary>
        /// Initialize <see cref="CPUID"/> class (get information).
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception?[] Initialize()
        {
            Trace.WriteLine("Initializing Free64.Information.CPUID class...");

            return new Exception?[]
            {
                GetExtensions(),
                GetProcessorNameString(),
                GetProcessorCapabilities(),
                GetProcessorCaches()
            };
        }
#nullable disable

        /// <summary>
        /// <see cref="Array"/>, representing instruction set extensions of CPU
        /// </summary>
        public InstructionSetExtension[] Instructions { get; private set; }

        /// <summary>
        /// Indicates what are processor capabilities.
        /// </summary>
        public CpuCapabilities Capabilities { get; private set; }

        /// <summary>
        /// Structure, which indicates what are processor capabilities.
        /// </summary>
        public readonly struct CpuCapabilities
        {
            /// <summary>
            /// <b>For Intel processors only!</b> Indicates whether processor supports <b>Intel Turbo Boost</b> or not.
            /// </summary>
            public bool IsTurboBoostSupported { get; init; }

            /// <summary>
            /// Indicates, whether processor supports <b>Digital Thermal Sensor</b> or not.
            /// </summary>
            public bool IsDtsSupported { get; init; }

            /// <summary>
            /// Indicates, whether processor supports <b>Package Thermal Management</b> or not.
            /// </summary>
            public bool IsPtmSupported { get; init; }
        }

        /// <summary>
        /// Processor name, that was gathered from CPUID
        /// </summary>
        public string ProcessorName { get; private set; }

        /// <summary>
        /// Processor original name (specification), that was gathered from CPUID
        /// </summary>
        public string ProcessorSpecification { get; private set; }

        /// <summary>
        /// Represents struct, which contain information about <b>Central Processing Unit</b> cache itself
        /// </summary>
        public readonly struct CacheLevel
        {
            /// <summary>
            /// Size of the cache. Measure is <b>byte</b>. This property is <b>0</b> by default.
            /// </summary>
            public ulong Size { get; init; }

            /// <summary>
            /// Number of cache associativity ways. This property is <b>0</b> by default.
            /// </summary>
            public ushort Associativity { get; init; }

            /// <summary>
            /// Number of processor threads, which are sharing cache. Value of this property cannot be less than <b>1</b>.
            /// </summary>
            public ushort ThreadsSharingCache { get; init; }
        }

        /// <summary>
        /// Represents information about <b>Central Processing Unit</b> cache itself
        /// </summary>
        public CacheLevel[] ProcessorCacheInfo { get; private set; }

        /// <summary>
        /// <see langword="Readonly"/> structure, representing processor instruction set extension
        /// </summary>
        public readonly struct InstructionSetExtension
        {
            /// <summary>
            /// Name of this instruction set extension. This property must be filled.
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Name of the instruction set extension must be provided.")]
            public string Name { get; init; }

            /// <summary>
            /// Whether processor <b>has</b> this instruction set extension or <b>not</b>. This property is <see langword="false"/> by default.
            /// </summary>
            public bool Support { get; init; }

            /// <summary>
            /// Returns the name of this instruction set extension.
            /// </summary>
            /// <returns></returns>
            public override string ToString() => Name;
        }

        /// <summary>
        /// Get information about supporting of <see cref="InstructionSetExtension"/>s
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception? GetExtensions()
#nullable disable
        {
            try
            {
                Trace.WriteLine("\tCollecting instruction set extensions support from CPUID...");

                uint reax = 0, rebx = 0, recx = 0, redx = 0;
                
                List<InstructionSetExtension> list = new();

                GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.ProcessorFeatures);

                list.AddRange(new InstructionSetExtension[]
                {
                    new() { Name = "MMX",               Support = (redx >> 23 & 1) == 1 },
                    new() { Name = "SSE",               Support = (redx >> 25 & 1) == 1 },
                    new() { Name = "SSE2",              Support = (redx >> 26 & 1) == 1 },
                    new() { Name = "SSE3",              Support = (recx & 1) == 1 },
                    new() { Name = "SSSE3",             Support = (recx >> 9 & 1) == 1 },
                    new() { Name = "SSE4.1",            Support = (recx >> 19 & 1) == 1 },
                    new() { Name = "SSE4.2",            Support = (recx >> 23 & 1) == 1 },
                    new() { Name = "AVX",               Support = (recx >> 28 & 1) == 1 },
                    new() { Name = "VMX",               Support = (recx >> 5 & 1) == 1 },
                    new() { Name = "SMX",               Support = (recx >> 6 & 1) == 1 },
                    new() { Name = "EIST",              Support = (recx >> 7 & 1) == 1 },
                    new() { Name = "Thermal Monitor 2", Support = (recx >> 8 & 1) == 1 },
                    new() { Name = "FMA3",              Support = (recx >> 12 & 1) == 1 },
                    new() { Name = "AES",               Support = (recx >> 25 & 1) == 1 },
                    new() { Name = "FPU",               Support = (redx & 1) == 1 },
                    new() { Name = "VME",               Support = (redx >> 1 & 1) == 1 },
                    new() { Name = "PSE",               Support = (redx >> 3 & 1) == 1 },
                    new() { Name = "TSC",               Support = (redx >> 4 & 1) == 1 },
                    new() { Name = "MSR",               Support = (redx >> 5 & 1) == 1 },
                    new() { Name = "PAE",               Support = (redx >> 6 & 1) == 1 },
                    new() { Name = "MCE",               Support = (redx >> 7 & 1) == 1 },
                    new() { Name = "APIC",              Support = (redx >> 9 & 1) == 1 },
                    new() { Name = "MTRR",              Support = (redx >> 12 & 1) == 1 },
                    new() { Name = "PGE",               Support = (redx >> 13 & 1) == 1 },
                    new() { Name = "MCA",               Support = (redx >> 14 & 1) == 1 },
                    new() { Name = "CMOV",              Support = (redx >> 15 & 1) == 1 },
                    new() { Name = "PSE-36",            Support = (redx >> 17 & 1) == 1 },
                    new() { Name = "Self-Snoop",        Support = (redx >> 27 & 1) == 1 },
                    new() { Name = "PBE",               Support = (redx >> 31 & 1) == 1 }
                });

                GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.ExtendedProcessorFeatures);

                list.AddRange(new InstructionSetExtension[]
                {
                    new() { Name = "AVX2",              Support = (rebx >> 5 & 1) == 1 },
                    new() { Name = "SHA",               Support = (rebx >> 29 & 1) == 1 }
                });

                GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.AmdInfo);

                list.AddRange(new InstructionSetExtension[]
                {
                    new() { Name = "MMX+",              Support = (redx >> 22 & 1) == 1 },
                    new() { Name = "3DNow!",            Support = (redx >> 31 & 1) == 1 },
                    new() { Name = "Extended 3DNow!",   Support = (redx >> 30 & 1) == 1 },
                    new() { Name = "SSE4A",             Support = (recx >> 6 & 1) == 1 },
                    new() { Name = "FMA4",              Support = (recx >> 16 & 1) == 1 },
                    new() { Name = "AMD-V",             Support = (recx >> 2 & 1) == 1 },
                });

                Instructions = list.ToArray();
            }
            catch (Exception e)
            {
                Trace.WriteLine($"\t[GetExtensions()] {e.Message}...");
                return e;
            }

            return null;
        }

        /// <summary>
        /// Gets information about <b>Central Processing Unit</b> caches from CPUID.
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception? GetProcessorCaches()
#nullable disable
        {
            try
            {
                Trace.WriteLine("\tCollecting information about cache from CPUID...");

                uint reax = 0, rebx = 0, recx = 0, redx = 0;

                GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.AmdInfo);

                uint address = (((recx >> 22) & 1) == 1) ? (uint)Leaves.AmdCacheTopology : (uint)Leaves.IntelCacheTopology;

                List<CacheLevel> list = new();
                
                for (uint i = 0; ; ++i)
                {
                    GetCpuId(out reax, out rebx, out recx, out redx, address, i);
                    if ((reax & 31) == 0) break;

                    uint ways    = ((rebx >> 22) & 1023) + 1;
                    uint parts   = ((rebx >> 12) & 1023) + 1;
                    uint lsize   = (rebx & 4095) + 1;
                    uint sets    = recx + 1;
                    
                    uint sharing = ((reax >> 14) & 4095) + 1;

                    list.Add(new()
                    {
                        Size = (ulong)ways * parts * lsize * sets,
                        Associativity = (ushort)ways,
                        ThreadsSharingCache = (ushort)sharing
                    });
                }

                ProcessorCacheInfo = list.ToArray();
            }
            catch(Exception e)
            {
                Trace.WriteLine($"\t[GetProcessorCaches()] {e.Message}...");
                return e;
            }
            return null;
        }

        /// <summary>
        /// Gets <b>Central Processing Unit</b> capabilities with CPUID instruction.
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception? GetProcessorCapabilities()
#nullable disable
        {
            try
            {
                Trace.WriteLine("\tCollecting processor capabilities from CPUID...");

                uint reax = 0, rebx = 0, recx = 0, redx = 0;

                GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.ThermalManagement);

                Capabilities = new()
                {
                    IsTurboBoostSupported   = ((reax >> 1) & 1) == 1,
                    IsDtsSupported          = (reax & 1) == 1,
                    IsPtmSupported          = ((reax >> 6) & 1) == 1
                };
            }
            catch (Exception e)
            {
                Trace.WriteLine($"\t[GetProcessorCapabilities()] {e.Message}...");

                return e;
            }

            return null;
        }

        /// <summary>
        /// Get <see cref="ProcessorName"/> and <see cref="ProcessorSpecification"/> strings from CPUID
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception? GetProcessorNameString()
#nullable disable
        {
            try
            {
                Trace.WriteLine("\tCollecting processor name string from CPUID...");

                List<byte> namechars = new(ProcessorNameLength);

                uint reax = 0, rebx = 0, recx = 0, redx = 0;

                for (uint i = 0; i <= 2; ++i)
                {
                    GetCpuId(out reax, out rebx, out recx, out redx, (uint)Leaves.CpuNameString + i);

                    namechars.AddRange(new byte[]
                    {
                        (byte)((reax) & byte.MaxValue),
                        (byte)((reax >> 8) & byte.MaxValue),
                        (byte)((reax >> 16) & byte.MaxValue),
                        (byte)((reax >> 24) & byte.MaxValue),
                        (byte)((rebx) & byte.MaxValue),
                        (byte)((rebx >> 8) & byte.MaxValue),
                        (byte)((rebx >> 16) & byte.MaxValue),
                        (byte)((rebx >> 24) & byte.MaxValue),
                        (byte)((recx) & byte.MaxValue),
                        (byte)((recx >> 8) & byte.MaxValue),
                        (byte)((recx >> 16) & byte.MaxValue),
                        (byte)((recx >> 24) & byte.MaxValue),
                        (byte)((redx) & byte.MaxValue),
                        (byte)((redx >> 8) & byte.MaxValue),
                        (byte)((redx >> 16) & byte.MaxValue),
                        (byte)((redx >> 24) & byte.MaxValue)
                    });
                }

                ProcessorSpecification = Encoding.ASCII.GetString(namechars.ToArray());

                if(ProcessorSpecification.Length < 1 || ProcessorSpecification[0] == '\0')
                {
                    ProcessorSpecification = string.Empty;
                    ProcessorName = string.Empty;

                    throw new DataException(DataException.ExceptionType.InvalidData);
                }

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
                    .Split("with")[0]
                    .Trim();
            }
            catch (Exception e)
            {
                Trace.WriteLine($"\t[GetProcessorNameString()] {e.Message}...");
                return e;
            }

            return null;
        }

        /// <summary>
        /// Enumeration of CPUID leaves, they're needed to be provided for collecting information from CPUID.
        /// <para>You can read more about 'em here: <u>http://en.wikipedia.org/wiki/CPUID</u></para>    
        /// </summary>
        public enum Leaves : uint
        {
            /// <summary>
            /// Information about <b>Intel thermal management</b> features.
            /// </summary>
            ThermalManagement = 0x6u,

            /// <summary>
            /// Various information about <b>AMD processor</b>
            /// </summary>
            AmdInfo = 0x80000001u,

            /// <summary>
            /// Information about cache on <b>AMD processor</b>
            /// </summary>
            AmdCacheTopology = 0x8000001Du,

            /// <summary>
            /// Information about cache on <b>Intel processor</b>
            /// </summary>
            IntelCacheTopology = 0x4u,

            /// <summary>
            /// This leaf contains <b>CPU name</b>, this is base leaf address, rest of name contains in <b>next 2 leaves</b>.
            /// <para>Each character is <b>8-bit wide</b>, they're containing in registers EAX, EBX, ECX, EDX (in this order).</para>
            /// </summary>
            CpuNameString = 0x80000002u,

            /// <summary>
            /// Contains information about <b>supporting of various instruction set extensions.</b>
            /// </summary>
            ProcessorFeatures = 0x1u,

            /// <summary>
            /// Contains information about <b>supporting of specific instruction set extensions.</b>
            /// </summary>
            ExtendedProcessorFeatures = 0x7u
        }
    }
}
