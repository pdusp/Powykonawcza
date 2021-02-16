using System.Collections.Generic;
using System.Threading.Tasks;
using Powykonawcza.Model;
using Powykonawcza.Model.Szablon;

namespace Powykonawcza.Services
{
    public interface IGeoDataImportService
    {
        Task<IList<RegGeoPoint>> Import(IReadOnlyList<SzablonItem> tmpItems, string text);
    }
}