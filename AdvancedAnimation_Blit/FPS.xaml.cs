using System;
using System.Globalization;
using System.Windows.Media;

namespace AdvancedAnimation_Blit
{
    public partial class Fps
    {
        private const int Digits = 6;
        private const String Prefix = "FPS: ";
        private const int SampleSize = 20;
        private const double Springness = 0.2;
        private int _counter;
        private TimeSpan _date;
        private double _displayFps;
        private double _fps;

        public Fps()
        {
            InitializeComponent();

            // set the date
            _date = DateTime.Now.TimeOfDay;

            // start the enter frame event
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        // FPS
        // on enter frame simulator
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            // calculate the FPS on SAMPLE_SIZE instance
            if (_counter++ == SampleSize)
            {
                TimeSpan now = DateTime.Now.TimeOfDay;

                // get the time difference
                TimeSpan diff = now - _date;

                _fps = _counter/(diff.TotalMilliseconds/1000.0);
                _counter = 0;
                _date = now;
            }

            // Display the FPS
            _displayFps += (_fps - _displayFps)*Springness;
            int digits = Math.Min(_displayFps.ToString(CultureInfo.InvariantCulture).Length, Digits);
            fpsTxt.Text = Prefix + _displayFps.ToString(CultureInfo.InvariantCulture).Substring(0, digits);
        }

        // Springness
    }
}