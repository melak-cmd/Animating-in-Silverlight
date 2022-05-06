using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using Artefact.Animation;

namespace AdvancedAnimation_Blit
{
    [Guid("F0AE4717-E3F9-463F-9C47-C0E2CDDB1E04")]
    public static class Helper
    {
        public static readonly Random Rnd = new Random();

        public static EaseObject AnimateZProps(UIElement element,
            double rotationX, double rotationY, double rotationZ,
            double localOffsetX, double localOffsetY, double localOffsetZ,
            double globalOffsetX, double globalOffsetY, double globalOffsetZ,
            double centerOfRotationX, double centerOfRotationY, double centerOfRotationZ,
            double time, PercentHandler ease, double delay)
        {
            return ArtefactAnimator.AddEase(element, UIElement.ProjectionProperty,
                new PlaneProjection
                {
                    RotationX = rotationX,
                    RotationY = rotationY,
                    RotationZ = rotationZ,
                    LocalOffsetX = localOffsetX,
                    LocalOffsetY = localOffsetY,
                    LocalOffsetZ = localOffsetZ,
                    GlobalOffsetX = globalOffsetX,
                    GlobalOffsetY = globalOffsetY,
                    GlobalOffsetZ = globalOffsetZ,
                    CenterOfRotationX = centerOfRotationX,
                    CenterOfRotationY = centerOfRotationY,
                    CenterOfRotationZ = centerOfRotationZ,
                },
                time, ease, delay);
        }

        public static PlaneProjection Copy(PlaneProjection planeProjection)
        {
            return planeProjection == null
                ? new PlaneProjection()
                : new PlaneProjection
                {
                    CenterOfRotationX = planeProjection.CenterOfRotationX,
                    CenterOfRotationY = planeProjection.CenterOfRotationY,
                    CenterOfRotationZ = planeProjection.CenterOfRotationZ,
                    RotationX = planeProjection.RotationX,
                    RotationY = planeProjection.RotationY,
                    RotationZ = planeProjection.RotationZ,
                    LocalOffsetX = planeProjection.LocalOffsetX,
                    LocalOffsetY = planeProjection.LocalOffsetY,
                    LocalOffsetZ = planeProjection.LocalOffsetZ,
                    GlobalOffsetX = planeProjection.GlobalOffsetX,
                    GlobalOffsetY = planeProjection.GlobalOffsetY,
                    GlobalOffsetZ = planeProjection.GlobalOffsetZ
                };
        }

        public static double GetApothem(double height, double sides)
        {
            double x0 = 360.0/(2*sides);
            double k = Math.Sin(Math.PI*x0/180.0);
            double r = height/2/k;
            return -Math.Sqrt((r*r) - Math.Pow(height/2, 2));
        }

        public static bool HasChildrenAndAreVisible(Panel panelElement)
        {
            return panelElement.Children.Count >= 1 &&
                   panelElement.Children.Any(child => child.Visibility == Visibility.Visible);
        }

        public static double RandomRange(double low, double high)
        {
            return (low + ((high - low)*Rnd.NextDouble()));
        }

        public static void SendToConsole(string message)
        {
            HtmlPage.Window.Eval(string.Format("window.console.log(\"{0}\");", message));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                var b = new byte[1];
                do provider.GetBytes(b); while (!(b[0] < n*(Byte.MaxValue/n)));
                int k = (b[0]%n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}