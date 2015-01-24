using System;
using System.Collections.Generic;
using System.Text;

namespace DupliFinder.Processing
{
    /// <summary>
    /// An abstract class defining the basic properties of comparison algorithms.
    /// </summary>
    abstract class ComparisonAlgorithm
    {
        // Algorithms can specify the comparison width by overriding this function.
        public virtual int CompareImageWidth
        {
            get
            {
                return 32;
            }
        }

        public virtual int CompareImageHeight
        {
            get
            {
                return 32;
            }
        }
        

        /// <summary>
        /// This is a typical "useful" similarity value.  Different algorithms have different thresholds
        /// </summary>
        public abstract float SimilarityThreshold { get; }

        /// <summary>
        /// We override the ToString; algorithms _must_ provide a user-visible name.  That name _should_ go
        /// into the resx (see mse/sift/levenshtein for example)
        /// </summary>
        /// <returns></returns>
        public override abstract string ToString();

        /// <summary>
        /// Returns the similarity as a percentage between 0 and 100%, with 0 being completely
        /// dissimmilar and 100 being identical.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public virtual float Similarity(byte[] first, byte[] second)
        {
            float distance = Distance(first, second);
            float maxLen = Math.Max(Math.Max(first.Length, second.Length), distance);
            return (maxLen == 0) ? 1 : (1 - distance / maxLen) * 100;
        }

        abstract public float Distance(byte[] first, byte[] second);

        /// <summary>
        /// Constructs a square macro block with upper left corner at x,y.  
        /// </summary>
        /// <param name="x">The x coordinate of the upper left corner</param>
        /// <param name="y">The y coordinate of the upper left corner</param>
        /// <param name="macrosize">This is the width of the macroblock.  For example, if we want an 8 * 8 macroblock, this should be 8</param>
        /// <param name="source">The array from which we want to extract a macroblock.</param>
        /// <returns></returns>
        public byte[] ConstructMacroblockAt(int x, int y, int macrosize, byte[] source)
        {
            byte[] ret = new byte[macrosize * macrosize];
            int retPos = 0;                                 // our position in the return array
            int sourcePos = x + (y * CompareImageWidth);    // this is our start position in the source array.

            // here, we want to construct a macroblock based on source.
            for (int rows = 0; rows < macrosize; rows++)
            {
                Array.Copy(source, sourcePos, ret, retPos, macrosize);
                retPos += macrosize;
                sourcePos += CompareImageWidth;
            }

            return ret;
        }
    }
}
