using System;
using System.Collections.Generic;
using System.Text;
using DupliFinder.Properties;

namespace DupliFinder.Processing
{
    internal class SIFT : ComparisonAlgorithm
    {
        int _maxOffset = 20; // TODO not sure what this value should be!
        public SIFT() { }

        /// <summary>
        /// Calculate a distance similar to Levenstein, but faster and less reliable.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public override float Distance(byte[] x, byte[] y)
        {
            SiftStringSimilarity.StringSift3 siftComparer = new SiftStringSimilarity.StringSift3(_maxOffset);
            return siftComparer.Distance(Encoding.ASCII.GetString(x), Encoding.ASCII.GetString(y));
        }

        public override float SimilarityThreshold
        {
            get { return 65.0F; }
        }

        public override string ToString()
        {
            return Resources.strSift;
        }
    }
}
