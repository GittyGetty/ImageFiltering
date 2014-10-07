using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Linq;

namespace ImageManipulation
{
    static class FilterFramework
    {
        public static Image ApplyFilter(Image image, IImageFilter filter)
        {
            if (image == null)
            {
                return null;
            }

            var bitmap = new Bitmap(image);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            {
                int sizeBitmap = bitmapData.Stride * bitmap.Height;
                int sizePixel = bitmapData.Stride / bitmap.Width;
                byte[] pixels = new byte[sizeBitmap];

                Marshal.Copy(bitmapData.Scan0, pixels, 0, sizeBitmap);
                {
                    filter.Filter(pixels, sizePixel);
                }
                Marshal.Copy(pixels, 0, bitmapData.Scan0, sizeBitmap);
            }
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
        public static IEnumerable<Type> FindFilters()
        {
            var filterInterface = typeof(IImageFilter);

            // Assume the first type is the interface itself. This may not be guaranteed and
            // this could break.
            var filterTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                     .SelectMany(s => s.GetTypes())
                                                     .Where(p => filterInterface.IsAssignableFrom(p))
                                                     .Skip(1);
            return filterTypes;
        }
    }
}
