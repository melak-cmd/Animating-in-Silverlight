using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AdvancedAnimation_Blit
{
    public static class Util
    {
        public static WriteableBitmap LoadBitmap(string path)
        {
            var img = new BitmapImage {CreateOptions = BitmapCreateOptions.None};
            Stream s = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
            img.SetSource(s);
            return new WriteableBitmap(img);
        }
    }
}