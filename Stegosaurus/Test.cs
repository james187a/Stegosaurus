using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Stegosaurus
{
    public static class Test
    {
        public static Bitmap EmbedText(string text, Bitmap bmp)
        {
            foreach (var xPixel in bmp.Height)
            {

            }
        }

        public int Process(int charValue, byte colorElement)
        {
            var leastSignificantBit = colorElement % 2;

            return charValue * 2 + leastSignificantBit;
        }

        public int ClearLeastSignificantBit(byte colorElement)
        {
            return colorElement - colorElement % 2;
        }

        public bool IsLastPixelInImage(long pixelElementIndex)
        {
            return (pixelElementIndex - 1) % 3 < 2;
        }

        public bool Have8BitsBeenProcessed(long pixelElementIndex)
        {
            return pixelElementIndex % 8 == 0;
        }

        public bool HaveAllCharactersBeenHidden(int charIndex, int textLength)
        {
            return charIndex >= textLength;
        }
    }
}
