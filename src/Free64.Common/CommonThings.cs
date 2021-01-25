using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free64.Common
{
    /// <summary>
    /// This class contains common useful things.
    /// </summary>
    public static class CommonThings
    {
        /// <summary>
        /// Get UNIX Time (how many seconds passed from 1970-1-1 0:00).
        /// </summary>
        /// <returns></returns>
        public static double GetUNIXTime()
        {
            return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
