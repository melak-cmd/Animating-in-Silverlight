using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Artefact.Animation;

namespace AdvancedAnimation_Blit
{
    public partial class RoundedSquare
    {
        public RoundedSquare()
        {
            // Required to initialize variables
            InitializeComponent();
            Loaded += _Loaded;

            // prep the LayoutRoot for RenderTransform animations
            LayoutRoot.NormalizeTransformGroup();
            LayoutRoot.RenderTransformOrigin = new Point(.5, .5);
        }

        public void Flash()
        {
            ArtefactAnimator.AddEase(LayoutRoot,
                new[] {Border.BorderBrushProperty, Border.BorderThicknessProperty},
                new object[] {new SolidColorBrush(Colors.White), new Thickness(3)}, .3,
                AnimationTransitions.CubicEaseOut).OnComplete((eo, p) =>
                    ArtefactAnimator.AddEase(LayoutRoot,
                        new[] {Border.BorderBrushProperty, Border.BorderThicknessProperty},
                        new[] {Application.Current.Resources["AquaSolidBrush"], new Thickness(2)}, .3,
                        AnimationTransitions.CubicEaseIn));
        }

        #region INITIALIZATION

        private void _Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= _Loaded;

            MouseEnter += _MouseEnter;
            MouseLeave += _MouseLeave;
            MouseLeftButtonUp += _MouseLeftButtonUp;
            Cursor = Cursors.Hand;
        }

        #endregion INITIALIZATION

        #region PROPS

        public ScaleTransform Scale
        {
            get
            {
                return (ScaleTransform)
                    ((TransformGroup) RenderTransform).Children[(int) TransformGroupIndexes.ScaleTransform];
            }
        }

        public double ScaleX
        {
            get { return Scale.ScaleX; }
        }

        public double ScaleY
        {
            get { return Scale.ScaleY; }
        }

        #endregion PROPS

        #region INTERACTION

        private void _MouseEnter(object sender, MouseEventArgs e)
        {
            ArtefactAnimator.AddEase(LayoutRoot, Border.CornerRadiusProperty, new CornerRadius(0), .5,
                AnimationTransitions.CubicEaseOut);
            ArtefactAnimator.AddEase(LayoutRoot,
                new[] {Border.BorderBrushProperty, Border.BackgroundProperty, Border.BorderThicknessProperty},
                new[]
                {
                    new SolidColorBrush(Colors.White), Application.Current.Resources["OrangeGradientBrush"],
                    new Thickness(5)
                },
                1, AnimationTransitions.CubicEaseOut);
        }

        private void _MouseLeave(object sender, MouseEventArgs e)
        {
            ArtefactAnimator.AddEase(LayoutRoot, Border.CornerRadiusProperty, new CornerRadius(Width/2), .5,
                AnimationTransitions.CubicEaseOut);
            ArtefactAnimator.AddEase(LayoutRoot,
                new[] {Border.BorderBrushProperty, Border.BackgroundProperty, Border.BorderThicknessProperty},
                new[]
                {
                    Application.Current.Resources["AquaSolidBrush"], Application.Current.Resources["BlueGradientBrush"],
                    new Thickness(2)
                },
                1, AnimationTransitions.CubicEaseOut);
        }

        private void _MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LayoutRoot.RotateTo(360, 1, AnimationTransitions.ElasticEaseOut, 0)
                .OnComplete((eo, p) => ((UIElement) eo.Data).RotateTo(0))
                .Data = LayoutRoot;
        }

        #endregion INTERACTION

        #region POSITION

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof (Point), typeof (RoundedSquare),
                new PropertyMetadata(new Point(0, 0), OnPositionChanged));

        /// <summary>
        ///     Example of using a Custom Dependency Property to animate position - you could just have used SlideTo().
        /// </summary>
        public Point Position
        {
            get { return (Point) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement) d;
            var pt = (Point) e.NewValue;
            Canvas.SetLeft(element, pt.X);
            Canvas.SetTop(element, pt.Y);
        }

        #endregion POSITION
    }
}