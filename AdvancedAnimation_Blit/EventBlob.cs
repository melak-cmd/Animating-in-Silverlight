using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdvancedAnimation_Blit
{
    public class EventBlob
    {
        public WriteableBitmapExtensions.BlendMode BlendMode;
        public Color Color;
        public Point Position;

        public double FadeRate { get; set; }

        public WriteableBitmap Img { get; set; }

        public double Opacity { get; set; }

        public Rect SourceRect { get; set; }

        public double Vx { get; set; }

        public double Vy { get; set; }
    }
}