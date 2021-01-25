// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

#define EAX 0
#define EBX 1
#define ECX 2
#define EDX 3
#define DllExport __declspec(dllexport)

extern "C"
{
	DllExport unsigned char GetInstructionSet(int frag);
}
unsigned char GetInstructionSet(int frag)
{
	int ci[4] = {};

	__cpuid(ci, 1); //0x00000001
	switch (frag) {
		case 0: { return (ci[EDX] >> 23 & 1); } // MMX
		case 1: { return (ci[EDX] >> 25 & 1); } // SSE
		case 2: { return (ci[EDX] >> 26 & 1) && (ci[EDX] >> 19 & 1); } // SSE2
		case 3: { return (ci[ECX] & 1) && (ci[ECX] >> 3 & 1); } // SSE3
		case 4: { return (ci[ECX] >> 9 & 1); } // SSSE3
		case 5: { return (ci[ECX] >> 19 & 1); } // SSE4.1
		case 6: { return ((ci[ECX] >> 23 & 1) & (ci[ECX] >> 20 & 1)); } // SSE4.2
		case 7: { return (ci[ECX] >> 28 & 1); } // AVX
		case 8: { return (ci[ECX] >> 5 & 1); } // VMX
		case 9: { return (ci[ECX] >> 6 & 1); } // SMX
		case 10: { return (ci[ECX] >> 7 & 1); } // EIST
		case 11: { return (ci[ECX] >> 8 & 1) && (ci[EDX] >> 29 & 1); } // Thermal Monitor 2
		case 12: { return (ci[ECX] >> 12) & 1; } // FMA3
		case 13: { return (ci[ECX] >> 25 & 1); } // AES
		case 14: { return (ci[EDX] & 1); } // CPU has FPU
		case 15: { return (ci[EDX] >> 1 & 1); } // VME
		case 16: { return (ci[EDX] >> 3 & 1); } // PSE
		case 17: { return (ci[EDX] >> 4 & 1); } // TSC
		case 18: { return (ci[EDX] >> 5 & 1); } // MSR
		case 19: { return (ci[EDX] >> 6 & 1); } // Physical Address Extension
		case 20: { return (ci[EDX] >> 7 & 1); } // MCE
		case 21: { return (ci[EDX] >> 9 & 1); } // APIC
		case 22: { return (ci[EDX] >> 12 & 1); } // MTRR
		case 23: { return (ci[EDX] >> 13 & 1); } // PGE
		case 24: { return (ci[EDX] >> 14 & 1); } // MCA
		case 25: { return (ci[EDX] >> 15 & 1); } // CMOV
		case 26: { return (ci[EDX] >> 17 & 1); } // PSE-36
		case 27: { return (ci[EDX] >> 27 & 1); } // Self-Snoop
		case 28: { return (ci[EDX] >> 31 & 1); } // PBE
	}

	__cpuid(ci, 7); //0x00000007
	switch (frag) {
		case 29: { return (ci[EBX] >> 5 & 1); } // AVX2
		case 30: { return (ci[EBX] >> 29 & 1); } // SHA
	}

	__cpuid(ci, 0x80000001); //0x80000001
	switch (frag) {
		case 31: { return (ci[EDX] >> 22 & 1); } // MMXplus
		case 32: { return (ci[EDX] >> 31 & 1); } // 3DNow!
		case 33: { return (ci[EDX] >> 30 & 1); } // Extended 3dNow!
		case 34: { return (ci[ECX] >> 6 & 1); } // SSE4A
		case 35: { return (ci[ECX] >> 16 & 1); } // FMA4
	}

	return false;
}
