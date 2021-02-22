using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMyBrainWPFChart.Utilities
{
    public class Utilities
    {
        public static bool CaptureScreen(string fileName)
        {
            try
            {
                Bitmap captureBitmap = new Bitmap(
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                    System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );
                System.Drawing.Rectangle captureRectangle = System.Windows.Forms.Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                captureBitmap.Save(fileName, ImageFormat.Jpeg);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return false;
            }

        }
    }
}
