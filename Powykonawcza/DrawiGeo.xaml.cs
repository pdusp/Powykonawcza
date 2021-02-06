using Powykonawcza.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Powykonawcza
{
    /// <summary>
    /// Interaction logic for DrawiGeo.xaml
    /// </summary>
    public partial class DrawiGeo : Window
    {
        private List<GeoPoint> lg;
        public DrawiGeo(List<GeoPoint> _l)
        {
            InitializeComponent();
            lg = _l;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int ofx = int.Parse(offx.Text);
            int ofy = int.Parse(offy.Text);


            foreach (GeoPoint p in lg)
            {
                var ellipse = new Ellipse() { Width = 1.5, Height = 1.5, Stroke = new SolidColorBrush(Colors.LightBlue) };

                double x =  (double)p.x - ofx;
                double y =  (double)p.y - ofy;
                DrawPoint(x,y);
                  
            }

           

        }

        private void DrawPoint(double x, double y)
        {
            int dotSize = 10;

            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.Green);
            currentDot.StrokeThickness = 3;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.Green);
            currentDot.Margin = new Thickness(x*100, y*100, 0, 0); // Sets the position.
            canvas.Children.Add(currentDot);
        }
    }
}
