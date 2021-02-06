using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Powykonawcza.DAL;
using Powykonawcza.Model;

namespace Powykonawcza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<GeoPoint> lg;
        public MainWindow()
        {
            InitializeComponent();
            //
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            var ci = new CultureInfo(currentCulture)
            {
                NumberFormat = { NumberDecimalSeparator = "," },
                DateTimeFormat = { DateSeparator = "/" }
            };
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            //
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
       
             
            StringCollection lines = Import.GetLinesCollectionFromTextBox(RawDate);
            lg = new List<GeoPoint>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                const string FilterFilter = @"^\s*(\d+)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)(\s+.*)?$";
                Regex f = new Regex(FilterFilter, RegexOptions.Multiline | RegexOptions.Compiled);
                int id=0;
                decimal x=0, y=0, z=0;
                string type="", warning="";
                //id = Int32.Parse(f[1]);

               


                string[] result = Regex.Split(line, FilterFilter,
                                   RegexOptions.IgnoreCase,
                                   TimeSpan.FromMilliseconds(500));
                int val;
                decimal vald;
                bool success = true;

                if (result.Length > 5)
                {
                    success = Int32.TryParse(result[1], out val);
                    if (!success)
                    { warning += "Błąd id"; }
                    else
                    {
                        id = val;
                    }
                    //
                    success = Decimal.TryParse(result[2].Replace('.', ','), out vald);
                    if (!success)
                    { warning += "Błąd x"; }
                    else
                    {
                        x = vald;
                    }
                    //
                    success = Decimal.TryParse(result[3].Replace('.', ','), out vald);
                    if (!success)
                    { warning += "Błąd y"; }
                    else
                    {
                        y = vald;
                    }
                    //
                    success = Decimal.TryParse(result[4].Replace('.', ','), out vald);
                    if (!success)
                    { warning += "Błąd z"; }
                    else
                    {
                        z = vald;
                    }
                    //
                    type = result[5];
                    //
                }
                else
                {
                    warning = "Błędne dane";
                }

                GeoPoint p = new GeoPoint { id = id, x = x, y = y, z=z, type = type, warning = warning };
                
                lg.Add(p);
            }


            
            //lg.Add(new GeoPoint { id = 2, x = 5655.34M, y = 66500.12M, type = "SUPC_01", warning = "" });
            //lg.Add(new GeoPoint { id = 3, x = 5755.34M, y = 67500.12M, type = "SUPC_01", warning = "" });

            dg1.ItemsSource = lg;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DrawiGeo dr = new DrawiGeo(lg);
            dr.ShowDialog();

        }
 
      
        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
