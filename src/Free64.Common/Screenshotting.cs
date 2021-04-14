using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

#nullable enable

namespace Free64.Common
{
    /// <summary>
    /// Class, which can make screenshot of Windows Forms elements
    /// </summary>
    public static class WindowsFormsScreenshot
    {
        /// <summary>
        /// Take a screenshot of <see cref="Control"/> and return <see cref="Bitmap"/> with it.
        /// </summary>
        /// <param name="Control"></param>
        public static Bitmap TakeScreen(this Control Control)
        {
            Bitmap bitmap = new(Control.Width, Control.Height);
            Control.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), new Size(Control.Width, Control.Height)));
            return bitmap;
        }
    }
}
