using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.IO;
using FastWebcam;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SolitaireSolver
{
    public class WebcamSource : IDisposable
    {
        public readonly GraphicBuffer UpdateBuffer;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        private bool isCameraRunning = false;
        
        public WebcamSource(GraphicBuffer UpdateBuffer)
        {
            this.UpdateBuffer = UpdateBuffer;
        }

        public void Start()
        {
            CaptureCamera();
            isCameraRunning = true;
        }

        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        private void CaptureCameraCallback()
        {
            if (!isCameraRunning)
            {
                return;
            }
            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);
            capture.FrameWidth = 1920;
            capture.FrameHeight = 1080;
            
            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    capture.Read(frame);

                    image = BitmapConverter.ToBitmap(frame);

#if DEBUG
                    if (GetAsyncKeyState(0x7B) == -32767)
                    {
                        if (!Directory.Exists("imgs"))
                            Directory.CreateDirectory("imgs");

                        var rnd = new Random();
                        var rndName = rnd.Next(0, int.MaxValue);

                        int blockIndex = 0;
                        for (int x = 0; x < image.Width; x += 608)
                        {
                            for (int y = 0; y < image.Height; y+= 608)
                            {
                                blockIndex++;

                                if (image.Width > 300)
                                {
                                    using (Bitmap blockRegion = new Bitmap(608, 608))
                                    {
                                        using (Graphics blockGraphics = Graphics.FromImage(blockRegion))
                                        {
                                            blockGraphics.DrawImage(image, new Rectangle(0, 0, 608, 608), new Rectangle(x, y, 608, 608), GraphicsUnit.Pixel);

                                            var name = $"solitaire_{rndName}_{x}_{y}.png";
                                            blockRegion.Save($"imgs/{name}", ImageFormat.Png);
                                        }
                                    }
                                }
                            }
                        }

                        Console.Beep();
                    }
#endif


                    UpdateBuffer.Draw(image);
                }
            }
        }

        public void Stop()
        {
            camera.Abort();
            capture.Release();
            isCameraRunning = false;
        }

        public void Dispose()
        {
            Stop();
            capture.Dispose();
            frame.Dispose();
            image.Dispose();
        }
    }
}
