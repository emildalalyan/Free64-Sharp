using System;
using System.Windows.Forms;
using Free64.Information;
using System.Globalization;
using System.Threading;
using Free64.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Free64
{
    internal static class Program
    {
        /// <summary>
        /// The <b>entry point</b> of the <see cref="Free64"/> application.
        /// </summary>
        [STAThread] // He came into your apartment and make it single-threaded
        
        internal static void Main()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles(); // We're enabling visual styles.
            Application.SetCompatibleTextRenderingDefault(false); // We're switching off compatible text rendering.
            Application.Run(new MainForm()); // Don't make a fight :D. The application is started.
        }
    }

    /// <summary>
    /// Describes all program <see langword="const"/>ants.
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// Version of <see cref="Free64"/> build. This is a <see langword="readonly string"/>.
        /// </summary>
        public static readonly string Version = $"{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}-{Platform.GetPlatformString()}";

        /// <summary>
        /// Link to the <see cref="Free64"/> repository page. This is a constant.
        /// </summary>
        public const string LinkToRepo = "https://github.com/emildalalyan/Free64-Sharp";

        /// <summary>
        /// Date, when build of <b>Free64.Information</b> was compiled. It is <see langword="default"/> if attempt of build date gathering was failed.
        /// </summary>
        public static readonly DateTime BuildDate = BuildInformation.BuildDate;

        // We're defining the constants
    }

    /// <summary>
    /// Class, contains all Free64 <see cref="Form"/>s
    /// </summary>
    public static class Forms
    {
        /// <summary>
        /// Instance of <see cref="Free64.MainForm"/> (main <see cref="Form"/> of Free64)
        /// </summary>
        public static MainForm MainForm { get; internal set; } // Instance of fmMain, it will be filled by fmMain constructor called by Program.Main()
    }

    /// <summary>
    /// Class, contains all <see cref="Free64Tool"/>s
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Instance of <see cref="CpuidTool"/> class
        /// </summary>
        public static CpuidTool Cpuid { get; } = new();
    }

    public static partial class Free64Application
    {
        /// <summary>
        /// Instance of the <see cref="GraphicalTraceListener"/> class.
        /// </summary>
        public static GraphicalTraceListener GraphicalTrace { get; } = new();

        /// <summary>
        /// Instance of the <see cref="WMI"/> class
        /// </summary>
        public static WMI WmiInfo { get; } = new();

        /// <summary>
        /// Instance of the <see cref="Registry"/> class
        /// </summary>
        public static Registry RegInfo { get; } = new();

        /// <summary>
        /// Instance of the <see cref="Information.CPUID"/> class
        /// </summary>
        public static Information.CPUID CpuidInfo { get; } = new();

        /// <summary>
        /// <see cref="Free64Application"/> static constructor
        /// </summary>
        static Free64Application()
        {
            _ = Trace.Listeners.Add(GraphicalTrace);
        }

        /// <summary>
        /// Indicates, whether application is initialized or not
        /// </summary>
        public static bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Initialize <see cref="Free64"/> and gather all information from all <see cref="IInformationSource"/>s
        /// </summary>
        public static void Initialize()
        {
            if (IsInitialized)
            {
                GraphicalTrace.Clear(); // We're cleaning the debug list

                // We're deleting previous gathered information with "Reset()" function

                WmiInfo.Reset();

                CpuidInfo.Reset();

                RegInfo.Reset();
            }

            Trace.Write("Starting an initialization...");

            Trace.WriteLine(" Showing splash screen...");

            using SplashForm splash = new();

            splash.Show();

            splash.Refresh();

            Stopwatch time = Stopwatch.StartNew();

            Trace.WriteLine("Starting a stopwatch...");

            Trace.WriteLine($"Free64 Extreme Version: {Constants.Version} [build from {Constants.BuildDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}]"); // Showing Free64 Version

            Trace.WriteLine($"Assembly architecture: {Enum.GetName(typeof(ProcessorArchitecture), Assembly.GetExecutingAssembly().GetName().ProcessorArchitecture)}"); // Showing command line arguments

            var wmiinit = WmiInfo.InitializeAsync();
            Task.WaitAll(wmiinit);
            
            foreach (var task in wmiinit)
            {
                if (task.Result != null) Trace.WriteLine($"\t{task.Result.Message} {task.Result.StackTrace.Replace("   ", "")}");
            }

            var registryinit = RegInfo.InitializeAsync();
            Task.WaitAll(registryinit);

            foreach (var task in registryinit)
            {
                if (task.Result != null) Trace.WriteLine($"\t{task.Result.Message} {task.Result.StackTrace.Replace("   ", "")}");
            }

            _ = CpuidInfo.Initialize();

            TimeSpan inittime = time.Elapsed; // Time spent on collecting information.
            
            Trace.WriteLine("Working with the interface...");

            Forms.MainForm.listView1.Items["osname"].SubItems[1].Text = RegInfo.OperatingSystem.ProductName?.Length > 0 ? RegInfo.OperatingSystem.ProductName : WmiInfo.OperatingSystem.Caption.IfNullReturnNA();

            Forms.MainForm.listView1.Items["compname"].SubItems[1].Text = WmiInfo.OperatingSystem.ComputerSystemName.IfNullReturnNA();

            Forms.MainForm.listView1.Items["osbuild"].SubItems[1].Text = WmiInfo.OperatingSystem.BuildNumber.IfZeroReturnNA() + (!string.IsNullOrEmpty(RegInfo.OperatingSystem.DisplayVersion) ? $" ({RegInfo.OperatingSystem.DisplayVersion})" : ((RegInfo.OperatingSystem.ReleaseId == null || RegInfo.OperatingSystem.ReleaseId.Length < 1) ? "" : $" ({RegInfo.OperatingSystem.ReleaseId})"));

            Forms.MainForm.listView1.Items["oskernel"].SubItems[1].Text = WmiInfo.OperatingSystem.Version.IfNullReturnNA();

            Forms.MainForm.listView1.Items["installdate"].SubItems[1].Text = (WmiInfo.OperatingSystem.InstallDate.HasValue ? WmiInfo.OperatingSystem.InstallDate.Value.ToString("g") : string.Empty).IfNullReturnNA();

            Forms.MainForm.listView1.Items["compuuid"].SubItems[1].Text = WmiInfo.ComputerSystemProduct.UUID.IfNullReturnNA();

            Forms.MainForm.listView1.Items["syspn"].SubItems[1].Text = WmiInfo.ComputerSystemProduct.SystemProductName.IfNullReturnNA();

            Forms.MainForm.listView1.Items["oskerntype"].SubItems[1].Text = WmiInfo.OperatingSystem.BuildType.IfNullReturnNA();

            Trace.WriteLine("Initializing Free64.CpuidTool class...");

            Tools.Cpuid.Initialize(WmiInfo, RegInfo, CpuidInfo);

            time.Stop();

            Trace.WriteLine($"**Entire initialization is done in: {time.Elapsed.TotalMilliseconds:F2} ms");

            Trace.WriteLine($"\tTime spent on collecting information: {inittime.TotalMilliseconds:F2} ms");

            IsInitialized = true;
        }

        /// <summary>
        /// Close all forms, save settings, and exit program...
        /// </summary>
        public static void CloseProgram()
        {
            Forms.MainForm.SaveFormSettings();
            Application.OpenForms.DisposeAll();
            Application.Exit();
        }

        /// <summary>
        /// Save form settings and restart program
        /// </summary>
        public static void RestartProgram()
        {
            Forms.MainForm.SaveFormSettings();
            Application.OpenForms.DisposeAll();
            Application.Restart();
        }

        // No thought was put into this.
    }
}