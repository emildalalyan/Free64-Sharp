using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Free64.Common;

namespace Free64.CPUID
{
    /// <summary>
    /// This class is representing <see cref="CpuidTool"/> main form itself.
    /// </summary>
    public partial class CpuidToolForm : Form
    {
        public CpuidToolForm()
        {
            InitializeComponent();
        }

        private void fmCPUID_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ContextSave.Show(SaveButton, new Point(0,0));
        }

        private void ClipboardSave_Click(object sender, EventArgs e)
        {
            using Bitmap bitmap = this.TakeScreen();
            Clipboard.SetImage(bitmap);
        }

        private void FileSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dlg = new()
            {
                FileName = $"Free64-{DateTime.Now:dd-MM-yyyy-HH-mm}",
                Filter = "Portable Network Graphics (*.png)|*.png"
            };

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                using Bitmap bitmap = this.TakeScreen();
                bitmap.Save(dlg.FileName, ImageFormat.Png);
            }
        }
    }
}
