using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Free64.Information
{
    /// <summary>
    /// This exception throws when data was <b>not found</b> or it is <b>invalid</b>
    /// </summary>
    public class DataException : Exception
    {
        /// <summary>
        /// Enumeration of possible exception types
        /// </summary>
        public enum ExceptionType : byte
        {
            /// <summary>
            /// Data or parameter <b>was not found</b>.
            /// </summary>
            ParameterNotFound,

            /// <summary>
            /// <b>Invalid</b> data or parameter was found
            /// </summary>
            InvalidData
        }

        /// <summary>
        /// Messages corresponding to types of exception.
        /// </summary>
        protected readonly string[] ExceptionMessages =
        {
            "Parameter wasn't found",
            
            "Invalid data found"
        };

        /// <summary>
        /// Type of the thrown <see cref="Exception"/>
        /// </summary>
        public ExceptionType Type { get; init; }

        public override string Message => ExceptionMessages[(int)Type];

        /// <summary>
        /// Creates new instance of <see cref="DataException"/> with specified type of exception.
        /// </summary>
        /// <param name="exceptionType"></param>
        public DataException(ExceptionType exceptionType) : base()
        {
            this.Type = exceptionType;
        }
    }

    /// <summary>
    /// Class, which help to interop with low-level library (-ies)
    /// </summary>
    public static class LowLevelInteropHelper
    {
        /// <summary>
        /// Path to the <b>Free64.Information.LowLevel</b> library file
        /// </summary>
        public const string PathToLowLevelLibrary = "Free64.Information.LowLevel.dll";
    }

    /// <summary>
    /// Interface, representing all information sources in Free64.Information
    /// </summary>
    public interface IInformationSource
    {
        /// <summary>
        /// Initialize information class (gather all infomation from source).
        /// </summary>
        /// <returns><see cref="Exception"/>?[] — <see cref="Exception"/>s of all init. operations</returns>
#nullable enable
        public Exception?[] Initialize();
#nullable disable

        /// <summary>
        /// Reset all information in <see cref="IInformationSource"/>
        /// </summary>
        public void Reset();
    }

    /// <summary>
    /// Interface, representing all information sources in Free64.Information, which realize multi-threaded collection of information.
    /// </summary>
    public interface IMultiThreadedInformationSource : IInformationSource
    {
        /// <summary>
        /// Asynchronous initialize information class (gather all infomation from source).
        /// </summary>
        /// <returns><see cref="Array"/> of <see cref="Task"/>s</returns>
#nullable enable
        public Task<Exception?>[] InitializeAsync();
#nullable disable
    }
}
