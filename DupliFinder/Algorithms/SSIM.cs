using System;
using System.Collections.Generic;
using System.Text;
using DupliFinder.Properties;

namespace DupliFinder.Processing
{

    /// <summary>
    /// Implementation of Z. Wang SSIM image quality metric
    /// </summary>
    class SSIM : ComparisonAlgorithm
    {
        float C1 = (float)Math.Pow(0.01 * 255, 2);
        float C2 = (float)Math.Pow(0.03 * 255, 2);

        public override int CompareImageWidth
        {
            get
            {
                return 256;
            }
        }

        public override int CompareImageHeight
        {
            get
            {
                return 256;
            }
        }

        /// <summary>
        /// Default value for the similarity threshold.  This is the value that seems to produce the best results with our image database.
        /// </summary>
        public override float SimilarityThreshold
        {
            //get { return 55.0F; } 
            get { return 70.0F; } 
        }

        public override string ToString()
        {
            return Resources.strSSIM;
        }

        public override float Distance(byte[] first, byte[] second)
        {
            float muOfFirst = mu(first);
            float muOfSecond = mu(second);
            float sigmaOfFirst = sigmaSingle(first, muOfFirst);
            float sigmaOfSecond = sigmaSingle(second, muOfSecond);
            float sigmaOfBoth = sigmaDouble(first, second, muOfFirst, muOfSecond);

            return (2 * muOfFirst * muOfSecond + C1) * (2 * sigmaOfBoth + C2) / (((float)Math.Pow(muOfFirst, 2) + (float)Math.Pow(muOfSecond, 2) + C1) * ((float)Math.Pow(sigmaOfFirst, 2) + (float)Math.Pow(sigmaOfSecond, 2) + C2));                         
        }

        public override float Similarity(byte[] first, byte[] second)
        {
            // Assumption: return value is between 0 and 1.0
            return 100 * Distance(first, second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public float mu(byte[] image)
        {
            float sum = 0;
            foreach (byte value in image)
            {
                sum += value;
            }
            return sum / (float)image.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public float sigmaSingle(byte[] image, float mu)
        {
            float sum = 0;
            foreach (byte value in image)
            {
                sum += (float)Math.Pow(value - mu, 2);
            }
            return (float)Math.Pow(sum / ((float)image.Length - 1), 0.5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public float sigmaDouble(byte[] imageOne, byte[] imageTwo, float muOne, float muTwo)
        {
            float sum = 0;
            for (int i = 0; i < imageOne.Length; i++)
            {
                sum += ((imageOne[i] - muOne) * (imageTwo[i] - muTwo));
            }
            return sum / ((float)imageOne.Length - 1);
        }
    }
}
