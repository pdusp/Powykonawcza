using Powykonawcza.Model.Szablon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.DAL
{
    class TemplateOperation
    {
        public static void SaveTemplate(ObservableCollection<SzablonItem> gri)
        {
            JsonUtils.SaveJson(@"Template.dat", gri);
        }

    }
}
