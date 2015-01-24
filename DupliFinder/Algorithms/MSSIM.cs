using System;
using System.Collections.Generic;
using System.Text;
using DupliFinder.Properties;

namespace DupliFinder.Processing
{
    class MSSIM : ComparisonAlgorithm
    {

        public override int CompareImageWidth
        {
            get
            {
                return 128;
            }
        }
        public override int CompareImageHeight
        {
            get
            {
                return 128;
            }
        }

        public override float SimilarityThreshold
        {
            get { return 70.0F; }   // TODO fix this 
        }

        public override string ToString()
        {
            return Resources.strMSSIM;

        }
        
        SSIM _ssim = new SSIM();

        public override float Distance(byte[] first, byte[] second)
        {
            float finalDistance = 0;
            int macroBlockSize = 32;

            byte[] subFirst = new byte[macroBlockSize*macroBlockSize];
            byte[] subSecond = new byte[macroBlockSize*macroBlockSize];

            for (int y = 0; y < CompareImageWidth; y += macroBlockSize) //iterate horizontally over all macroblocks
            {
                for (int x = 0; x < CompareImageHeight; x += macroBlockSize) //iterate vertically over all macroblocks
                {
                    byte[] macroblock1 = ConstructMacroblockAt(x, y, macroBlockSize, first);
                    byte[] macroblock2 = ConstructMacroblockAt(x, y, macroBlockSize, second);
                    finalDistance += _ssim.Distance(macroblock1, macroblock2);
                }
            }

            return finalDistance/((CompareImageWidth/macroBlockSize)*(CompareImageHeight/macroBlockSize));       
        }

        public override float Similarity(byte[] first, byte[] second)
        {
            // Assumption: return value is between 0 and 1.0
            return 100 * Distance(first, second);
        }

    }
}
