using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireSolver
{
    class CardModel
    {
        public string Type { get; set; }
        public Rectangle Bounds { get; set; }
        public double Confidence { get; set; }

        public Rectangle Look_Bounds { get; set; }
        public CardModel(string Type, Rectangle Bounds, Rectangle Look_Bounds, double Confidence)
        {
            this.Type = Type;
            this.Bounds = Bounds;
            this.Confidence = Confidence;
            this.Look_Bounds = Look_Bounds;
        }
    }
}
