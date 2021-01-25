using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Free64.Common
{
    static public class WindowScreenshot
    {
        /// <summary>
        /// Take a screenshot to be in the Clipboard.
        /// </summary>
        /// <param name="Control"></param>
        /// <returns></returns>
        static public Exception TakeAndPutIntoClipboard(Control Control)
        {
            try
            {
                System.Drawing.Bitmap Bitmap = new Bitmap(Control.Width, Control.Height);
                Control.DrawToBitmap(Bitmap, new Rectangle(new Point(0, 0), new Size(Control.Width, Control.Height)));
                Clipboard.SetImage(Bitmap);
            }
            catch(Exception e)
            {
                return e;
            }
            return null;
        }

        /// <summary>
        /// Take a screenshot to be in File.
        /// </summary>
        /// <param name="Control"></param>
        /// <returns></returns>
        static public Exception TakeAndPutIntoFile(Control Control, FileDialog Dialog)
        {
            try
            {
                if (Dialog is null)
                {
                    Dialog = new SaveFileDialog()
                    {
                        Filter = "Portable Network Graphics (*.png)|*.png"
                    };
                }
                using (Bitmap Bitmap = new Bitmap(Control.Width, Control.Height))
                {
                    if (Dialog.ShowDialog() == DialogResult.OK && Dialog.FileName.Length > 0)
                    {
                        Control.DrawToBitmap(Bitmap, new Rectangle(new Point(0, 0), new Size(Control.Width, Control.Height)));
                        Bitmap.Save(Dialog.FileName);
                    }
                }
            }
            catch(Exception e)
            {
                return e;
            }
            if(Dialog != null) Dialog.Dispose();
            return null;
        }

        /// <summary>
        /// Take a screenshot to be in Stream
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Stream"></param>
        /// <returns></returns>
        static public Exception TakeAndPutIntoStream(Control Control, Stream Stream)
        {
            try
            {
                using (Bitmap Bitmap = new Bitmap(Control.Width, Control.Height))
                {
                    if (Stream.CanWrite)
                    {
                        Control.DrawToBitmap(Bitmap, new Rectangle(new Point(0, 0), new Size(Control.Width, Control.Height)));
                        Bitmap.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }
    }
}
