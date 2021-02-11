using Newtonsoft.Json;
using Powykonawcza.Model.Szablon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.DAL
{
    public class JsonUtils
    {

        public static T LoadJsonFile<T>(string filename) where T : class
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            if (!File.Exists(filename)) return null;
            var json = Encoding.UTF8.GetString(File.ReadAllBytes(filename));
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        public static void SaveJson(string filename, object data)
        {
            var fi = new FileInfo(filename);
            fi.Directory?.Create();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllBytes(fi.FullName, Encoding.UTF8.GetBytes(json));
        }

        
    }
}
