using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DupliFinder.Processing
{
    /// <summary>
    /// Class for converting the image to grayscale.
    /// </summary>
    class GrayScale : PreprocessingAlgorithm
    {
        /// <summary>
        /// A colormatric we use to convert the image to grayscale.
        /// </summary>
        public static ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

        public override Bitmap ApplyPreprocessing(System.Drawing.Bitmap b)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(b.Width, b.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the grayscale color matrix
                g.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height),
                   0, 0, b.Width, b.Height, GraphicsUnit.Pixel, attributes);

            }

            return newBitmap;
        }

        /// <summary>
        /// Grayscale preprocessing doesn't require a convolution filter.
        /// </summary>
        public override ConvFilter ConvolutionFilter
        {
            get { return null; }
        }
    }
}
