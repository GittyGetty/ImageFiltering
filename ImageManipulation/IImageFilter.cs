using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public interface IImageFilter
    {
        void Filter(byte[] pixels, int bpp);
    }
}
