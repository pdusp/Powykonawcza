using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Powykonawcza.Model;

namespace Powykonawcza
{
    /// <summary>
    ///     Interaction logic for DrawiGeo.xaml
    /// </summary>
    public partial class DrawiGeo : Window
    {
        public DrawiGeo(List<RegGeoPoint> _l)
        {
            InitializeComponent();
            lg = _l;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofx = int.Parse(offx.Text);
            var ofy = int.Parse(offy.Text);

            foreach (var p in lg)
            {
                var ellipse = new Ellipse {Width = 1.5, Height = 1.5, Stroke = new SolidColorBrush(Colors.LightBlue)};

                var x = (double)p.x - ofx;
                var y = (double)p.y - ofy;
                DrawPoint(x, y);
            }
        }

        private void DrawPoint(double x, double y)
        {
            var dotSize = 10;

            var currentDot = new Ellipse();
            currentDot.Stroke          = new SolidColorBrush(Colors.Green);
            currentDot.StrokeThickness = 3;
            Panel.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width  = dotSize;
            currentDot.Fill   = new SolidColorBrush(Colors.Green);
            currentDot.Margin = new Thickness(x * 100, y * 100, 0, 0); // Sets the position.
            canvas.Children.Add(currentDot);
        }

        private readonly List<RegGeoPoint> lg;
    }
}