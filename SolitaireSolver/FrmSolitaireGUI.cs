using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolitaireSolver
{
    public partial class FrmSolitaireGUI : Form
    {
        Graphics graphics;
        Graphics backBufferGraphics;
        Bitmap backBuffer;
        Size originalSize;
        Size cardSize = new Size(65, 100);

        readonly object mutex = new object();

        public FrmSolitaireGUI()
        {
            InitializeComponent();

            originalSize = this.Size;

            ResizeBuffer(this.Width, this.Height);
            Resize += (s, e) => ResizeBuffer(((Form)s).Width, ((Form)s).Height);
            
            new Thread(() => {
                while (true)
                {
                    Thread.Sleep(15);
                    Draw();
                }
            }).Start();
        }


        public void Draw()
        {
            lock (mutex)
            {
                backBufferGraphics.Clear(Color.Green);

                DrawDeck();
                DrawColorStacks();

                graphics.DrawImage(backBuffer, new Point(0, 0));                
            }
        }

        private Size calculateRelativeSize(Size original, Size wantedSize, Size newSize)
        {
            return new Size(
                (int)((wantedSize.Width / (double)original.Width) * newSize.Width),
                (int)((wantedSize.Height / (double)original.Height) * newSize.Height)
            );
        }
        private Point calculateRelativePoint(Size original, Point wantedPoint, Size newSize)
        {
            return new Point(
               (int)((wantedPoint.X / (double)original.Width) * newSize.Width),
               (int)((wantedPoint.Y / (double)original.Height) * newSize.Height)
           );
        }

        private void ResizeBuffer(int width, int height)
        {
            lock (mutex)
            {
                backBufferGraphics?.Dispose();
                graphics?.Dispose();
                
                backBuffer = new Bitmap(width, height);
                backBufferGraphics = Graphics.FromImage(backBuffer);

                graphics = this.CreateGraphics();
            }
        }
    
        private void DrawDeck()
        {
            var relativeCardSize = calculateRelativeSize(originalSize, cardSize, this.Size);
            var relativeCardPoint = calculateRelativePoint(originalSize, new Point(5, 5), this.Size);

            backBufferGraphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(
                new Point(relativeCardPoint.X - 1, relativeCardPoint.Y - 1),
                new Size(relativeCardSize.Width + 1, relativeCardSize.Height + 1)));
            backBufferGraphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(relativeCardPoint, relativeCardSize));
        }

        private void DrawColorStacks()
        {
            for (int i = 0; i < 4; i++)
            {
                var cardPoint = new Point(450 + i*(cardSize.Width+15), 5);

                var relativeCardSize = calculateRelativeSize(originalSize, new Size(65, 100), this.Size);
                var relativeCardPoint = calculateRelativePoint(originalSize, cardPoint, this.Size);

                backBufferGraphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(
                    new Point(relativeCardPoint.X - 1, relativeCardPoint.Y - 1),
                    new Size(relativeCardSize.Width + 1, relativeCardSize.Height + 1)));
                
                backBufferGraphics.FillRectangle(
                    new SolidBrush(Color.White), 
                    new Rectangle(relativeCardPoint, relativeCardSize));
                
                
            }
        }
    }
}
