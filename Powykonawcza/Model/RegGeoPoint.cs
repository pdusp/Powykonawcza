using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.Model
{
    public struct RegGeoPoint
    {
        public string pkt { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal h { get; set; }
        public string kod { get; set; }
        public DateTime? data { get; set; }
        public string mn { get; set; }
        public string mh { get; set; }
        public string mp { get; set; }
        public int? e { get; set; }
        public int? sat { get; set; }
        public decimal? pdop { get; set; }
        public decimal? wys_tyczki { get; set; }
        public string typ { get; set; }
        public string warning { get; set; }
    }

}
