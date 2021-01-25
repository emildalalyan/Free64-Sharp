using System;
using System.Windows.Forms;

namespace Free64
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
            Constants.fmMain = this;
            this.Text = "Free64 Extreme Edition v" + Constants.Version;
            treeView1.Nodes[0].Expand();
            toolStripStatusLabel1.Text = "Free64 Extreme Edition v" + Constants.Version;

            Free64Application.Settings.Initialize();

            toolbarToolStripMenuItem.Checked = Free64Application.Settings.Toolbar;
            toolBar.Visible = Free64Application.Settings.Toolbar;

            this.Width = (Free64Application.Settings.Width > 300) ? Free64Application.Settings.Width : 800;
            this.Height = (Free64Application.Settings.Height > 300) ? Free64Application.Settings.Height : 544;

            Free64Application.Initialize();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.PrevVisibleNode == null) return; 
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
            using (fmAbout About = new fmAbout())
            {
                About.ShowDialog();
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Free64Application.Debug.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Free64Application.Initialize();
        }

        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.Repository);
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
            Free64Application.Settings.Toolbar = toolBar.Visible = ConfigControl.Write("Toolbar.Visible", toolbarToolStripMenuItem.Checked);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Constants.exCPUID.Show();
        }

        private void free64CPUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Constants.exCPUID.Show();
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count < 1) return;

            switch (listView2.SelectedItems[0].Index)
            {
                case 0:
                    {
                        Constants.exCPUID.Show();
                        break;
                    }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode.NextVisibleNode == null) return;
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
            Free64Application.SaveFormSettings();
        }
    }
}
