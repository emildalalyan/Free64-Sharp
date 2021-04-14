using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace Free64.Information
{
    /// <summary>
    /// Class intended for gathering information from <i>Windows Registry</i>
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class Registry : IInformationSource
    {
        /// <summary>
        /// Information, that gathered from Registry about OS
        /// </summary>
        public readonly struct OperatingSystemInformation
        {
            /// <summary>
            /// Windows release ID (beginning from <b>Windows 10</b>)
            /// </summary>
            public string ReleaseId { get; init; }

            /// <summary>
            /// Windows display version (beginning from <b>Windows 10 20H1</b>)
            /// </summary>
            public string DisplayVersion { get; init; }

            /// <summary>
            /// Windows name (version) string
            /// </summary>
            public string ProductName { get; init; }
        }

        /// <summary>
        /// Information, that gathered from Registry about CPU
        /// </summary>
        public readonly struct ProcessorInformation
        {
            /// <summary>
            /// Processor family
            /// </summary>
            public ushort Family { get; init; }

            /// <summary>
            /// Processor model
            /// </summary>
            public ushort Model { get; init; }
        }

        /// <summary>
        /// Structure, storing information about OS.
        /// </summary>
        public OperatingSystemInformation OperatingSystem { get; private set; }

        /// <summary>
        /// Structure, storing information about CPU.
        /// </summary>
        public ProcessorInformation Processor { get; private set; }

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            OperatingSystem = new OperatingSystemInformation();
            Processor = new ProcessorInformation();
        }

        /// <summary>
        /// Constructor of <b><see cref="Information.Registry"/></b>.
        /// <para>Parameter <i>initialize</i> is using for initializing <see cref="Information.Registry"/> after its creation.</para>
        /// </summary>
        /// <param name="initialize"></param>
        public Registry(bool initialize = false)
        {
            Reset();
            if (initialize) this.Initialize();
        }

        /// <summary>
        /// Gather <b>all</b> information from <i>Windows Registry</i>
        /// </summary>
        /// <returns></returns>
        public List<Exception> Initialize()
        {
            Trace.WriteLine("Initializing Free64.Information.Registry class...");

            return new List<Exception>()
            {
                ProcessorInfo(),
                OperatingSystemInfo()
            };
        }

        /// <summary>
        /// Get only information about OS from <i>Registry</i>
        /// </summary>
        /// <returns></returns>
        public Exception OperatingSystemInfo()
        {
            Trace.WriteLine("module> Collecting Information about Operating System from Windows Registry...");
            try
            {
                using RegistryKey Data = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
                if (Data is not null)
                {
                    OperatingSystem = new OperatingSystemInformation()
                    {
                        ReleaseId = Data.GetValue("ReleaseId") as string ?? string.Empty,
                        DisplayVersion = Data.GetValue("DisplayVersion") as string ?? string.Empty,
                        ProductName = Data.GetValue("ProductName") as string ?? string.Empty
                    };
                }
                else throw new DataRegistryException(DataRegistryException.ExceptionType.ParameterNotFound);
            }
            catch (Exception e)
            {
                Trace.WriteLine($"module> [OperatingSystemInfo()] {e.Message}");
                return e;
            }

            return null;
        }

        /// <summary>
        /// Get only information about CPU from Registry
        /// </summary>
        /// <returns></returns>
        public Exception ProcessorInfo()
        {
            Trace.WriteLine("module> Collecting Information about Processor from Windows Registry...");
            try
            {
                using RegistryKey Reg = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
                
                string ID = Reg.GetValue("Identifier").ToString();
                if (!ID.Contains(" Family ") || !ID.Contains(" Model ") || !ID.Contains(" Stepping ")) throw new DataRegistryException(DataRegistryException.ExceptionType.InvalidData);
                string[] IdSplit = ID.Split(new string[] { " Family " }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { " Model " }, StringSplitOptions.RemoveEmptyEntries);
                IdSplit[1] = IdSplit[1].Split(new string[] { " Stepping " }, StringSplitOptions.RemoveEmptyEntries)[0];
                Processor = new()
                {
                    Family = ushort.Parse(IdSplit[0]),
                    Model = ushort.Parse(IdSplit[1])
                };
            }
            catch(Exception e)
            {
                Trace.WriteLine("module> [ProcessorInfo()] " + e.Message);
                return e;
            }

            return null;
        }
    }
}
