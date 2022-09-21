using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Collections;
using Steganography;
using Microsoft.Win32;

namespace Stegosaurus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnterText(object sender, RoutedEventArgs e)
        {
            var image = new Bitmap(new MemoryStream(Properties.Resources.i_can_has_cheezburger));
            var text = mainInput.Text;

            var pixelCount = image.Width * image.Height;

            var pixelList = new List<StegoPixel>();

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var color = image.GetPixel(x, y);
                    var stegoPixel = new StegoPixel(color, x, y);
                    pixelList.Add(stegoPixel);
                }
            }

            var resultImage = StegoHelper.EmbedText(text, image);

            //var stegoPixels = new StegoPixels(pixelList);

            mainOutput.Text = $"{pixelCount} pixels.";
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dlg = new OpenFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".txt", // Default file extension
                Filter = string.Empty
            };

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                mainInput.Text = filename;

                var uri = new Uri(filename);

                imageField.Source = new BitmapImage(uri);
            }

            
        }
    }

    public class StegoPixel
    {
        public StegoPixel(System.Drawing.Color color, int xCoordinate, int yCoordinate)
        {
            Color = color;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }

        public System.Drawing.Color Color { get; set; }

        public int XCoordinate { get; set; }

        public int YCoordinate { get; set; }
    }

    //public class StegoPixels : IEnumerable<StegoPixel>
    //{
    //    public StegoPixels(IEnumerable<StegoPixel> list)
    //    {
    //        Pixels = list;
    //    }

    //    public IEnumerable<StegoPixel> Pixels { get; set; }

    //    private class StegoEnumerator : IEnumerator<StegoPixel>
    //    {
    //        private bool disposedValue;

    //        public IEnumerable<StegoPixel> stegoPixels;

    //        public StegoEnumerator(IEnumerable<StegoPixel> pixels)
    //        {
    //            stegoPixels = pixels;
    //        }

    //        public StegoPixel Current => throw new NotImplementedException();

    //        object IEnumerator.Current => throw new NotImplementedException();

    //        public bool MoveNext()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void Reset()
    //        {
    //            throw new NotImplementedException();
    //        }

    //        protected virtual void Dispose(bool disposing)
    //        {
    //            if (!disposedValue)
    //            {
    //                if (disposing)
    //                {
    //                    // TODO: dispose managed state (managed objects)
    //                }

    //                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
    //                // TODO: set large fields to null
    //                disposedValue = true;
    //            }
    //        }

    //        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    //        // ~StegoPixels()
    //        // {
    //        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //        //     Dispose(disposing: false);
    //        // }

    //        public void Dispose()
    //        {
    //            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //            Dispose(disposing: true);
    //            GC.SuppressFinalize(this);
    //        }
    //    }

    //    public IEnumerator<StegoPixel> GetEnumerator()
    //    {
    //        return new StegoEnumerator(Pixels);
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
