using System;
using System.Drawing;

namespace Stegosaurus
{
    public interface ISteganographyImage
    {
        Bitmap EmbedSecretFileInImage(byte[] secretFile, Bitmap coverFile);
        byte[] ExtractSecretFileFromImage(Bitmap coverFile);
        int GetNumberOfBitsAbleToBeEmbedded(Bitmap coverFile);
    }
}