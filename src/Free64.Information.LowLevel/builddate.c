#pragma once

#if defined __cplusplus
extern "C"
#endif

/// <summary>
/// Puts build date string into specified array
/// </summary>
DllExport errno_t GetBuildDate(char* date)
{
	return strcpy_s(date, strlen(__DATE__) + 1, __DATE__);
}