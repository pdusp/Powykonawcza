using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
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
using Microsoft.Win32;
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
            //RawDate
            //StringCollection lines = Import.GetLinesCollectionFromTextBox(richTextBox1);
            //StringCollection lines = 
            TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            string[] rtbLines = textRange.Text.Split('\n');
            //
            foreach (var line in rtbLines)
            {
                Console.WriteLine(line);
            }
            //
            lg = new List<GeoPoint>();
            foreach (string line in rtbLines)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }
                //regex example: @"^\s*(\d+)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)(\s+.*)?$";
                const string FilterFilter = @"^\s*(\d+)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)\s+(\d+(?:[\.,]\d+)?)(\s+.*)?$";
                Regex f = new Regex(FilterFilter, RegexOptions.Multiline | RegexOptions.Compiled);
                string pkt = "";
                decimal x = 0, y = 0, h = 0;
                string typ = "", warning = "";
                //id = Int32.Parse(f[1]);
                // 
                string[] result = Regex.Split(line, FilterFilter, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
                int val;
                decimal vald;
                bool success = true;

                if (result.Length > 5)
                {
                    //success = Int32.TryParse(result[1], out val);
                    if (result[1].Length < 1)
                    { warning += "Błąd id"; }
                    else
                    {
                        pkt = result[1];
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
                    { warning += "Błąd h"; }
                    else
                    {
                        h = vald;
                    }
                    //
                    typ = result[5];
                    //
                }
                else
                {
                    warning = "Błędne dane";
                }

                GeoPoint p = new GeoPoint { pkt = pkt, x = x, y = y, h = h, typ = typ, warning = warning };

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

        private void MenuItem_ClickOpen(object sender, RoutedEventArgs e)
        {
            richTextBox1.Document.Blocks.Clear();
            MessageBox.Show("Please open file");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|(*.rtf)|*.rtf|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                //TextRange range = new TextRange(richTextBox1.FileName, richTextBox1.ContentEnd);
                // FileStream fStream = new FileStream(openFileDialog, FileMode.Open, FileAccess.Read, FileShare.Read);
                //range.Load(fStream, DataFormats.Rtf);
                //fStream.Close();

                string txt = File.ReadAllText(openFileDialog.FileName);
                string ext = System.IO.Path.GetExtension(openFileDialog.FileName);

                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(txt));

                if (ext.ToLower() == ".rtf")
                {
                    richTextBox1.Selection.Load(stream, DataFormats.Rtf);
                }

                if (ext.ToLower() == ".txt")
                {
                    richTextBox1.Selection.Load(stream, DataFormats.Text);
                }

            }

        }



        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.O)) // This will capture the Ctrl+O shortcut.
            {
                MenuItem_ClickOpen(null, null);
            }
        }

        private void MenuSzablonyImportu(object sender, RoutedEventArgs e)
        {
            SzablonyImportu sz = new SzablonyImportu();
            sz.ShowDialog();
        }
    }
}
