using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Additionals
{
    public static class BitmapExtended
    {
        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(this System.Drawing.Bitmap bmp)
        {
            System.Windows.Media.Imaging.BitmapImage bmpImage = new System.Windows.Media.Imaging.BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                bmpImage.BeginInit();
                bmpImage.StreamSource = memory;
                bmpImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bmpImage.EndInit();
            }
            return bmpImage;
        }
    }
}
