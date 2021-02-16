using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using Powykonawcza;
using Powykonawcza.DAL;
using Powykonawcza.Model.Szablon;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        public static string GetResourceByName(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var reader   = new StreamReader(assembly.GetManifestResourceStream(resource));
            return reader.ReadToEnd();
        }


        [Fact]
        public void ImportTXT()
        {
            List<SzablonItem> szablonItems;
            try
            {
                szablonItems =
                    JsonUtils.LoadJsonFile<List<SzablonItem>>(@".\PrzykladyTXT\Szablony\SzablonImportuTXT1.dat");
                //if (szablonItems is null)
                //  MessageBox.Show("brak pliku SzablonImportu.dat");

                szablonItems = szablonItems.Where(p => p.import).ToList();
            }
            catch
            {
                //Exception ee
                //MessageBox.Show(ee.Message);
                return;
            }

            ;
            var expectedColumns = szablonItems.Count();
            if (expectedColumns < 4)
                //MessageBox.Show("Selected Template is empty");
                return;

            var richTextBox1 = new RichTextBox();

            //trzeba załadowac do RichTextBox

            //richTextBox1.Selection.Load(stream, DataFormats.Rtf); 

            var textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            var rtbLines  = textRange.Text.Split('\n').ToList();
            //czyszczenie z ewidentnie pustych linii 
            rtbLines = rtbLines.Where(w => w.Length > 4).ToList();

            var lineNo = 0;
            foreach (var txtline in rtbLines)
            {
                lineNo++;
                if (txtline.Length < 10)
                {
                    //MessageBox.Show($"LineNo: {lineNo} ->  {txtline } is not correct, too short. Import break!");
                }
            }

            foreach (var txtline in rtbLines)
            {
                var txt     = txtline.Replace("\t", " ").Replace("\r", "");
                var objects = new Tokenizer().Parse(txt);
                if (objects.Tokens.Length != expectedColumns)
                {
                    // MessageBox.Show($"Line no: {txt} is not correct. Import break!");
                }
            }
        }
    }
}