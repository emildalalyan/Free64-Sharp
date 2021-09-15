using System;
using System.Linq;
using Free64.Information;
using System.IO;
using Free64.Common;
using System.Drawing;
using System.Globalization;

namespace Free64
{
    /// <summary>
    /// Class representing <b>Free64 CPUID</b> tool
    /// </summary>
    public class CpuidTool : Free64Tool
    {
        /// <summary>
        /// Represents instance of <see cref="CpuidTool"/> main form
        /// </summary>
        public override CPUID.CpuidToolForm MainForm { get; } = new();

        /// <summary>
        /// Creates a new instance of <see cref="CpuidTool"/> class.
        /// </summary>
        /// <param name="Initialize"></param>
        public CpuidTool()
        {
        }

        /// <summary>
        /// Gather all information and initialize <see cref="CpuidTool"/> class
        /// </summary>
        public void Initialize()
        {
            WMI wmi = new(false);
            wmi.Processor.Initialize();
            wmi.CacheMemory.Initialize();
            wmi.Motherboard.Initialize();
            wmi.Bios.Initialize();
            
            Registry reg = new(false);
            reg.Processor.Initialize();

            Initialize(wmi, reg, new(true));
        }

        /// <summary>
        /// Provide initialized classes to prevent unnecessary collection of information and initialize <see cref="CpuidTool"/>
        /// </summary>
        /// <param name="wmi"></param>
        /// <param name="registry"></param>
        /// <param name="cpuid"></param>
        public void Initialize(WMI wmi, Registry registry, Information.CPUID cpuid)
        {
            MainForm.CpuExts.Text = string.Join(", ", cpuid.Instructions.Where((i) => Information.CPUID.CommonExtensions.Contains(i.Name) && i.Support));

            if(cpuid.ProcessorCacheInfo == null || cpuid.ProcessorCacheInfo.Length < 1)
            {
                int Length = wmi.CacheMemory.CacheInformation.Length;

                MainForm.SizeL1I.Text = Length > 0 ? wmi.CacheMemory.CacheInformation[0].Size.SizeInBytes() : "N/A";
                MainForm.AssocL1I.Text = Length > 0 ? wmi.CacheMemory.CacheInformation[0].Associativity.IfNullReturnNA() : "N/A";

                MainForm.SizeL1D.Text = "N/A";
                MainForm.AssocL1D.Text = "N/A";
                MainForm.SizeL1D.Enabled = false;
                MainForm.AssocL1D.Enabled = false;

                MainForm.SizeL2.Text = Length > 1 ? wmi.CacheMemory.CacheInformation[1].Size.SizeInBytes() : "N/A";
                MainForm.AssocL2.Text = Length > 1 ? wmi.CacheMemory.CacheInformation[1].Associativity.IfNullReturnNA() : "N/A";

                MainForm.SizeL3.Text = Length > 2 ? wmi.CacheMemory.CacheInformation[2].Size.SizeInBytes() : "N/A";
                MainForm.AssocL3.Text = Length > 2 ? wmi.CacheMemory.CacheInformation[2].Associativity.IfNullReturnNA() : "N/A";

                MainForm.SizeL4.Text = Length > 3 ? wmi.CacheMemory.CacheInformation[3].Size.SizeInBytes() : "N/A";
                MainForm.AssocL4.Text = Length > 3 ? wmi.CacheMemory.CacheInformation[3].Associativity.IfNullReturnNA() : "N/A";
            }
            else
            {
                int Length = cpuid.ProcessorCacheInfo.Length;

                MainForm.SizeL1D.Text = Length > 0 ? $"{wmi.Processor.NumberOfLogicalProcessors / cpuid.ProcessorCacheInfo[0].ThreadsSharingCache} ⨯ {cpuid.ProcessorCacheInfo[0].Size.SizeInBytes()}" : "N/A";
                MainForm.AssocL1D.Text = Length > 0 ? cpuid.ProcessorCacheInfo[0].Associativity + "-way Set-Associative" : "N/A";

                MainForm.SizeL1I.Text = Length > 1 ? $"{wmi.Processor.NumberOfLogicalProcessors / cpuid.ProcessorCacheInfo[1].ThreadsSharingCache} ⨯ {cpuid.ProcessorCacheInfo[1].Size.SizeInBytes()}" : "N/A";
                MainForm.AssocL1I.Text = Length > 1 ? cpuid.ProcessorCacheInfo[1].Associativity + "-way Set-Associative" : "N/A";

                MainForm.SizeL2.Text = Length > 2 ? $"{wmi.Processor.NumberOfLogicalProcessors / cpuid.ProcessorCacheInfo[2].ThreadsSharingCache} ⨯ {cpuid.ProcessorCacheInfo[2].Size.SizeInBytes()}" : "N/A";
                MainForm.AssocL2.Text = Length > 2 ? cpuid.ProcessorCacheInfo[2].Associativity + "-way Set-Associative" : "N/A";

                MainForm.SizeL3.Text = Length > 3 ? $"{wmi.Processor.NumberOfLogicalProcessors / cpuid.ProcessorCacheInfo[3].ThreadsSharingCache} ⨯ {cpuid.ProcessorCacheInfo[3].Size.SizeInBytes()}" : "N/A";
                MainForm.AssocL3.Text = Length > 3 ? cpuid.ProcessorCacheInfo[3].Associativity + "-way Set-Associative" : "N/A";

                MainForm.SizeL4.Text = Length > 4 ? $"{wmi.Processor.NumberOfLogicalProcessors / cpuid.ProcessorCacheInfo[4].ThreadsSharingCache} ⨯ {cpuid.ProcessorCacheInfo[4].Size.SizeInBytes()}" : "N/A";
                MainForm.AssocL4.Text = Length > 4 ? cpuid.ProcessorCacheInfo[4].Associativity + "-way Set-Associative" : "N/A";
            }

            MainForm.NumberOfCpus.Text = wmi.Processor.NumberOfProcessors.ToString().IfNullReturnNA();
            MainForm.CpuFamily.Text = registry.Processor.Family == null ? "N/A" : registry.Processor.Family.Value.ToString("X") + "h";
            MainForm.CpuModel.Text = registry.Processor.Model == null ? "N/A" : registry.Processor.Model.Value.IfZeroReturnNA();

            string cpuspecification = (cpuid.ProcessorSpecification == null || cpuid.ProcessorSpecification.Length < 1 ? wmi.Processor.Specification : cpuid.ProcessorSpecification) ?? string.Empty;
            string cpuname = (cpuid.ProcessorName == null || cpuid.ProcessorName.Length < 1 ? wmi.Processor.Name : cpuid.ProcessorName) ?? string.Empty;

            MainForm.ProcessorName.Text = cpuname.IfNullReturnNA();
            MainForm.PlatformName.Text = wmi.Processor.SocketDesignation.IfNullReturnNA();
            MainForm.CpuStepping.Text = registry.Processor.Stepping == null ? wmi.Processor.Stepping.ToString().IfNullReturnNA() : registry.Processor.Stepping.Value.ToString();
            MainForm.CpuMaxClock.Text = wmi.Processor.MaxClockSpeed > 0 ? $"{wmi.Processor.MaxClockSpeed} MHz" : "N/A";
            MainForm.CpuManufacturer.Text = wmi.Processor.Manufacturer.IfNullReturnNA();
            MainForm.CpuSpec.Text = cpuspecification.IfNullReturnNA();
            MainForm.CpuCores.Text = wmi.Processor.NumberOfCores > 0 ? wmi.Processor.NumberOfCores.ToString() : "N/A";
            MainForm.CpuThreads.Text = wmi.Processor.NumberOfLogicalProcessors.IfZeroReturnNA();
            MainForm.CpuRev.Text = $"{registry.Processor.Model:00}{wmi.Processor.Stepping:00}";
            MainForm.BiosVer.Text = $"{(wmi.Bios.ReleaseDate is DateTime dt ? dt.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : "N/A")} | Version: {wmi.Bios.Version.IfNullReturnNA()}";

            MainForm.MotherboardName.Text = wmi.Motherboard.Product.IfNullReturnNA();
            MainForm.MbManufacturer.Text = wmi.Motherboard.Manufacturer.IfNullReturnNA();

            if (Directory.Exists($"{ToolsResourcesLocation}/cpuid/products/") && cpuname.Length > 0)
            {
                // We find matching file as long as possible 
                int max = 0;

                //We're seeking directory files
                foreach (string file in Directory.GetFiles($"{ToolsResourcesLocation}/cpuid/products/"))
                {
                    string nakedname = Path.GetFileNameWithoutExtension(file);

                    if (cpuname.Contains(nakedname) && max < nakedname.Length)
                    {
                        max = nakedname.Length;

                        MainForm.CpuImage.Image = Image.FromFile(file);
                    }
                }
            }

            if(MainForm.CpuImage.Image == null && File.Exists($"{ToolsResourcesLocation}/cpuid/manufacturers/{wmi.Processor.Manufacturer}.png"))
            {
                MainForm.CpuImage.Image = Image.FromFile($"{ToolsResourcesLocation}/cpuid/manufacturers/{wmi.Processor.Manufacturer}.png");
            }
        }

        /// <summary>
        /// Unload <see cref="CpuidTool"/> resources
        /// </summary>
        ~CpuidTool()
        {
            this.MainForm.Dispose();
        }

        public override void Show()
        {
            MainForm.Show();
            MainForm.BringToFront();
        }
    }
}
