using System;
using System.Drawing;

namespace Stegosaurus
{
    public class SteganographyImage : ISteganographyImage
    {
        public Bitmap CoverFile { get; set; }
        public byte[] SecretData { get; set; }
        public Bitmap OutputFile { get; set; }

        public SteganographyImage()
        {
        }

        public SteganographyImage(byte[] secretData, Bitmap coverFile)
        {
            SecretData = secretData;
            CoverFile = coverFile;
        }

        public Bitmap EmbedSecretFileInImage(byte[] secretData, Bitmap coverFile)
        {
            // initially, we'll be hiding characters in the image
            var state = State.Hiding;

            // holds the index of the character that is being hidden
            var charIndex = 0;

            // holds the value of the character converted to integer
            var charValue = 0;

            // holds the index of the color element (R or G or B) that is currently being processed
            var pixelElementIndex = 0L;

            // holds the number of trailing zeros that have been added when finishing the process
            var zeros = 0;

            // hold pixel elements
            int R, G, B;

            // pass through the rows
            for (int i = 0; i < coverFile.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < coverFile.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    var pixel = coverFile.GetPixel(j, i);

                    // now, clear the least significant bit (LSB) from each pixel element
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        // check if new 8 bits has been processed
                        if (pixelElementIndex % 8 == 0)
                        {
                            // check if the whole process has finished
                            // we can say that it's finished when 8 zeros are added
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                // apply the last pixel on the image
                                // even if only a part of its elements have been affected
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    coverFile.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                // return the bitmap with the text hidden in
                                return coverFile;
                            }

                            // check if all characters has been hidden
                            if (charIndex >= secretData.Length)
                            {
                                // start adding zeros to mark the end of the text
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                // move to the next character and process again
                                charValue = secretData[charIndex++];
                            }
                        }

                        // check which pixel element has the turn to hide a bit in its LSB
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        // the rightmost bit in the character will be (charValue % 2)
                                        // to put this value instead of the LSB of the pixel element
                                        // just add it to it
                                        // recall that the LSB of the pixel element had been cleared
                                        // before this operation
                                        R += charValue % 2;

                                        // removes the added rightmost bit of the character
                                        // such that next time we can reach the next one
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }

                                    coverFile.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.Filling_With_Zeros)
                        {
                            // increment the value of zeros until it is 8
                            zeros++;
                        }
                    }
                }
            }

            return coverFile;
        }

        public byte[] ExtractSecretFileFromImage(Bitmap coverFile)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfBitsAbleToBeEmbedded(Bitmap coverFile)
        {
            throw new NotImplementedException();
        }
    }
}