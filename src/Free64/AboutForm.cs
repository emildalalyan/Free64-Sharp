using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Free64
{
    /// <summary>
    /// Class representing Free64 about <see cref="Form"/> itself
    /// </summary>
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Number of activated easter eggs by user
        /// </summary>
        private byte ActivatedEasterEggs { get; set; } = 0;
        //Please, if you've saw this, don't tell about that to others. Let them find it themselves. ;)
        //Пожалуйста, если вы это увидели, не говорите об этом другим. Дайте им найти это самим. ;)

        public AboutForm()
        {
            InitializeComponent();

            label10.Text = Constants.Version;

        #if NET5_0
            label10.Text += " (running on .NET 5)";
        #elif NET4_0
            label10.Text += " (running on .NET Framework 4)";
        #endif

            label11.Text = Constants.BuildDate.ToString("d MMMM yyyy");
            linkLabel1.Text = Constants.LinkToRepo;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = Constants.LinkToRepo
            });
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            if (ActivatedEasterEggs != 1) return;

            label9.Text  = "Some songs give A LOT of energy,";
            label10.Text = "which goes to work.";

            ++ActivatedEasterEggs;
        }

        private void TitleLabel_DoubleClick(object sender, EventArgs e)
        {
            if (ActivatedEasterEggs == 0)
            {
                ++ActivatedEasterEggs;
                return;
            }

            TitleLabel.Text = "Free64 Nirvana Edition";
        }

        private void label11_DoubleClick(object sender, EventArgs e)
        {
            if (ActivatedEasterEggs != 2)
            {
                label11.Text = "20 February 1967";
                return;
            }

            label11.Text = "The day is done.";
            label12.Text = "But i'm having fun).";

            ++ActivatedEasterEggs;
        }
    }
}
