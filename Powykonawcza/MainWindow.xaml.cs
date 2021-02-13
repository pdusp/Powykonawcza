using System;
using System.Windows;
using System.Windows.Input;
using Powykonawcza.DAL;
using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;
using Microsoft.Win32;
using System.Reflection;

/*
Do przeniesienia:
mn - typ poprawek sieciowych i typ rozwiązania układu równań (RTK - już nieużywany dawny sposób nakładania poprawek) RTN - real time network i po myślniku Fix (dokoładny typ rozwiązania - fixed), Float (mniej dokładny), Auto - autonomiczny, bez uwzględnienia poprawek sieciowych
Mh - średni błąd punktu wysokościowo (H)
Mp - średni błąd punktu sytuacyjnie (X i Y)
e - ilość epok pomiaru, czyli jak wiele razy odbiornik wyznaczał pozycję zanim ją uśrednił i zapisał
sat - ilość satelit, na podstawie obserwacji których wyznaczono współrzędne danego punktu
PDOP - układ geometryczny satelit, których użyto do pomiaru - im wyższa wartość tym gorszy układ.
Wys. tyczki - wiadomo, ale zdarza się też używać pozycji wysokość anteny, a to trochę co innego 
*/


namespace Powykonawcza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<RegGeoPoint> lg;
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
            lg = new List<RegGeoPoint>();
            dg1.ItemsSource = null;
        }



        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            //GetSzablon
            btnsave.IsEnabled = false;
            lg.Clear();
            dg1.ItemsSource = null;
            //dg1
            //
            SzablonyImportu sz = new SzablonyImportu();
            sz.ShowDialog();
            ;
            List<SzablonItem> szablonItems;
            try
            {
                szablonItems = JsonUtils.LoadJsonFile<List<SzablonItem>>(@"SzablonImportu.dat");
                szablonItems = szablonItems.Where(p => p.import == true).ToList();
            }
            catch
            {
                return;
            }
            ;
            int expectedColumns = szablonItems.Count();
            if (expectedColumns < 2)
            {
                MessageBox.Show("Selected Template is empty");
                return;
            }

            //RawDate
            //StringCollection lines = Import.GetLinesCollectionFromTextBox(richTextBox1);
            //StringCollection lines = 
            TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            List<string> rtbLines = textRange.Text.Split('\n').ToList();
            //czyszczenie z pustych linii 
            rtbLines = rtbLines.Where(w => w.Length > 4).ToList();
            //
            //wstępna weryfikacja pliku
            int lineNo = 0;
            foreach (string txtline in rtbLines)
            {
                lineNo++;
                if (txtline.Length < 10)
                {
                    MessageBox.Show($"LineNo: {lineNo} ->  {txtline } is not correct, too short. Import break!");
                    return;
                }
            }


            foreach (string txtline in rtbLines)
            {
                string txt = txtline.Replace("\t", " ").Replace("\r", "");
                var objects = new Tokenizer().Parse(txt);
                if (objects.Tokens.Length != expectedColumns)
                {
                    MessageBox.Show($"Line no: {objects.Tokens[0] } is not correct. Import break!");
                    return;
                }
            }
            //
            //
           
            foreach (string txtline in rtbLines)
            {
                RegGeoPoint point = new RegGeoPoint();
                var objects = new Tokenizer().Parse(txtline);//objects.Tokens
                for (var i = 0; i < objects.Tokens.Length; i++)
                {
                    PropertyInfo prop = point.GetType().GetProperty(szablonItems[i].nazwa.ToString(), BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite && (szablonItems[i].type.ToString() == "string"))
                    {
                        prop.SetValue(point, objects.Tokens[i].ToString(), null);
                    }
                    if (null != prop && prop.CanWrite && (szablonItems[i].type.ToString() == "date"))
                    {
                        prop.SetValue(point, objects.Tokens[i].ToString(), null);
                    }
                    if (null != prop && prop.CanWrite && (szablonItems[i].type.ToString() == "numeric"))
                    {
                        prop.SetValue(point, decimal.Parse(objects.Tokens[i].ToString()), null);
                    }
                    if (null != prop && prop.CanWrite && (szablonItems[i].type.ToString() == "integer"))
                    {
                        prop.SetValue(point, Int32.Parse(objects.Tokens[i].ToString()), null);
                    }
                }
                lg.Add(point);
            }



            //lg.Add(new GeoPoint { id = 2, x = 5655.34M, y = 66500.12M, type = "SUPC_01", warning = "" });
            //lg.Add(new GeoPoint { id = 3, x = 5755.34M, y = 67500.12M, type = "SUPC_01", warning = "" });

            //dg1.ItemsSource = lg;

            if (lg.Count>0)
            {
                btnsave.IsEnabled = true;
            }
            dg1.ItemsSource = lg;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //DrawiGeo dr = new DrawiGeo(lg);
            //dr.ShowDialog();

            if (lg.Count<1)
            {
                MessageBox.Show("List of Geopoint is empty");
                return;
            }
            try
            {
                JsonUtils.SaveJson(@"Geopoints.dat", lg);
            }
            catch
            {
                MessageBox.Show("Error: Save Problem!");
            }
            MessageBox.Show("Save Ok");
        }


        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_ClickOpen(object sender, RoutedEventArgs e)
        {
            ButtonImport.IsEnabled = false;
            lg.Clear();
            btnsave.IsEnabled = false;
            //
            richTextBox1.Document.Blocks.Clear();
            //
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|(*.rtf)|*.rtf|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string txt = File.ReadAllText(openFileDialog.FileName);
                string ext = System.IO.Path.GetExtension(openFileDialog.FileName);

                MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(txt));

                if (ext.ToLower() == ".rtf")
                {
                    richTextBox1.Selection.Load(stream, DataFormats.Rtf);
                }

                if (ext.ToLower() == ".txt")
                {
                    richTextBox1.Selection.Load(stream, DataFormats.Text);
                }

                ButtonImport.IsEnabled = true;

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



/*
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
                //int val;
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
 */