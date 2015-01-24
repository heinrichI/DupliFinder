using System;
using System.Collections.Generic;
using System.Text;
using DupliFinder.Properties;

namespace DupliFinder.Processing
{
    class MSE : ComparisonAlgorithm
    {

        public override float Similarity(byte[] first, byte[] second)
        {
            float distance = Distance(first, second);

            int length = first.Length > second.Length ? second.Length : first.Length;

            // Need to convert this to a scale from 0 to 100
            int maxunsimilarity = (int)Math.Pow(255, 2) * length / length;
            return 100 * (1 - (distance / maxunsimilarity));
        }

        public override float Distance(byte[] first, byte[] second)
        {
            int sum = 0;

            // We'll use which ever array is shorter.
            int length = first.Length > second.Length ? second.Length : first.Length;
            for (int x = 0; x < length; x++)
            {
                sum = sum + (int) Math.Pow((first[x] - second[x]),2);
            }

            return sum / length;
        }

        // Typical "useful" values seem to be greater than 97%
        public override float SimilarityThreshold
        {
            get { return 97.0F; }
        }

        public override string ToString()
        {
            return Resources.strMSE;
        }
    }
}