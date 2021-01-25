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
    public partial class fmAbout : Form
    {
        public fmAbout()
        {
            InitializeComponent();
            label10.Text = Constants.Version;
            label11.Text = Constants.BuildDate;
            linkLabel1.Text = Constants.Repository;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.Repository);
        }
    }
}
