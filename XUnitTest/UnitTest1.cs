using Powykonawcza.Model.Szablon;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using Xunit;
using System.Linq;
using Powykonawcza.DAL;
using Powykonawcza.Model;
using Powykonawcza;
using System.Reflection;
using System.IO;

namespace XUnitTest
{
    public class UnitTest1
    {
        public static string GetResourceByName(string resource)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resource));
            return reader.ReadToEnd();
        }


        [Fact]
        public void ImportTXT()
        {
            List<SzablonItem> szablonItems;
            try
            {
                szablonItems = JsonUtils.LoadJsonFile<List<SzablonItem>>(@".\PrzykladyTXT\Szablony\SzablonImportuTXT1.dat");
                //if (szablonItems is null)
                //  MessageBox.Show("brak pliku SzablonImportu.dat");

                szablonItems = szablonItems.Where(p => p.import == true).ToList();
            }
            catch 
            {
                //Exception ee
                //MessageBox.Show(ee.Message);
                return;
            }
            ;
            int expectedColumns = szablonItems.Count();
            if (expectedColumns < 4)
            {
                //MessageBox.Show("Selected Template is empty");
                return;
            }

            RichTextBox richTextBox1 = new RichTextBox();
            
            //trzeba za³adowac do RichTextBox

            //richTextBox1.Selection.Load(stream, DataFormats.Rtf); 


            TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            List<string> rtbLines = textRange.Text.Split('\n').ToList();
            //czyszczenie z ewidentnie pustych linii 
            rtbLines = rtbLines.Where(w => w.Length > 4).ToList();

            int lineNo = 0;
            foreach (string txtline in rtbLines)
            {
                lineNo++;
                if (txtline.Length < 10)
                {
                    //MessageBox.Show($"LineNo: {lineNo} ->  {txtline } is not correct, too short. Import break!");
                }
            }

            foreach (string txtline in rtbLines)
            {
                string txt = txtline.Replace("\t", " ").Replace("\r", "");
                var objects = new Tokenizer().Parse(txt);
                if (objects.Tokens.Length != expectedColumns)
                {
                   // MessageBox.Show($"Line no: {txt} is not correct. Import break!");
                }
            }
 



        }
    }
}
