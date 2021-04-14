using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Free64
{
    /// <summary>
    /// Class representing Free64 about <see cref="Form"/> itself
    /// </summary>
    public partial class fmAbout : Form
    {
        private byte _activatedEasterEgg = 0;

        public fmAbout()
        {
            InitializeComponent();
            label10.Text = Constants.Version;
        #if NET5_0
            label10.Text += " (running on .NET 5)";
        #elif NET4_0
            label10.Text += " (running on .NET Framework 4)";
        #endif
            label11.Text = Constants.BuildDate.ToString("d MMMM yyyy");
            linkLabel1.Text = Constants.Repository;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = Constants.Repository
            });
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            if (_activatedEasterEgg != 1) return;
            label9.Text  = "Эта пасхалка посвящается всем моим";
            label10.Text = "друзьям и родственникам :D";
            _activatedEasterEgg++;
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            _activatedEasterEgg++;
        }

        private void label11_DoubleClick(object sender, EventArgs e)
        {
            if (_activatedEasterEgg != 2) return;
            label11.Text = "Вы явно очень настырный,";
            label12.Text = "целеустремлённый человек :D.";
            //Please, if you've saw this, don't tell about that to others. Let them find it themselves. ;)
            //Пожалуйста, если вы это увидели, не говорите об этом другим. Дайте им найти это самим. ;)
            _activatedEasterEgg++;
        }
    }
}
