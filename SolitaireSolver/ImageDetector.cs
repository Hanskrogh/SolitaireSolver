using Alturos.Yolo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireSolver
{
    class ImageDetector
    {
        private readonly YoloWrapper Wrapper;

        public ImageDetector(YoloWrapper Wrapper)
        {
            this.Wrapper = Wrapper;
        }

        public CardModel[] GetCards(Bitmap image, Rectangle look_bounds, double confidence = 0.4d) 
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                var items = Wrapper.Detect(ms.ToArray());
                return items
                    .Where(item => item.Confidence > confidence)
                    .Select(item => new CardModel(item.Type, new Rectangle(item.X, item.Y, item.Width, item.Height), look_bounds, item.Confidence))
                    .ToArray();
            }
        }
    }
}
