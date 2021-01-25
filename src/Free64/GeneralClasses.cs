using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace Free64
{
    /// <summary>
    /// Struct, representing settings in Free64
    /// </summary>
    public struct Settings
    {
        /// <summary>
        /// Width of fmMain
        /// </summary>
        public ushort Width;

        /// <summary>
        /// Width of fmMain
        /// </summary>
        public ushort Height;

        /// <summary>
        /// Do taskbar has enabled?
        /// </summary>
        public bool Toolbar;

        /// <summary>
        /// Get information from Config
        /// </summary>
        public void Initialize()
        {
            Width = Convert.ToUInt16(ConfigControl.Read("fmMain.Width"));
            Height = Convert.ToUInt16(ConfigControl.Read("fmMain.Height"));
            Toolbar = Convert.ToBoolean(ConfigControl.Read("Toolbar.Visible"));
        }
    }
    public static partial class Free64Application
    {
        /// <summary>
        /// Close all forms in program
        /// </summary>
        public static void CloseAllForms()
        {
            for(int i = 0; i < Application.OpenForms.Count; i++)
            {
                Application.OpenForms[i].Dispose();
            }
        }
    }
}
