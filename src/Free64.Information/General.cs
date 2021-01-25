using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Free64.Information
{
    /// <summary>
    /// Struct, contains Exception, Class Name String and Successfulness Execution of Code
    /// </summary>
    public struct Message
    {
        public Exception Exception;
        public bool Successful;
        public string Class;
    }

    /// <summary>
    /// Interface for information sources
    /// </summary>
    public interface IInformationSource
    {
        /// <summary>
        /// Initialize a class.
        /// </summary>
        /// <returns></returns>
        List<Message> Initialize();

        /// <summary>
        /// Reset all information
        /// </summary>
        void Reset();
    }
}
