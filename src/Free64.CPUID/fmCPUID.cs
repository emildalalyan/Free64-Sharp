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
    public partial class fmCPUID : Form
    {
        public fmCPUID()
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
            ContextSave.Show(button1, new Point(0,0));
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
                FileName = "Free64-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm"),
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
