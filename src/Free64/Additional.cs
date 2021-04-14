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
