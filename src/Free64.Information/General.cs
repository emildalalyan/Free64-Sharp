using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Free64.Information
{
    /// <summary>
    /// Exception thrown when data didn't found in <i>Registry</i> or it's corrupted
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DataRegistryException : Exception
    {
        /// <summary>
        /// Enumeration of exception types
        /// </summary>
        public enum ExceptionType : byte
        {
            /// <summary>
            /// Parameter wasn't found in Windows Registry
            /// </summary>
            ParameterNotFound,

            /// <summary>
            /// Invalid data found in Windows Registry
            /// </summary>
            InvalidData
        }

        private readonly string[] exceptionMessages =
        {
            "Parameter wasn't found in Windows Registry",
            "Invalid data found in Windows Registry"
        };

        /// <summary>
        /// Type of thrown exception
        /// </summary>
        public ExceptionType Type { get; init; }

        public override string Message => exceptionMessages[(int)Type];

        public DataRegistryException(ExceptionType exceptionType) : base()
        {
            Type = exceptionType;
        }
    }

    /// <summary>
    /// Interface, representing all information sources in <see cref="Free64"/>
    /// </summary>
    public interface IInformationSource
    {
        /// <summary>
        /// Initialize an information class (gather infomation from source).
        /// </summary>
        /// <returns><see cref="List{Exception}"/>, where T = <see cref="Exception"/></returns>
        List<Exception> Initialize();

        /// <summary>
        /// Resets all information in information class
        /// </summary>
        void Reset();
    }
}
