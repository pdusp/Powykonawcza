using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powykonawcza.Model
{
    public struct GeoPoint
    {
        public int id { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
        public string type { get; set; }
        public string warning { get; set; }

    }
}
