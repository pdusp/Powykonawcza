using Powykonawcza.DAL;
using Powykonawcza.Model.Szablon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.Services
{
    public static class WPFtemplateService 
    {
        //: IWPFtemplateService
        //public WPFtemplateService(IWPFtemplateService wpftemplateService)
        //{
        //    _wpftemplateService = wpftemplateService;
        //}
        public static bool CheckRequiredBeforeSave(ObservableCollection<SzablonItem> gitm)
        {
            foreach (var itm in gitm)
                if ((itm.Name == "Point" || itm.Name == "X" || itm.Name == "Y" || itm.Name == "H") &&
                    itm.Import == false)
                {
                    return false;
                }
            return true;
        }

        public static ObservableCollection<SzablonItem> TryLoadTemplateFromFile()
        {
            var items = JsonUtils.LoadJsonFile<ObservableCollection<SzablonItem>>(@"Template.dat");
            if (items is null || items.Count == 0) throw new ArgumentException("Template is null");
            //GridItems = items;
            return items;
        }

        public static IReadOnlyList<SzablonItem> PopulateDefaultTemplate()
        {
            List<SzablonItem> l = new List<SzablonItem>();

            l.Add(new SzablonItem("Point", true, "numer Punktu", SzablonItem.GeoType.String));
            l.Add(new SzablonItem("X", true, "współrzędna X", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("Y", true, "współrzędna y", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("H", true, "wysokość h", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("Date", false, "data pomiaru", SzablonItem.GeoType.Date));
            l.Add(new SzablonItem("Code", false, "", SzablonItem.GeoType.String));
            l.Add(new SzablonItem("Mn", false, "", SzablonItem.GeoType.String));
            l.Add(new SzablonItem("Mh", false, "", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("Mp", false, "", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("E", false, "", SzablonItem.GeoType.Integer));
            l.Add(new SzablonItem("Sat", false, "", SzablonItem.GeoType.Integer));
            l.Add(new SzablonItem("Pdop", false, "", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("HeightPole", false, "", SzablonItem.GeoType.Numeric));
            l.Add(new SzablonItem("Type", false, "", SzablonItem.GeoType.String));
            return l;
        }

        public static ObservableCollection<SzablonItem> MoveItemUp(ObservableCollection<SzablonItem> GridItems, SzablonItem itm)
        {
            List<SzablonItem> szitm = new List<SzablonItem>();

            for (var i = 0; i < GridItems.Count; i++)
                if (GridItems[i].Name == itm.Name)
                {
                    if (i > 0)
                    {
                        var szp = GridItems[i - 1];
                        var szk = GridItems[i];
                        GridItems[i] = szp;
                        GridItems[i - 1] = szk;                      
                    }

                    break;
                }
            return GridItems;
        }

        public static ObservableCollection<SzablonItem> MoveItemDown(ObservableCollection<SzablonItem> GridItems, SzablonItem itm)
        {
            List<SzablonItem> szitm = new List<SzablonItem>();

            for (var i = 0; i < GridItems.Count; i++)
                if (GridItems[i].Name == itm.Name)
                {
                    if (i > 0)
                    {
                        var szp = GridItems[i + 1];
                        var szk = GridItems[i];
                        GridItems[i] = szp;
                        GridItems[i + 1] = szk;
                    }

                    break;
                }
            return GridItems;
        }

    }
}
