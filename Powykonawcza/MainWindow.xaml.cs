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
            Lg              = new List<RegGeoPoint>();
            dg1.ItemsSource = null;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Lg.Count < 1)
            {
                MessageBox.Show("List of Geopoint is empty");
                return;
            }

            try
            {
                JsonUtils.SaveJson(@"Geopoints.dat", Lg);
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
            Lg.Clear();
            //
            dg1.ItemsSource = null;
            dg1.Items.Clear();
            dg1.Items.Refresh();
            MessageBox.Show("Proszę czekać...");

            try
            {
                var tmpItems = JsonUtils.LoadJsonFile<List<SzablonItem>>(@"Template.dat");
                if (tmpItems is null)
                {
                    MessageBox.Show("brak pliku Template.dat");
                    return;
                }

                tmpItems = tmpItems.Where(p => p.Import).ToList();
                var textRange     = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                var importService = new GeoDataImportService(_progressService);
                var points        = await importService.Import(tmpItems, textRange.Text);

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
            Lg.Clear();
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
            // This will capture the Ctrl+O shortcut.
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.O)
            ) 
                MenuItem_ClickOpen(null, null);
        }

        public readonly List<RegGeoPoint> Lg;
        private readonly IProgressService _progressService;
    }
}

