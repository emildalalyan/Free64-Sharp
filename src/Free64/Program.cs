using System;
using System.Windows.Forms;
using Free64.Information;
using Free64.CPUID;
using System.Globalization;
using System.Threading;
using Free64.Common;
using System.Threading.Tasks;

namespace Free64
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] // He came into your apartment and choosed Single-Thread Apartment
        
        static void Main() // A main function. The program starts here.
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Application.EnableVisualStyles(); // We enable visual styles.
            Application.SetCompatibleTextRenderingDefault(false); // We're switching off compatible text rendering.
            Application.Run(new fmMain()); // Don't make a fight :D. The application is started.
        }
    }

    public static class Constants
    {
        /// <summary>
        /// Version constant
        /// </summary>
        public const string Version = "0.5-alpha";

        /// <summary>
        /// Assembly Build Date
        /// </summary>
        public const string BuildDate = "January 2021";

        /// <summary>
        /// Instance of <see cref="Free64.fmMain"/>
        /// </summary>
        public static fmMain fmMain; // Instance of fmMain, it will be filled by fmMain constructor called by Program.Main()

        /// <summary>
        /// Instance of <see cref="Free64.CPUID.GUICPUID"/> class
        /// </summary>
        public static GUICPUID exCPUID = new GUICPUID();

        /// <summary>
        /// Free64 Repository Link
        /// </summary>
        public const string Repository = "https://github.com/emildalalyan/Free64-Sharp";
        // We're defining the varibles
    }
    
    public static partial class Free64Application
    {
        /// <summary>
        /// Instance of <see cref="Debug.Debug"/> class
        /// </summary>
        public static Debug.Debug Debug = new Debug.Debug();

        /// <summary>
        /// Instance of <see cref="Free64.Information.WMI"/> class
        /// </summary>
        public static WMI Information = new WMI(false, Debug);

        /// <summary>
        /// Instance of <see cref="Free64.Information.Registry"/> class
        /// </summary>
        public static Registry RegInfo = new Registry(false, Debug);

        /// <summary>
        /// Instance of <see cref="Free64.Free64Application.Settings"/> struct
        /// </summary>
        public static Settings Settings = new Settings();

        /// <summary>
        /// Instance of <see cref="Free64.Information.CPUID"/> class
        /// </summary>
        public static Information.CPUID CPUID = new Information.CPUID(false, Debug);


        /// <summary>
        /// Initialization function
        /// </summary>
        public static void Initialize() // Initialization function
        {
            Debug.Clear(); // We're cleaning the debug list

            Information.Reset();

            Debug.SendMessage("Starting an initialization...");

            int Completed = Debug.SendMessage("Getting date/time point...");

            double time = CommonThings.GetUNIXTime(); // We're figuring out the current UNIX time.

            Debug.AppendToMessage(" Completed Successfully...", Completed);

            Debug.SendMessage($"Free64 Extreme Version: {Constants.Version} [build from {Constants.BuildDate}]"); // Showing Free64 Version

            Debug.SendMessage($"Assembly platform: {(Environment.Is64BitProcess ? "Win64" : "Win32")}"); // Showing command line arguments

            Debug.SendMessage("Showing splash screen...");

            fmSplash Splash = new fmSplash();

            Splash.Show();

            Debug.SendMessage("Initializing Free64.Information.WMI class...");

            Information.Initialize();

            Debug.SendMessage("Initializing Free64.Information.Registry class...");

            RegInfo.Initialize();

            Debug.SendMessage("Working with the GUI...");

            foreach (ListViewItem a in Constants.fmMain.listView1.Items)
            {
                switch (a.Tag)
                {
                    case "osname":
                        {
                            a.SubItems[1].Text = Information.OperatingSystem.Caption.IfNullReturnNA();
                            break;
                        }
                    case "compname":
                        {
                            a.SubItems[1].Text = Information.OperatingSystem.ComputerSystemName.IfNullReturnNA();
                            break;
                        }
                    case "osbuild":
                        {
                            a.SubItems[1].Text = Information.OperatingSystem.BuildNumber.IfNullReturnNA() + $" (Release ID: {RegInfo.Information.ReleaseId.IfNullReturnNA()})";
                            break;
                        }
                    case "oskernel":
                        {
                            a.SubItems[1].Text = Information.OperatingSystem.Version.IfNullReturnNA();
                            break;
                        }
                    case "installdate":
                        {
                            a.SubItems[1].Text = Information.OperatingSystem.InstallDate.ToString("g");
                            break;
                        }
                }
            }

            Debug.SendMessage("Initializing Free64.Information.CPUID class...");

            CPUID.Initialize();

            Debug.SendMessage("Initializing Free64.CPUID.GUICPUID class...");

            Constants.exCPUID.Initialize(Information, RegInfo, CPUID);

            Debug.SendMessage("Hiding the splash...");

            Splash.Hide();
            Splash.Dispose();

            Debug.SendMessage($"Initializing is done! ({Math.Round((CommonThings.GetUNIXTime() - time), 3)} sec)");

            // I'm bad. You know it. :D
            // Clear from code, i'm so bad ;)

            Completed = Debug.SendMessage("Collecting garbage from a Memory...");
            try
            {
                GC.Collect();
                Debug.AppendToMessage(" Succeeded...", Completed);
            }
            catch(Exception e)
            {
                Debug.SendMessage($"[GarbageCollector] {e.Message}...");
                Debug.AppendToMessage(" Failed...", Completed);
            }
        }

        /// <summary>
        /// Save Form Width and Height
        /// </summary>
        public static void SaveFormSettings()
        {
            ConfigControl.Write("fmMain.Width", Constants.fmMain.Width);
            ConfigControl.Write("fmMain.Height", Constants.fmMain.Height);
        }

        /// <summary>
        /// Close all forms, save settings, and exit program...
        /// </summary>
        public static void CloseProgram()
        {
            Free64Application.SaveFormSettings();
            Free64Application.CloseAllForms();
            Application.Exit();
        }

        /// <summary>
        /// Restart program
        /// </summary>
        public static void RestartProgram()
        {
            Free64Application.SaveFormSettings();
            Free64Application.CloseAllForms();
            Application.Restart();
        }

        // Will you tell us, that you're okay?
        // Especially after looking at my code ;)
    }
}
