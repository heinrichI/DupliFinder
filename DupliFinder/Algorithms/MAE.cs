using System;
using System.Collections.Generic;
using System.Text;
using DupliFinder.Properties;

namespace DupliFinder.Processing
{
    /// <summary>
    /// A class implementing the Mean Absolute Error algorithm, which should be slightly cheaper than MSE, and 
    /// also not weight outliers so heavily.
    /// </summary>
    class MAE : ComparisonAlgorithm
    {
        public override float Similarity(byte[] first, byte[] second)
        {
            float distance = Distance(first, second);

            int length = first.Length > second.Length ? second.Length : first.Length;

            // Need to convert this to a scale from 0 to 100
            return 100 * (1 - (distance / 255));
        }

        public override float Distance(byte[] first, byte[] second)
        {
            int sum = 0;

            // We'll use which ever array is shorter.
            int length = first.Length > second.Length ? second.Length : first.Length;
            for (int x = 0; x < length; x++)
            {
                sum += Math.Abs(first[x] - second[x]);
            }

            return sum / length;
        }

        // Typical "useful" values seem to be greater than 97%
        public override float SimilarityThreshold
        {
            get { return 87.0F; }
        }

        public override string ToString()
        {
            return Resources.strMAE;
        }
    }

}
