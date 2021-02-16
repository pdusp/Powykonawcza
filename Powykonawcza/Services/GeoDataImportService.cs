using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;

namespace Powykonawcza.Services
{
    public class GeoDataImportService : IGeoDataImportService
    {
        public GeoDataImportService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public Task<IList<RegGeoPoint>> Import([NotNull] IReadOnlyList<SzablonItem> tmpItems,
            [NotNull] string text)
        {
            if (tmpItems == null) throw new ArgumentNullException(nameof(tmpItems));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (tmpItems.Count < 2)
                throw new Exception("Empty import template");
            return Task.Run(() => ImportInternal(tmpItems, text));
        }

        private IList<RegGeoPoint> ImportInternal(IReadOnlyList<SzablonItem> tmpItems, string text)
        {
            int lineno = 0;
            lineno++;
            var expectedColumns = tmpItems.Count;
            var rtbLines = text.Split('\r', '\n')
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToArray();

            for (var index = 0; index < rtbLines.Length; index++)
            {
                var txtline = rtbLines[index];
                // todo: sprawdzić czy parser ogarnia tabulatory
                var txt = txtline.Replace('\t', ' ');
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
                var templateItem = tmpItems[columnIdx];
                props[columnIdx] =
                    typeof(RegGeoPoint).GetProperty(templateItem.name, BindingFlags.Public | BindingFlags.Instance);
            }

            for (var index = 0; index < rtbLines.Length; index++)
            {
                var txtline = rtbLines[index];
                _progressService.ReportProgress(index * 100.0 / rtbLines.Length);
                // todo tylko żeby pokazać progress
#if DEBUG
                Thread.Sleep(20);
#endif
                var point   = new RegGeoPoint();
                var objects = new Tokenizer().Parse(txtline);
                for (var columnIdx = 0; columnIdx < objects.Tokens.Length; columnIdx++)
                {
                    var templateItem = tmpItems[columnIdx];
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

        private readonly IProgressService _progressService;
    }
}