#include "pch.h"

enum Registers
{
	EAX,
	EBX,
	ECX,
	EDX
};

extern "C"
{
	DllExport bool GetInstructionSet(bool* arr);
	DllExport void GetProcessorNameString(unsigned char* value);
}

/// <summary>
/// Get processor name from <see cref="__cpuid"/> to be written in <see cref="unsigned char*"/> (array)
/// </summary>
/// <param name="name"></param>
void GetProcessorNameString(unsigned char* name)
{
	for (int i = 0, a = 0; i <= 2; ++i, ++a)
	{
		int info[4] = {};
		__cpuid(info, 0x80000002 + i);
		name[a] = ((info[EAX]) & UCHAR_MAX);
		name[++a] = ((info[EAX] >> 8) & UCHAR_MAX);
		name[++a] = ((info[EAX] >> 16) & UCHAR_MAX);
		name[++a] = ((info[EAX] >> 24) & UCHAR_MAX);
		name[++a] = ((info[EBX]) & UCHAR_MAX);
		name[++a] = ((info[EBX] >> 8) & UCHAR_MAX);
		name[++a] = ((info[EBX] >> 16) & UCHAR_MAX);
		name[++a] = ((info[EBX] >> 24) & UCHAR_MAX);
		name[++a] = ((info[ECX]) & UCHAR_MAX);
		name[++a] = ((info[ECX] >> 8) & UCHAR_MAX);
		name[++a] = ((info[ECX] >> 16) & UCHAR_MAX);
		name[++a] = ((info[ECX] >> 24) & UCHAR_MAX);
		name[++a] = ((info[EDX]) & UCHAR_MAX);
		name[++a] = ((info[EDX] >> 8) & UCHAR_MAX);
		name[++a] = ((info[EDX] >> 16) & UCHAR_MAX);
		name[++a] = ((info[EDX] >> 24) & UCHAR_MAX);
	}
}

/// <summary>
/// Get instruction set of processor from <see cref="__cpuid"/> to be written in <see cref="bool*"/> (array)
/// </summary>
/// <param name="name"></param>
bool GetInstructionSet(bool* arr)
{
	int ci[4] = {};

	__cpuid(ci, 1); //0x00000001
	
	arr[0] = (ci[EDX] >> 23 & 1); // MMX
	arr[1] = (ci[EDX] >> 25 & 1); // SSE
	arr[2] = (ci[EDX] >> 26 & 1) && (ci[EDX] >> 19 & 1); // SSE2
	arr[3] = (ci[ECX] & 1) && (ci[ECX] >> 3 & 1); // SSE3
	arr[4] = (ci[ECX] >> 9 & 1); // SSSE3
	arr[5] = (ci[ECX] >> 19 & 1); // SSE4.1
	arr[6] = ((ci[ECX] >> 23 & 1) & (ci[ECX] >> 20 & 1)); // SSE4.2
	arr[7] = (ci[ECX] >> 28 & 1); // AVX
	arr[8] = (ci[ECX] >> 5 & 1); // VMX
	arr[9] = (ci[ECX] >> 6 & 1); // SMX
	arr[10] = (ci[ECX] >> 7 & 1); // EIST
	arr[11] = (ci[ECX] >> 8 & 1) && (ci[EDX] >> 29 & 1); // Thermal Monitor 2
	arr[12] = (ci[ECX] >> 12) & 1; // FMA3
	arr[13] = (ci[ECX] >> 25 & 1); // AES
	arr[14] = (ci[EDX] & 1); // CPU has FPU
	arr[15] = (ci[EDX] >> 1 & 1); // VME
	arr[16] = (ci[EDX] >> 3 & 1); // PSE
	arr[17] = (ci[EDX] >> 4 & 1); // TSC
	arr[18] = (ci[EDX] >> 5 & 1); // MSR
	arr[19] = (ci[EDX] >> 6 & 1); // Physical Address Extension
	arr[20] = (ci[EDX] >> 7 & 1); // MCE
	arr[21] = (ci[EDX] >> 9 & 1); // APIC
	arr[22] = (ci[EDX] >> 12 & 1); // MTRR
	arr[23] = (ci[EDX] >> 13 & 1); // PGE
	arr[24] = (ci[EDX] >> 14 & 1); // MCA
	arr[25] = (ci[EDX] >> 15 & 1); // CMOV
	arr[26] = (ci[EDX] >> 17 & 1); // PSE-36
	arr[27] = (ci[EDX] >> 27 & 1); // Self-Snoop
	arr[28] = (ci[EDX] >> 31 & 1); // PBE

	__cpuid(ci, 7); //0x00000007

	arr[29] = (ci[EBX] >> 5 & 1); // AVX2
	arr[30] = (ci[EBX] >> 29 & 1); // SHA

	__cpuid(ci, 0x80000001); //0x80000001

	arr[31] = (ci[EDX] >> 22 & 1); // MMXplus
	arr[32] = (ci[EDX] >> 31 & 1); // 3DNow!
	arr[33] = (ci[EDX] >> 30 & 1); // Extended 3dNow!
	arr[34] = (ci[ECX] >> 6 & 1); // SSE4A
	arr[35] = (ci[ECX] >> 16 & 1); // FMA4
	arr[36] = (ci[ECX] >> 2 & 1); // AMD-V

	return true;
}
