using System;

namespace Free64.Common
{
    /// <summary>
    /// This class contains common useful things.
    /// </summary>
    public static class CommonThings
    {
        /// <summary>
        /// Get UNIX Time (how many seconds have passed from 1970-1-1 0:00).
        /// </summary>
        /// <returns></returns>
        public static double GetUNIXTime() => DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;

        /// <summary>
        /// Enumeration representing data units of measure
        /// </summary>
        public enum DataUnitsOfMeasure
        {
            B, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        /// <summary>
        /// Converts <see cref="uint"/> to readable size of bytes
        /// </summary>
        /// <param name="Bytes"></param>
        public static string SizeInBytes(this uint Bytes)
        {
            if(Bytes < 1024) return $"{Bytes} B";

            ushort power = (ushort)Math.Floor(Math.Log(Bytes, 1024));
            return $"{Bytes / Math.Pow(1024, power)} {Enum.GetName(typeof(DataUnitsOfMeasure), power)}";
        }

        /// <summary>
        /// Converts <see cref="ulong"/> to readable size of bytes
        /// </summary>
        /// <param name="Bytes"></param>
        public static string SizeInBytes(this ulong Bytes)
        {
            if (Bytes < 1024) return $"{Bytes} B";

            ushort power = (ushort)Math.Floor(Math.Log(Bytes, 1024));
            return $"{Bytes / Math.Pow(1024, power)} {Enum.GetName(typeof(DataUnitsOfMeasure), power)}";
        }
    }
}
