using AForge.Video.DirectShow;
using Alturos.Yolo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolitaireSolver
{
    public partial class FrmSolitaire : Form
    {
        GraphicBuffer gBuffer;
        readonly object mutex = new object();

        public FrmSolitaire()
        {
            InitializeComponent();
            
            var yoloWrapper = InitializeYoloWrapper();
            if (yoloWrapper == default) return;//error case.
            var detector = new ImageDetector(yoloWrapper);

            gBuffer = new GraphicBuffer(pnlGraphics);
            WebcamSource source = new WebcamSource(gBuffer);

            Brush redBrush = new SolidBrush(Color.Red);
            Brush greenBrush = new SolidBrush(Color.Green);
            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);
            Pen redPen = new Pen( redBrush, 2 );
            Pen blackPen = new Pen(blackBrush, 2);

            CardModel[] oldCardModels = default;
            Image lastSource = default;

            Rectangle[] regions = default;

            Thread detectionThread = new Thread(() => {
                while (true)
                {
                    if (lastSource != default)
                    {
                        Image sourceCopy = null;
                        lock (mutex)
                        {
                            sourceCopy = (Image)lastSource.Clone();
                            if (regions == default) regions = GenerateRegions(sourceCopy);
                        }

                        var foundCards = new List<CardModel>();

                        // split image into bitmaps of 608x608
                        for (int x = 0; x < sourceCopy.Width; x += 608)
                        {
                            for (int y = 0; y < sourceCopy.Height; y += 608)
                            {
                                var currentRegionBmp = new Bitmap(608, 608);
                                var currentRegionBmpGraphics = Graphics.FromImage(currentRegionBmp);

                                var LookBounds = new Rectangle(x, y, 608, 608);
                                currentRegionBmpGraphics.DrawImage(sourceCopy, new Rectangle(0, 0, 608, 608), LookBounds, GraphicsUnit.Pixel);

                                foundCards.AddRange(detector.GetCards((Bitmap)currentRegionBmp, LookBounds, 0));
                                currentRegionBmpGraphics.Dispose();
                            }
                        }

                        lock (mutex)
                        {
                            oldCardModels = foundCards.ToArray();
                        }
                        sourceCopy.Dispose();
                    }
                }
            });
            detectionThread.Start();


            
            gBuffer.OnDraw += e => 
            {
                lock (mutex)
                {
                    lastSource = e.Source;

                    if (regions != default)
                    {
                        int regionIndex = 0;
                        foreach (var region in regions)
                        {
                            regionIndex++;

                            e.BackBufferGraphics.DrawRectangle(blackPen, region);
                            
                        }
                    }

                    if (oldCardModels != default)
                    {
                        foreach (var card in oldCardModels)
                        {
                            var bounds = new Rectangle(card.Bounds.X + card.Look_Bounds.X, card.Bounds.Y + card.Look_Bounds.Y, card.Bounds.Width, card.Bounds.Height);

                            e.BackBufferGraphics.DrawRectangle(redPen, bounds);

                            var offset = new Point(0, -25);
                            var confidence = card.Confidence;
                            confidence *= 100;
                            

                            e.BackBufferGraphics.DrawString($"{card.Type}: {(int)confidence}%", this.Font, blackBrush, new Point(offset.X + bounds.X + 1, offset.Y + bounds.Y + 1));
                            e.BackBufferGraphics.DrawString($"{card.Type}: {(int)confidence}%", this.Font, greenBrush, new Point(offset.X + bounds.X, offset.Y + bounds.Y));
                        }
                    }
                }

                e.BackBufferGraphics.DrawString($"FPS: {gBuffer.fps}", this.Font, blackBrush, new Point(5, 5));
                e.BackBufferGraphics.DrawString($"FPS: {gBuffer.fps}", this.Font, whiteBrush, new Point(4, 4)); 
            };
            

            
            source.Start();
            FormClosing += (s, e) =>
            {
                source.Stop();
                detectionThread.Abort();
            };
        }


        YoloWrapper InitializeYoloWrapper()
        {
            YoloWrapper yoloWrapper = new YoloWrapper($"trainfiles/solitaire_images.cfg", $"trainfiles/solitaire_images.weights", $"trainfiles/solitaire_images.names");

            if (yoloWrapper.EnvironmentReport.CudaExists == false)
            {
                MessageBox.Show("Install CUDA 10");
                return default;
            }
            if (yoloWrapper.EnvironmentReport.CudnnExists == false)
            {
                MessageBox.Show("Cudnn doesn't exist");
                return default;
            }
            if (yoloWrapper.EnvironmentReport.MicrosoftVisualCPlusPlusRedistributableExists == false)
            {
                MessageBox.Show("Install Microsoft Visual C++ 2017 Redistributable");
                return default;
            }
            if (yoloWrapper.DetectionSystem.ToString() != "GPU")
            {
                MessageBox.Show("No GPU card detected. Exiting...");
                return default;
            }

            return yoloWrapper;
        }

        Rectangle[] GenerateRegions(Image source)
        {
            List<Rectangle> regions = new List<Rectangle>();
            for (int x = 0; x < source.Width; x += 608)
            {
                for (int y = 0; y < source.Height; y +=608)
                {
                    regions.Add(new Rectangle(x, y, 608, 608));
                }
            }
            return regions.ToArray();
        }
    }
}
