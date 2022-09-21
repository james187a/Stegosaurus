using Steganography;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Stegosaurus.Tests
{
    public class SteganographyTests
    {
        [Fact]
        public void Embed_File_In_Bitmap_Returns_Valid_Bitmap()
        {
            // 1. Arrange
            var secretData = Encoding.ASCII.GetBytes("icanhascheezburger");
            var coverFile = Properties.Resources.i_can_has_cheezburger;

            var stego = new SteganographyImage();

            // 2. Act
            var resultFile = stego.EmbedSecretFileInImage(secretData, coverFile);

            using var fileHash = SHA256.Create();

            var stegoStream = new MemoryStream();
            resultFile.Save(stegoStream, ImageFormat.Bmp);
            stegoStream.Position = 0;

            var testStream = new MemoryStream();
            var testFile = Properties.Resources.stegocat;
            testFile.Save(testStream, ImageFormat.Bmp);
            testStream.Position = 0;

            var stegodImageBytes = fileHash.ComputeHash(stegoStream);
            var stegodImageHash = string.Concat(stegodImageBytes.Select(x => x.ToString("X2")));
            var testImageBytes = fileHash.ComputeHash(testStream);
            var testImageHash = string.Concat(testImageBytes.Select(x => x.ToString("X2")));

            // 3. Assert
            Assert.Equal(stegodImageHash, testImageHash);
        }

        [Fact]
        public void Get_Max_Size_of_Secret_To_Be_Hidden()
        {
            var coverFile = Properties.Resources.i_can_has_cheezburger;

            int redValue, greenValue, blueValue;

            for (int i = 0; i < coverFile.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < coverFile.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    var pixel = coverFile.GetPixel(j, i);

                    // now, clear the least significant bit (LSB) from each pixel element
                    redValue = pixel.R - pixel.R % 2;
                    greenValue = pixel.G - pixel.G % 2;
                    blueValue = pixel.B - pixel.B % 2;
                }
            }
        }

        [Fact(Skip = "Only used for creating the stego'd bmp")]
        public void Temp_Save_Stegod_Image()
        {
            var secretData = "icanhascheezburger";
            var coverFile = Properties.Resources.i_can_has_cheezburger;

            var resultFile = StegoHelper.EmbedText(secretData, coverFile);

            resultFile.Save(@"C:\Users\James\stegocat.bmp");
        }
    }
}