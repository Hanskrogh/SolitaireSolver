using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireSolver
{
    public class DrawEventArgs : EventArgs
    {
        public readonly Graphics BackBufferGraphics;
        public readonly Image Source;

        public DrawEventArgs(Graphics BackBufferGraphics, Image Source)
        {
            this.BackBufferGraphics = BackBufferGraphics;
            this.Source = Source;

        }
    }
}
