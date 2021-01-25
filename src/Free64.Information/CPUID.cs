using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Free64.Information
{
    public class CPUID : IInformationSource
    {
        [DllImport("Free64.GetCPUID.dll", EntryPoint = "GetInstructionSet", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __GetInstructionSet(int frag);

        public const byte InstructionsCount = 35;

        public static readonly string[] SIMDExtensions =
        {
            "MMX", "SSE", "SSE2", "SSE3", "SSSE3", "SSE4_1", "SSE4_2", "SSE4A", "AVX", "AVX2", "AMD3DNow", "AMD3DNowExt", "FMA3", "MMX_plus", "FMA4"
        };

        private readonly Debug.Debug Debug;

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            Instructions = new InstructionSet[InstructionsCount];
        }

        /// <summary>
        /// <see cref="CPUID"/> constructor
        /// </summary>
        /// <param name="Initialize"></param>
        /// <param name="DebugInstance"></param>
        public CPUID(bool Initialize, Debug.Debug DebugInstance)
        {
            Reset();
            Debug = DebugInstance;
            if (Initialize) this.Initialize();
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
        /// Initialize class (get information).
        /// </summary>
        /// <returns></returns>
        public List<Message> Initialize()
        {
            return new List<Message>()
            {
                GetInstructionSet()
            };
        }

        public InstructionSet[] Instructions;

        public struct InstructionSet
        {
            public string Name;
            public bool Support;

            public override string ToString()
            {
                return Name + (Support ? " is supported" : " is not supported");
            }
        }

        public Message GetInstructionSet()
        {
            try
            {
                if (Debug != null) Debug.SendMessage("Collecting information from CPUID...");
                for (int i = 0; i < CPUID.InstructionsCount; i++)
                {
                    Instructions[i].Support = Convert.ToBoolean(__GetInstructionSet(i));
                    Instructions[i].Name = Enum.GetName(typeof(InstructionSets), i);
                }
            }
            catch(Exception e)
            {
                if (Debug != null) Debug.SendMessage($"[GetInstructionSet()] {e.Message}...");
                return new Message()
                {
                    Class = "GetInstructionSet",
                    Exception = e,
                    Successful = false
                };
            }

            return new Message()
            {
                Class = "GetInstructionSet",
                Exception = null,
                Successful = true
            };
        }

        public enum InstructionSets
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
            FMA4
        }
    }
}
