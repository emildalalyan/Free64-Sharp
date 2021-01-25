using System;
using System.Linq;
using Free64.Information;
using System.IO;
using Free64.Common;

namespace Free64.CPUID
{
    public class GUICPUID : IDisposable
    {
        public fmCPUID Form = new fmCPUID();
        public GUICPUID(bool Initialize = false)
        {
            if (Initialize) this.Initialize();
        }
        public GUICPUID(WMI WinMI, Registry AltSrc, Information.CPUID CPUID)
        {
            this.Initialize(WinMI, AltSrc, CPUID);
        }
        public void Initialize()
        {
            WMI WinMI = new WMI(false);
            WinMI.Win32_Processor();
            WinMI.Win32_CacheMemory();
            WinMI.Win32_BaseBoard();
            Initialize(WinMI, new Registry(true), new Information.CPUID(true));
        }
        public void Initialize( WMI WinMI, Registry AltSrc, Information.CPUID CPUID)
        {
            Form.label18.Text = "";
            foreach (Free64.Information.CPUID.InstructionSet ins in CPUID.Instructions)
            {
                if (!ins.Support || !Free64.Information.CPUID.SIMDExtensions.Contains(ins.Name)) continue;
                Form.label18.Text += (Form.label18.Text.Length < 1 ? "" : ", ") + ins.Name;
            }
            Form.label18.Text = Form.label18.Text.Replace("SSE4_1", "SSE4.1")
                                                 .Replace("SSE4_2", "SSE4.2")
                                                 .Replace("MMX_plus", "MMX+")
                                                 .Replace("AMD3DNowExt", "Extended 3DNow!")
                                                 .Replace("AMD3DNow", "3DNow!");

            byte Length = (byte)WinMI.CacheMemory.CacheMemory.Length;
            if(Length > 0) Form.l1cs.Text = WinMI.CacheMemory.CacheMemory[0].SizeInBytes();
            if(Length > 1) Form.l2cs.Text = WinMI.CacheMemory.CacheMemory[1].SizeInBytes();
            if(Length > 2) Form.l3cs.Text = WinMI.CacheMemory.CacheMemory[2].SizeInBytes();
            if(Length > 3) Form.l4cs.Text = WinMI.CacheMemory.CacheMemory[3].SizeInBytes();

            Form.label46.Text = WinMI.Processor.NumberOfProcessors.ToString();
            Form.label20.Text = AltSrc.Information.Family.IfNullReturnNA();
            Form.label25.Text = AltSrc.Information.Model.IfNullReturnNA();

            Form.label7.Text = WinMI.Processor.Name.IfNullReturnNA();
            Form.label8.Text = WinMI.Processor.SocketDesignation.IfNullReturnNA();
            Form.label9.Text = WinMI.Processor.Stepping.IfNullReturnNA();
            Form.label10.Text = WinMI.Processor.MaxClockSpeed > 0 ? $"{WinMI.Processor.MaxClockSpeed} MHz" : "N/A";
            Form.label11.Text = WinMI.Processor.Manufacturer.IfNullReturnNA();
            Form.label12.Text = WinMI.Processor.Specification.IfNullReturnNA();
            Form.label15.Text = WinMI.Processor.NumberOfCores > 0 ? WinMI.Processor.NumberOfCores.ToString() : "N/A";
            Form.label13.Text = WinMI.Processor.NumberOfLogicalProcessors > 0 ? WinMI.Processor.NumberOfLogicalProcessors.ToString() : "N/A";

            Form.MotherBoard.Text = WinMI.Motherboard.Product.IfNullReturnNA();
            Form.MotherBoardManufacturer.Text = WinMI.Motherboard.Manufacturer.IfNullReturnNA();

            if (!Directory.Exists("Data/CPU"))
            {
                if (File.Exists($"Data/{WinMI.Processor.Manufacturer}.png"))
                {
                    Form.pictureBox1.Image = System.Drawing.Image.FromFile($"Data/{WinMI.Processor.Manufacturer}.png");
                }
                Directory.CreateDirectory("Data/CPU");
                return;
            }

            // Variables Definition
            byte TempLength = 0;

            //We're seeking directory files
            foreach (string FileName in Directory.GetFiles("Data/CPU"))
            {
                string NameWithoutExt = Path.GetFileNameWithoutExtension(FileName);
                if (WinMI.Processor.Name.Contains(NameWithoutExt))
                {
                    if (TempLength > NameWithoutExt.Length) continue;
                    TempLength = (byte)NameWithoutExt.Length;
                    Form.pictureBox1.Image = System.Drawing.Image.FromFile(FileName);
                }
            }
        }
        public void Show()
        {
            this.Form.Show();
            this.Form.BringToFront();
        }
        public void Dispose()
        {
            this.Form.Close();
            this.Form.Dispose();
        }
    }
}
