using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;

namespace Powykonawcza.Services
{
    public class GeoDataImportService : IGeoDataImportService
    {
        private static IList<RegGeoPoint> ImportInternal(IReadOnlyList<SzablonItem> szablonItems, string text)
        {
            var expectedColumns = szablonItems.Count;
            //

            /*try
            {
                szablonItems = JsonUtils.LoadJsonFile<List<SzablonItem>>(@"SzablonImportu.dat");
                if (szablonItems is null)
                {
                    MessageBox.Show("brak pliku SzablonImportu.dat");
                    return;
                }

                szablonItems = szablonItems.Where(p => p.import == true).ToList();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }*/
            ;

            /*TextRange textRange = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
            List<string> rtbLines = textRange.Text.Split('\n').ToList();*/
            var rtbLines = text.Split('\r', '\n')
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToArray();

            /*
            for (var index = 0; index < rtbLines.Length; index++)
            {
                string txtline = rtbLines[index];
                /*if (txtline.Length < 10)
                {
                    MessageBox.Show($"LineNo: {lineNo} ->  {txtline} is not correct, too short. Import break!");
                }#1#
            }
            */

            for (var index = 0; index < rtbLines.Length; index++)
            {
                var txtline = rtbLines[index];
                // todo: sprawdzić czy parser ogarnia tabulatory
                var txt     = txtline.Replace('\t', ' ');
                if (string.IsNullOrWhiteSpace(txtline))
                    continue;
                var objects = new Tokenizer().Parse(txt);
                if (objects.Tokens.Length != expectedColumns)
                    throw new Exception($"Line no: {txt} is not correct. Import break!");
            }

            var result = new List<RegGeoPoint>();

            // !!!!!! GetProperty jest kosztowne - trzeba to pobrać raz 
            var props = new PropertyInfo[expectedColumns];
            for (var columnIdx = 0; columnIdx < expectedColumns; columnIdx++)
            {
                var templateItem = szablonItems[columnIdx];
                props[columnIdx] =
                    typeof(RegGeoPoint).GetProperty(templateItem.nazwa, BindingFlags.Public | BindingFlags.Instance);
            }

            foreach (var txtline in rtbLines)
            {
                var point   = new RegGeoPoint();
                var objects = new Tokenizer().Parse(txtline);
                for (var columnIdx = 0; columnIdx < objects.Tokens.Length; columnIdx++)
                {
                    var templateItem = szablonItems[columnIdx];
                    var prop         = props[columnIdx];
                    var propCanWrite = null != prop && prop.CanWrite;
                    if (!propCanWrite)
                        continue;
                    var tokenValue = objects.Tokens[columnIdx];
                    switch (templateItem.type)
                    {
                        case "string":
                        case "date":
                            prop.SetValue(point, tokenValue.ToString(), null);
                            break;
                        case "numeric":
                        {
                            var dv = decimal.Parse(tokenValue.ToString().Replace(',', '.'));
                            prop.SetValue(point, dv, null);
                            break;
                        }
                        case "integer":
                            prop.SetValue(point, int.Parse(tokenValue.ToString()), null);
                            break;
                    }
                }

                result.Add(point);
            }

            return result;
        }

        public Task<IList<RegGeoPoint>> Import([NotNull] IReadOnlyList<SzablonItem> szablonItems,
            [NotNull] string text)
        {
            if (szablonItems == null) throw new ArgumentNullException(nameof(szablonItems));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (szablonItems.Count < 2)
                throw new Exception("Empty import template");
            return Task.Run(() => ImportInternal(szablonItems, text));
        }
    }
}