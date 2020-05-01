using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireSolver
{
    public class BlockConfiguration
    {
        public Size BlockSize { get; private set; }

        public BlockConfiguration(Size BlockSize)
        {
            this.BlockSize = BlockSize;
        }
    }
}
