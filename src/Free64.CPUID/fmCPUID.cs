using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Common.WindowScreenshot.TakeAndPutIntoClipboard(this);
        }

        private void FileSave_Click(object sender, EventArgs e)
        {
            Common.WindowScreenshot.TakeAndPutIntoFile(this, new SaveFileDialog()
            {
                FileName = "Free64-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm"),
                Filter = "Portable Network Graphics (*.png)|*.png"
            });
        }
    }
}
