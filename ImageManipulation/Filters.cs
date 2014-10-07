using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageManipulation
{
    class ImageBitRotation : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] = (byte)((pixels[i + 0] << 4) | (pixels[i + 0] >> 4));
                pixels[i + 1] = (byte)((pixels[i + 1] << 4) | (pixels[i + 1] >> 4));
                pixels[i + 2] = (byte)((pixels[i + 2] << 4) | (pixels[i + 2] >> 4));
            }
        }
    }

    class InvertColors : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] = (byte)(255 - pixels[i + 0]);
                pixels[i + 1] = (byte)(255 - pixels[i + 1]);
                pixels[i + 2] = (byte)(255 - pixels[i + 2]);
            }
        }
    }

    class ColorRotation : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                byte tmp = pixels[i + 0];
                pixels[i + 0] = pixels[i + 1];
                pixels[i + 1] = pixels[i + 2];
                pixels[i + 2] = tmp;
            }
        }
    }

    class XorPixels : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                byte tmp = pixels[i];
                pixels[i + 0] ^= pixels[i + 1];
                pixels[i + 1] ^= pixels[i + 2];
                pixels[i + 2] ^= tmp;
            }
        }
    }

    class RotatePixelsByOne : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] = (byte)(pixels[i + 0] << 1 | pixels[i + 0] >> 7);
                pixels[i + 1] = (byte)(pixels[i + 1] << 1 | pixels[i + 1] >> 7);
                pixels[i + 2] = (byte)(pixels[i + 2] << 1 | pixels[i + 2] >> 7);
            }
        }
    }

    class IncrementValues : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0]++;
                pixels[i + 1]++;
                pixels[i + 2]++;
            }
        }
    }


    class Monochrome : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                var avg = (byte)((pixels[i] + pixels[i + 1] + pixels[i + 2]) / 3);

                pixels[i + 0] = avg;
                pixels[i + 1] = avg;
                pixels[i + 2] = avg;
            }
        }
    }

    class XorSquare : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            byte c = 1;
            byte z = 0;

            for (int i = 0; i < pixels.Length; i += bpp)
            {
                z = (byte)((z * z + c) % 255);
                c = z;

                pixels[i + 0] ^= z;
                pixels[i + 1] ^= z;
                pixels[i + 2] ^= z;
            }
        }
    }

    class Stripes : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                byte b = (byte)(i / bpp & 255);

                pixels[i + 0] ^= b;
                pixels[i + 1] ^= b;
                pixels[i + 2] ^= b;
            }
        }
    }

    class CascadingStripes : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                byte b = (byte)((i & 255) & pixels[i] | pixels[i + 1] & pixels[i + 2]);

                pixels[i + 0] ^= b;
                pixels[i + 1] ^= b;
                pixels[i + 2] ^= b;
            }
        }
    }

    class Scatter : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] |= pixels[(i + pixels[i + 2]) % pixels.Length];
                pixels[i + 1] |= pixels[(i + pixels[i + 1]) % pixels.Length];
                pixels[i + 2] ^= pixels[(i + pixels[i + 0]) % pixels.Length];
            }
        }
    }

    class Mixer : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] ^= pixels[(((i % 255) & pixels[i + 0]) * i) % pixels.Length];
                pixels[i + 1] ^= pixels[(((i % 255) & pixels[i + 1]) * i) % pixels.Length];
                pixels[i + 2] ^= pixels[(((i % 255) & pixels[i + 2]) * i) % pixels.Length];
            }
        }
    }

    class Waver : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] += (byte)(Math.Cos(i) * 255);
                pixels[i + 1] += (byte)(Math.Cos(i) * 255);
                pixels[i + 2] += (byte)(Math.Cos(i) * 255);
            }
        }
    }

    class Edge : IImageFilter
    {
        public void Filter(byte[] pixels, int bpp)
        {
            for (int i = 0; i < pixels.Length; i += bpp)
            {
                pixels[i + 0] = pixels[i + 0] > 100 && pixels[i + 0] < 200 ? (byte)0 : pixels[i + 0];
                pixels[i + 1] = pixels[i + 1] > 100 && pixels[i + 1] < 200 ? (byte)0 : pixels[i + 1];
                pixels[i + 2] = pixels[i + 2] > 100 && pixels[i + 2] < 200 ? (byte)0 : pixels[i + 2];
            }
        }
    }
}
