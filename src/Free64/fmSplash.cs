using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Free64
{
    public partial class fmSplash : Form
    {
        public fmSplash()
        {
            InitializeComponent();
            pictureBox1.Parent = pictureBox2;
            label1.Parent = pictureBox2;
            label2.Parent = pictureBox2;
            label2.Text = Constants.Version + " | by Emil Dalalyan";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
