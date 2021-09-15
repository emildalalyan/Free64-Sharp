using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace Free64.Information
{
    partial class WMI
    {
        /// <summary>
        /// Abstract <see langword="class"/>, representing WMI classes
        /// </summary>
        public abstract class WMIClass
        {
            /// <summary>
            /// Gather information from class
            /// </summary>
            /// <param name="scope"></param>
            /// <returns><see cref="Exception"/>? instance</returns>
#nullable enable
            public virtual Exception? Initialize()
#nullable disable
            {
                Trace.WriteLine($"\tCollecting information from {WmiClassName} class...");

                try
                {
                    InitializationBody();
                }
                catch(Exception e)
                {
                    Trace.WriteLine($"\t[{WmiClassName}] {e.Message}...");

                    return e;
                }
                finally
                {
                    IsInitialized = true;
                }

                return null;
            }

            /// <summary>
            /// Asynchronous gather information from class
            /// </summary>
            /// <param name="scope"></param>
            /// <returns><see cref="Task"/>, which has been started</returns>
#nullable enable
            public virtual Task<Exception?> InitializeAsync()
#nullable disable
            {
                Trace.WriteLine($"\tCollecting information from {WmiClassName} class...");

                return Task.Run(() =>
                {
                    try
                    {
                        InitializationBody();
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                    finally
                    {
                        IsInitialized = true;
                    }

                    return null;
                });
            }

            /// <summary>
            /// Represents initialization itself, which will be called by <see cref="Initialize"/> or <see cref="InitializeAsync"/>
            /// </summary>
            protected abstract void InitializationBody();

            /// <summary>
            /// This property is indicating that class has been initialized
            /// </summary>
            public bool IsInitialized { get; protected set; } = false;

            /// <summary>
            /// Represents name of the <b>WMI</b> class.
            /// </summary>
            public abstract string WmiClassName { get; }

            /// <summary>
            /// Query, which will be sent to Management
            /// </summary>
            public abstract string Query { get; }
        }
    }
}
