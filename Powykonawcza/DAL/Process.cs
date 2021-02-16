using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.DAL
{
    public static class Process
    {
        public static List<RegGeoPoint> ProcessFile(List<SzablonItem> szablonItems, List<string> rtbLines)
        {
            List<RegGeoPoint> _lg = new List<RegGeoPoint>();
            //
            //wstępna weryfikacja pliku
            var start = DateTime.Now;
            //
            Debug.WriteLine("Etap 1 {0} sek", (DateTime.Now - start).TotalSeconds);
            //
            start = DateTime.Now;
            //
            Debug.WriteLine("Etap 2 {0} sek", (DateTime.Now - start).TotalSeconds);
            //
            start = DateTime.Now;
            //
            foreach (string txtline in rtbLines)
            {
                var point = new RegGeoPoint();
                var objects = new Tokenizer().Parse(txtline);
                for (var i = 0; i < objects.Tokens.Length; i++)
                {
                    var prop = point.GetType().GetProperty(szablonItems[i].nazwa, BindingFlags.Public | BindingFlags.Instance);
                    var propCanWrite = null != prop && prop.CanWrite;
                    if (!propCanWrite)
                        continue;
                    var tokenValue = objects.Tokens[i];
                    switch (szablonItems[i].type)
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
                            prop.SetValue(point, Int32.Parse(tokenValue.ToString()), null);
                            break;
                    }
                }
                _lg.Add(point);
            }
            Debug.WriteLine("Etap 3 {0} sek", (DateTime.Now - start).TotalSeconds);
            return _lg;
        }
    }
}
