using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DupliFinder.Properties;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace DupliFinder.Processing
{
    /// <summary>
    /// A wrapper class that performs a few basic steps on our bitmaps.  Each instance of the class immediately
    /// resizes the input image to the requested size, and then converts the image to grayscale.  We also discard 
    /// all redundant information in the grayscale image, and distill it down to basic values.
    /// </summary>
    internal class ComparableBitmap : IDisposable
    {
        private Bitmap _bitmap;
        private string _path;
        private Size _originalImageSize = Size.Empty;
        private byte[] _grayScaleData = null;
        public Size OrgImageSize
        {
            get { return _originalImageSize; }
        }

        public string Path
        {
            get { return _path; }
        }
        
        public byte[] GrayscaleData
        {
            get { return _grayScaleData; }
        }

        public ComparableBitmap(string path, int imageWidth, int imageHeight)
        {
            Bitmap bm = (Bitmap)Image.FromFile(path);
            _originalImageSize = bm.Size;
            _path = path;
            _bitmap = ComparableBitmap.resizeImage(bm, imageWidth, imageHeight);

            // TODO we should accept a list of preprocessing filters, and apply those filters
            // in order to the image before finally fetching the grayscale data.
            // TODO should grayscale conversion be an option???
            GrayScale gs = new GrayScale();
            _bitmap = gs.ApplyPreprocessing(_bitmap);
            this._grayScaleData = getBmpBytes(_bitmap);
            

            // make sure we hold no outstanding references to the source material.
            bm.Dispose();
            bm = null;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap resizeImage(Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                // set the resize quality modes to high quality
                // Might as well, since this is hardly the "long pole" for our processing.
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        /// <summary>
        /// During this process, we chuck the RGBA channels.  The RGB channels are all equal, and the A channels is completely
        /// irrelevant.  The remaining byte[] is simply a greyscale luma channel.
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        byte[] getBmpBytes(Bitmap bmp)
        {
            
            BitmapData bData = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int byteCount = (bData.Stride * bmp.Height) / 4;
            int bytesPerPixel = (bData.Stride * bmp.Height) / (bmp.Height * bmp.Width);
            byte[] bmpBytes = new byte[byteCount];

            unsafe
            {
                byte* p = (byte*)(void*)bData.Scan0;
                for (int x = 0; x < bmpBytes.Length; ++x)
                {
                    bmpBytes[x] = *p;
                    p += bytesPerPixel;
                }
            }

            bmp.UnlockBits(bData);
            return bmpBytes;

        }

        Dictionary<ComparableBitmap, float> m_matches = new Dictionary<ComparableBitmap,float>();
        public Dictionary<ComparableBitmap, float> ClosestMatches 
        {
            get { return m_matches; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }
        }

        #endregion
    }
}
