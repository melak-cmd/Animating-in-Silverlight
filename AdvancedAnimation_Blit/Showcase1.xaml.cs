using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AdvancedAnimation_Blit
{
    public partial class Showcase1
    {
        private Plotter _plotter;
        private WriteableBitmap _wb;

        public Showcase1()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            MouseMove += MainPage_MouseMove;
            MouseLeftButtonDown += MainPage_MouseLeftButtonDown;
            MouseLeftButtonUp += MainPage_MouseLeftButtonUp;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _plotter = new Plotter();
            _wb = new WriteableBitmap(460, 400);

            _plotter.TargetBmp = _wb;
            outImg.Source = _wb;
        }

        private void MainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _plotter.SetMouseClick(true);
        }

        private void MainPage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _plotter.SetMouseClick(false);
        }

        private void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            _plotter.SetMouse(e.GetPosition(outImg));
        }

        #region Events

        #endregion Events
    }
}