using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdvancedAnimation_Blit
{
    public class Plotter : UserControl
    {
        private const int Canheight = 400;
        private const int Canwidth = 460;
        private const int Numpblobs = 3000;
        private readonly Random _rand = new Random((int) DateTime.Now.Ticks);
        private EventBlob[] _data;
        private bool _mouseDown;
        private double _mouseVx;
        private double _mouseVy;
        private double _mouseX;
        private double _mouseY;
        private WriteableBitmap _particleBmp;
        private double _prevMouseX;
        private double _prevMouseY;

        public Plotter()
        {
            Setup();
        }

        public WriteableBitmap TargetBmp { get; set; }

        public void SetMouse(Point p)
        {
            if (p.X >= Canwidth - 10 || p.Y >= Canheight - 10 || p.X < 10 || p.Y < 10)
            {
                _mouseX = _mouseY = Canwidth*0.5;
            }
            else
            {
                _mouseX = p.X;
                _mouseY = p.Y;
            }
        }

        public void SetMouseClick(bool isDown)
        {
            _mouseDown = isDown;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            Update();
            DrawBitmap();
        }

        private void DrawBitmap()
        {
            //Clear Existing Bitmap
            TargetBmp.Clear();

            //Redraw each particle in updated positions
            foreach (EventBlob b in _data)
            {
                TargetBmp.Blit(b.Position, b.Img, b.SourceRect, b.Color, b.BlendMode);
                    //using WriteableBitmapEx Blip for extra perf
            }

            //Force Redraw on Writeable Bitmap
            TargetBmp.Invalidate();
        }

        private void Setup()
        {
            //Load Resources
            _particleBmp = Util.LoadBitmap("/AdvancedAnimation_Blit;component/images/particle.png");

            //Animation Timer
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            _mouseX = _prevMouseX = Canwidth*0.5;
            _mouseY = _prevMouseY = Canheight*0.5;

            _data = new EventBlob[Numpblobs];

            for (int i = 0; i < Numpblobs; i++)
            {
                var eb = new EventBlob();

                eb.Img = _particleBmp;
                eb.SourceRect = new Rect(0, 0, 6, 6);
                eb.Position.X = Math.Cos(i)*_rand.Next(0, Canwidth);
                eb.Position.Y = Math.Sin(i)*_rand.Next(0, Canheight);
                eb.Vx = Math.Cos(i)*34;
                eb.Vy = Math.Sin(i)*34;
                eb.Opacity = 1;
                eb.BlendMode = WriteableBitmapExtensions.BlendMode.Additive;
                eb.FadeRate = _rand.NextDouble()*0.005 + 0.002;
                var c = new Color
                {
                    A = (byte) _rand.Next(50, 255),
                    R = (byte) _rand.Next(0, 255),
                    G = (byte) _rand.Next(0, 255),
                    B = (byte) _rand.Next(0, 255)
                };
                eb.Color = c;
                _data[i] = eb;
            }
            ;
        }

        private void Update()
        {
            //Do Math and update each particle in the array

            _mouseVx = _mouseX - _prevMouseX;
            _mouseVy = _mouseY - _prevMouseY;
            const double toDist = Canwidth*.86;
            const double stirDist = Canwidth*0.055;
            const double blowDist = Canwidth*0.25;

            foreach (EventBlob b in _data)
            {
                double x = b.Position.X;
                double y = b.Position.Y;
                double vx = b.Vx;
                double vy = b.Vy;

                double dx = x - _mouseX;
                double dy = y - _mouseY;
                double d = Math.Sqrt(dx*dx + dy*dy);
                if (d == 0) d = 0.001;
                dx /= d;
                dy /= d;

                if (_mouseDown)
                {
                    if (d < blowDist)
                    {
                        double blowAcc = (1 - (d/blowDist))*14;
                        vx += dx*blowAcc + 0.5 - _rand.NextDouble();
                        vy += dy*blowAcc + 0.5 - _rand.NextDouble();
                    }
                }

                if (d < toDist)
                {
                    double toAcc = (1 - (d/toDist))*Canwidth*0.0014;
                    vx -= dx*toAcc;
                    vy -= dy*toAcc;
                }

                if (d < stirDist)
                {
                    double mAcc = (1 - (d/stirDist))*Canwidth*0.00026;
                    vx += _mouseVx*mAcc;
                    vy += _mouseVy*mAcc;
                }

                vx *= 0.96;
                vy *= 0.96;

                double avgVx = Math.Abs(vx);
                double avgVy = Math.Abs(vy);
                double avgV = (avgVx + avgVy)*0.5;

                if (avgVx < .1) vx *= _rand.NextDouble()*3;
                if (avgVy < .1) vy *= _rand.NextDouble()*3;

                double sc = avgV*0.45;
                sc = Math.Max(Math.Min(sc, 3.5), 0.4);

                double nextX = x + vx;
                double nextY = y + vy;

                if (nextX > Canwidth)
                {
                    nextX = Canwidth;
                    vx *= -1;
                }
                else if (nextX < 0)
                {
                    nextX = 0;
                    vx *= -1;
                }

                if (nextY > Canheight)
                {
                    nextY = Canheight;
                    vy *= -1;
                }
                else if (nextY < 0)
                {
                    nextY = 0;
                    vy *= -1;
                }

                b.Vx = vx;
                b.Vy = vy;
                b.Position.X = nextX;
                b.Position.Y = nextY;
            }
        }
    }
}