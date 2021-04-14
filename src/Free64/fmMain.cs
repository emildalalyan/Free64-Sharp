using System;
using System.Windows.Forms;
using Free64.Properties;

namespace Free64
{
    /// <summary>
    /// Class representing Free64 main <see cref="Form"/> itself
    /// </summary>
    public partial class fmMain : Form
    {
        /// <summary>
        /// Creates new instance of <see cref="fmMain"/> and initializes <see cref="Free64Application"/>
        /// </summary>
        public fmMain()
        {
            InitializeComponent();
            Forms.fmMain = this; //We're setting public variable

            treeView1.Nodes[0].Expand();
            this.Text = toolStripStatusLabel1.Text = "Free64 Extreme Edition v" + Constants.Version;

            toolbarToolStripMenuItem.Checked = toolBar.Visible = Settings.Default.EnableToolbar;
            statusBarToolStripMenuItem.Checked = statusStrip1.Visible = Settings.Default.EnableStatusBar;

            this.Width = Settings.Default.fmMainWidth;
            this.Height = Settings.Default.fmMainHeight;
            this.WindowState = (FormWindowState)Settings.Default.fmMainWindowState;

            Free64Application.Initialize();
        }

        /// <summary>
        /// Save <see cref="fmMain"/> form settings.
        /// </summary>
        public void SaveFormSettings()
        {
            Settings.Default["fmMainWidth"] = (ushort)this.Width;
            Settings.Default["fmMainHeight"] = (ushort)this.Height;
            Settings.Default["fmMainWindowState"] = (byte)this.WindowState;
            Settings.Default.Save();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.PrevVisibleNode == null) return;

            treeView1.SelectedNode.PrevVisibleNode.Expand();
            treeView1.SelectedNode = treeView1.SelectedNode.PrevVisibleNode;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Free64Application.CloseProgram();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Free64Application.RestartProgram();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using fmAbout About = new();
            About.ShowDialog();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Free64Application.GraphicalTrace.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Free64Application.Initialize();
        }

        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = Constants.Repository
            });
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
            treeView1.Nodes[0].Expand();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Free64Application.Initialize();
        }

        private void toolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolBar.Visible = (bool)(Settings.Default["EnableToolbar"] = ((ToolStripMenuItem)sender).Checked);
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = (bool)(Settings.Default["EnableStatusBar"] = ((ToolStripMenuItem)sender).Checked);
        }
        

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Forms.exCPUID.Show();
        }

        private void free64CPUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.exCPUID.Show();
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count < 1) return;

            switch (listView2.SelectedItems[0].Index)
            {
                case 0:
                    {
                        Forms.exCPUID.Show();
                        break;
                    }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.NextVisibleNode == null) return;

            treeView1.SelectedNode = treeView1.SelectedNode.NextVisibleNode;
            treeView1.SelectedNode.Expand();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Name)
            {
                case "noderoot":
                    {
                        panel1.BringToFront();
                        break;
                    }
                case "osinfo":
                    {
                        panel2.BringToFront();
                        break;
                    }
            }
        }

        private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveFormSettings();
        }
    }
}
