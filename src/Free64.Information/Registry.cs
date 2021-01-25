using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Free64.Information
{
    public class Registry : IInformationSource
    {
        /// <summary>
        /// Debug Instance
        /// </summary>
        private readonly Debug.Debug Debug;

        public struct RegistryInfo
        {
            public ushort Family { get; internal set; }
            public ushort Model { get; internal set; }
            public string ReleaseId { get; internal set; }
        }

        private RegistryInfo __Information;

        /// <summary>
        /// Struct, storing information.
        /// </summary>
        public RegistryInfo Information { get { return __Information; } }

        /// <summary>
        /// Clear all information
        /// </summary>
        public void Reset()
        {
            __Information = new RegistryInfo();
        }

        /// <summary>
        /// Constructor of <see cref="Information.Registry"/>
        /// </summary>
        /// <param name="Initialize"></param>
        public Registry(bool Initialize = false)
        {
            Reset();
            if (Initialize) this.Initialize();
        }

        /// <summary>
        /// Constructor of <see cref="Information.Registry"/>
        /// </summary>
        /// <param name="AutoInitialize"></param>
        /// <param name="DebugInstance"></param>
        public Registry(bool AutoInitialize, Debug.Debug DebugInstance)
        {
            Reset();
            Debug = DebugInstance;
            if (AutoInitialize) this.Initialize();
        }

        public List<Message> Initialize()
        {
            return new List<Message>()
            {
                ProcessorInfo(),
                OperatingSystemInfo()
            };
        }

        public Message OperatingSystemInfo()
        {
            if (Debug != null) Debug.SendMessage("Collecting Information about Operating System from Windows Registry...");
            try
            {
                using (RegistryKey Reg = Microsoft.Win32.Registry.LocalMachine)
                {
                    object Data = Reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false).GetValue("ReleaseId");
                    if (Data != null) __Information.ReleaseId = Data.ToString();
                    else throw new Exception("ReleaseId wasn't found in Windows Registry...");
                }
            }
            catch (Exception e)
            {
                __Information.ReleaseId = "";
                if (Debug != null) Debug.SendMessage($"[OperatingSystemInfo()] {e.Message}");
                return new Message
                {
                    Exception = e,
                    Class = "OperatingSystemInfo",
                    Successful = false
                };
            }

            return new Message
            {
                Exception = null,
                Class = "OperatingSystemInfo",
                Successful = true
            };
        }

        public Message ProcessorInfo()
        {
            if (Debug != null) Debug.SendMessage("Collecting Information about Processor from Windows Registry...");
            try
            {
                using(RegistryKey Reg = Microsoft.Win32.Registry.LocalMachine)
                {
                    string ID = Reg.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false).GetValue("Identifier").ToString();
                    if (!ID.Contains(" Family ") || !ID.Contains(" Model ") || !ID.Contains(" Stepping ")) throw new Exception("Invalid data found in Windows Registry.");
                    string[] IdSplit = ID.Split(new string[] { " Family " }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { " Model " }, StringSplitOptions.RemoveEmptyEntries);
                    IdSplit[1] = IdSplit[1].Split(new string[] { " Stepping " }, StringSplitOptions.RemoveEmptyEntries)[0];
                    __Information.Family = Convert.ToUInt16(IdSplit[0]);
                    __Information.Model = Convert.ToUInt16(IdSplit[1]);
                }
            }
            catch(Exception e)
            {
                if (Debug != null) Debug.SendMessage("[ProcessorInfo()] " + e.Message);
                return new Message
                {
                    Exception = e,
                    Class = "ProcessorInfo",
                    Successful = false
                };
            }

            return new Message
            {
                Exception = null,
                Class = "ProcessorInfo",
                Successful = true
            };
        }
    }
}
