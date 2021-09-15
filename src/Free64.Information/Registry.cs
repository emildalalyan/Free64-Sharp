using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Free64.Information
{
    /// <summary>
    /// Class intended to gathering information from <b>Windows Registry</b>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class Registry : IMultiThreadedInformationSource
    {
        /// <summary>
        /// Abstract <see langword="class"/>, represents all <see cref="Registry"/> groups itself
        /// </summary>
        public abstract class RegistryGroup
        {
            /// <summary>
            /// Synchronously gather all information in the group
            /// </summary>
            /// <returns></returns>
#nullable enable
            public virtual Exception? Initialize()
#nullable disable
            {
                Trace.WriteLine($"\tCollecting information ({GroupName} group) from Windows Registry...");

                try
                {
                    InitializationBody();
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"\t[{GroupName} group throws an exception] {e.Message}");

                    return e;
                }

                return null;
            }

            /// <summary>
            /// Asynchronously gather all information in the group
            /// </summary>
            /// <returns></returns>
#nullable enable
            public virtual Task<Exception?> InitializeAsync()
#nullable disable
            {
                Trace.WriteLine($"\tCollecting information ({GroupName} group) from Windows Registry...");

                return Task.Run(() =>
                {
                    try
                    {
                        InitializationBody();
                    }
                    catch(Exception e)
                    {
                        return e;
                    }

                    return null;
                });
            }

            /// <summary>
            /// Represents initialization itself, which will be called by <see cref="Initialize"/> or <see cref="InitializeAsync"/>
            /// </summary>
            protected abstract void InitializationBody();

            /// <summary>
            /// Represents name of the <see cref="RegistryGroup"/>
            /// </summary>
            protected abstract string GroupName { get; }
        }

        /// <summary>
        /// Information, that gathered from <i>Registry</i> about operating system
        /// </summary>
        public class OperatingSystemGroup : RegistryGroup
        {
            /// <summary>
            /// Windows release ID (beginning from <b>Windows 10</b>)
            /// </summary>
            public string ReleaseId { get; private set; }

            /// <summary>
            /// Windows display version (beginning from <b>Windows 10 20H1</b>)
            /// </summary>
            public string DisplayVersion { get; private set; }

            /// <summary>
            /// Windows name (version) string
            /// </summary>
            public string ProductName { get; private set; }

            /// <summary>
            /// Get information about OS from <i>Windows Registry</i>
            /// </summary>
            /// <returns></returns>
            protected override void InitializationBody()
            {
                using RegistryKey Data = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
                if (Data == null) throw new DataException(DataException.ExceptionType.ParameterNotFound);

                ReleaseId = Data.GetValue("ReleaseId") as string ?? string.Empty;
                DisplayVersion = Data.GetValue("DisplayVersion") as string ?? string.Empty;
                ProductName = Data.GetValue("ProductName") as string ?? string.Empty;
            }

            protected override string GroupName => "Operating System";
        }

        /// <summary>
        /// Information, that gathered from <i>Registry</i> about processor
        /// </summary>
        public class ProcessorGroup : RegistryGroup
        {
            /// <summary>
            /// <b>Central Processing Unit</b> family number
            /// </summary>
            public ushort? Family { get; private set; } = null;

            /// <summary>
            /// <b>Central Processing Unit</b> model number
            /// </summary>
            public ushort? Model { get; private set; } = null;

            /// <summary>
            /// Represents <b>Central Processing Unit</b> stepping number
            /// </summary>
            public byte? Stepping { get; private set; } = null;

            /// <summary>
            /// <b>Central Processing Unit</b> revision string
            /// </summary>
            public string Revision { get; private set; }

            /// <summary>
            /// Get information about <b>Central Processing Unit</b> from <i>Windows Registry</i>
            /// </summary>
            /// <returns></returns>
            protected override void InitializationBody()
            {
                using RegistryKey Reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
                    
                if (Reg.GetValue("Identifier") is not string ident || !ident.Contains(" Family ") || !ident.Contains(" Model ") || !ident.Contains(" Stepping ")) throw new DataException(DataException.ExceptionType.InvalidData);

                string[] values = ident.Replace("Family ", "")
                                        .Replace("Model ", "")
                                        .Replace("Stepping ", "")
                                        .Split(' ');

                if(values.Length != 4) throw new DataException(DataException.ExceptionType.InvalidData);

                Family = ushort.Parse(values[1]);
                Model =  ushort.Parse(values[2]);
                Stepping = byte.Parse(values[3]);
            }

            protected override string GroupName => "Central Processing Unit";
        }

        /// <summary>
        /// Class, storing information about OS.
        /// </summary>
        public OperatingSystemGroup OperatingSystem { get; private set; }

        /// <summary>
        /// Class, storing information about <b>Central Processing Unit</b>.
        /// </summary>
        public ProcessorGroup Processor { get; private set; }

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            OperatingSystem = new();
            Processor = new();
        }

        /// <summary>
        /// Creates <b><see langword="new"/></b> instance of <see cref="Information.Registry"/>.
        /// <para>Parameter <b>initialize</b> is using for initializing <see cref="Information.Registry"/> after its creation.</para>
        /// </summary>
        /// <param name="initialize"></param>
        public Registry(bool initialize = false)
        {
            this.Reset();
            if (initialize) this.Initialize();
        }

        /// <summary>
        /// Gather <b>all</b> information from <i>Windows Registry</i>
        /// </summary>
        /// <returns></returns>
#nullable enable
        public Exception?[] Initialize()
#nullable disable
        {
            Trace.WriteLine("Initializing Free64.Information.Registry class...");

            return new Exception[]
            {
                Processor.Initialize(),
                OperatingSystem.Initialize()
            };
        }

        /// <summary>
        /// Asynchronous initialize <see cref="Registry"/> class (gather <b>all</b> information)
        /// </summary>
        /// <returns><see cref="Array"/> of <see cref="Task"/>s</returns>
#nullable enable
        public Task<Exception?>[] InitializeAsync()
#nullable disable
        {
            Trace.WriteLine("Initializing Free64.Information.Registry class (async mode)...");

            return new Task<Exception>[]
            {
                OperatingSystem.InitializeAsync(),
                Processor.InitializeAsync()
            };
        }
    }
}
