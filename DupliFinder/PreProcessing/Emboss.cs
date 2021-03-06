﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DupliFinder.Processing
{
    class Emboss : PreprocessingAlgorithm
    {
        /// <summary>
        /// Emboss Laplascian
        /// -1 	0 	-1
        /// 0 	4 	0
        /// -1 	0 	-1 	/1+127
        /// </summary>
        public Emboss()
        {
            _filter = new ConvFilter();
            _filter.SetAll(0);

            _filter.TopLeft = -1;
            _filter.TopRight = -1;
            _filter.BottomLeft = -1;
            _filter.BottomRight = -1;
            _filter.Pixel = 4;

            _filter.Offset = 127;
        }

        public override Bitmap ApplyPreprocessing(Bitmap b)
        {
            PreprocessingAlgorithm.Conv3x3(b, _filter);
            return b;
        }

        public override ConvFilter ConvolutionFilter
        {
            get { return _filter; }
        }
    }
}
