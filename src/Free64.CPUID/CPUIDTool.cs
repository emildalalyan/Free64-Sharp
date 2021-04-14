using System;
using System.Linq;
using Free64.Information;
using System.IO;
using Free64.Common;

namespace Free64.CPUID
{
    /// <summary>
    /// Class representing <b>Free64 CPUID</b> tool
    /// </summary>
    public class CPUIDTool
    {
        /// <summary>
        /// <see cref="CPUIDTool"/> main form
        /// </summary>
        public fmCPUID Form = new();

        /// <summary>
        /// <see cref="CPUIDTool"/> constructor. If <see cref="bool"/> Initialize = <see langword="true"/>, then it will create and initialize necessary classes itself
        /// </summary>
        /// <param name="Initialize"></param>
        public CPUIDTool(bool Initialize = false)
        {
            if (Initialize) this.Initialize();
        }

        /// <summary>
        /// <para><see cref="CPUIDTool"/> constructor. Provide initialized classes to prevent unnecessary initialization.</para>
        /// <para>It calls <see cref="Initialize(WMI, Registry, Information.CPUID)"/> itself</para>
        /// </summary>
        /// <param name="wmi"></param>
        /// <param name="registry"></param>
        /// <param name="cpuid"></param>
        public CPUIDTool(WMI wmi, Registry registry, Information.CPUID cpuid)
        {
            this.Initialize(wmi, registry, cpuid);
        }

        /// <summary>
        /// Initialize <see cref="CPUIDTool"/> class
        /// </summary>
        public void Initialize()
        {
            WMI wmi = new(false);
            wmi.Processor.Initialize();
            wmi.CacheMemory.Initialize();
            wmi.Motherboard.Initialize();
            Initialize(wmi, new Registry(true), new Information.CPUID(true));
        }

        /// <summary>
        /// Provide initialized classes to prevent unnecessary initialization and initialize <see cref="CPUIDTool"/>
        /// </summary>
        /// <param name="wmi"></param>
        /// <param name="registry"></param>
        /// <param name="cpuid"></param>
        public void Initialize(WMI wmi, Registry registry, Information.CPUID cpuid)
        {
            Form.label18.Text = "";
            foreach (Information.CPUID.InstructionSet set in cpuid.Instructions)
            {
                if (!set.Support || !Information.CPUID.SIMDExtensions.Contains(set.Name)) continue;
                Form.label18.Text += (Form.label18.Text.Length < 1 ? "" : ", ") + set.Name;
            }
            Form.label18.Text = Form.label18.Text.Replace("SSE4_1", "SSE4.1")
                                                 .Replace("SSE4_2", "SSE4.2")
                                                 .Replace("MMX_plus", "MMX+")
                                                 .Replace("AMD3DNowExt", "Extended 3DNow!")
                                                 .Replace("AMD3DNow", "3DNow!")
                                                 .Replace("AMD_V", "AMD-V")
                                                 .Replace("VMX", "VT-x");

            if(wmi.CacheMemory.CacheMemory != null)
            {
                byte Length = (byte)wmi.CacheMemory.CacheMemory.Length;
                if (Length > 0) Form.l1cs.Text = wmi.CacheMemory.CacheMemory[0].SizeInBytes();
                if (Length > 1) Form.l2cs.Text = wmi.CacheMemory.CacheMemory[1].SizeInBytes();
                if (Length > 2) Form.l3cs.Text = wmi.CacheMemory.CacheMemory[2].SizeInBytes();
                if (Length > 3) Form.l4cs.Text = wmi.CacheMemory.CacheMemory[3].SizeInBytes();
            }

            Form.label46.Text = wmi.Processor.NumberOfProcessors.ToString().IfNullReturnNA();
            Form.label20.Text = registry.Processor.Family.IfNullReturnNA();
            Form.label25.Text = registry.Processor.Model.IfNullReturnNA();

            Form.label7.Text = cpuid.ProcessorName ?? wmi.Processor.Name.IfNullReturnNA();
            Form.label8.Text = wmi.Processor.SocketDesignation.IfNullReturnNA();
            Form.label9.Text = wmi.Processor.Stepping.ToString().IfNullReturnNA();
            Form.label10.Text = wmi.Processor.MaxClockSpeed > 0 ? $"{wmi.Processor.MaxClockSpeed} MHz" : "N/A";
            Form.label11.Text = wmi.Processor.Manufacturer.IfNullReturnNA();
            Form.label12.Text = cpuid.ProcessorSpecification ?? wmi.Processor.Specification.IfNullReturnNA();
            Form.label15.Text = wmi.Processor.NumberOfCores > 0 ? wmi.Processor.NumberOfCores.ToString() : "N/A";
            Form.label13.Text = wmi.Processor.NumberOfLogicalProcessors > 0 ? wmi.Processor.NumberOfLogicalProcessors.ToString() : "N/A";
            Form.label23.Text = $"{registry.Processor.Model:00}{wmi.Processor.Stepping:00}";

            Form.MotherBoard.Text = wmi.Motherboard.Product.IfNullReturnNA();
            Form.MotherBoardManufacturer.Text = wmi.Motherboard.Manufacturer.IfNullReturnNA();

            if (Directory.Exists("Data/CPU"))
            {
                // Variables Definition
                int TempLength = 0;

                //We're seeking directory files
                foreach (string FileName in Directory.GetFiles("Data/CPU"))
                {
                    string NameWithoutExt = Path.GetFileNameWithoutExtension(FileName);
                    if (string.IsNullOrEmpty(cpuid.ProcessorName) && string.IsNullOrEmpty(wmi.Processor.Name)) break;
                    if ((cpuid.ProcessorName ?? wmi.Processor.Name).Contains(NameWithoutExt))
                    {
                        if (TempLength > NameWithoutExt.Length) continue;
                        TempLength = NameWithoutExt.Length;
                        Form.pictureBox1.Image = System.Drawing.Image.FromFile(FileName);
                        return;
                    }
                }
            }
            else Directory.CreateDirectory("Data/CPU");

            if (File.Exists($"Data/{wmi.Processor.Manufacturer}.png"))
            {
                Form.pictureBox1.Image = System.Drawing.Image.FromFile($"Data/{wmi.Processor.Manufacturer}.png");
            }
        }

        /// <summary>
        /// Show <see cref="CPUIDTool"/> main form
        /// </summary>
        public void Show()
        {
            this.Form.Show();
            this.Form.BringToFront();
        }

        /// <summary>
        /// Unload <see cref="CPUIDTool"/> resources
        /// </summary>
        ~CPUIDTool()
        {
            this.Form.Dispose();
        }
    }
}
