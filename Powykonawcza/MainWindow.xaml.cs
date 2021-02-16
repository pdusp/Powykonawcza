using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.Win32;
using Powykonawcza.DAL;
using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;
using Powykonawcza.Services;


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
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public MainWindow()
        {
            InitializeComponent();
            _progressService = new WpfProgressbarProgressService(MyProgressbar);
            //
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            var ci = new CultureInfo(currentCulture)
            {
                NumberFormat   = {NumberDecimalSeparator = "."},
                DateTimeFormat = {DateSeparator          = "/"}
            };
            Thread.CurrentThread.CurrentCulture   = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            //
            lg              = new List<RegGeoPoint>();
            dg1.ItemsSource = null;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (lg.Count < 1)
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


        private async void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            //GetSzablon
            btnsave.IsEnabled = false;
            lg.Clear();
            //
            dg1.ItemsSource = null;
            dg1.Items.Clear();
            dg1.Items.Refresh();
            MessageBox.Show("Proszę czekać...");

            try
            {
                var szablonItems = JsonUtils.LoadJsonFile<List<SzablonItem>>(@"SzablonImportu.dat");
                if (szablonItems is null)
                {
                    MessageBox.Show("brak pliku SzablonImportu.dat");
                    return;
                }

                szablonItems = szablonItems.Where(p => p.import).ToList();
                var textRange     = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                var importService = new GeoDataImportService(_progressService);
                var points        = await importService.Import(szablonItems, textRange.Text);

                dg1.ItemsSource = points;
                MessageBox.Show("Zadanie wykonane");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        private void MenuItem_ClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_ClickOpen(object sender, RoutedEventArgs e)
        {
            mn_Import.IsEnabled = false;
            lg.Clear();
            btnsave.IsEnabled = false;
            //
            richTextBox1.Document.Blocks.Clear();
            dg1.Items.Refresh();
            //
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect      = true;
            openFileDialog.Filter           = "Text files (*.txt)|*.txt|(*.rtf)|*.rtf|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                var txt = File.ReadAllText(openFileDialog.FileName);
                var ext = Path.GetExtension(openFileDialog.FileName);

                var stream = new MemoryStream(Encoding.Default.GetBytes(txt));

                if (ext.ToLower() == ".rtf") richTextBox1.Selection.Load(stream, DataFormats.Rtf);

                if (ext.ToLower() == ".txt") richTextBox1.Selection.Load(stream, DataFormats.Text);

                mn_Import.IsEnabled = true;
            }
        }

        private void MenuSzablonyImportu(object sender, RoutedEventArgs e)
        {
            var sz = new SzablonyImportu();
            sz.ShowDialog();
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.O)
            ) // This will capture the Ctrl+O shortcut.
                MenuItem_ClickOpen(null, null);
        }

        public readonly List<RegGeoPoint> lg;
        private readonly IProgressService _progressService;
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