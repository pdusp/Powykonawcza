using JetBrains.Annotations;

namespace Powykonawcza.Model
{
    public class RegGeoPoint
    {
        [NotNull]
        public string point { get; set; }

        [NotNull]
        public decimal x { get; set; }

        [NotNull]
        public decimal y { get; set; }

        [NotNull]
        public decimal h { get; set; }

        [CanBeNull]
        public string code { get; set; }

        [CanBeNull]
        public string date { get; set; }

        [CanBeNull]
        public string mn { get; set; }

        [CanBeNull]
        public string mh { get; set; }

        [CanBeNull]
        public string mp { get; set; }

        [CanBeNull]
        public int? e { get; set; }

        [CanBeNull]
        public int? sat { get; set; }

        [CanBeNull]
        public decimal? pdop { get; set; }

        [CanBeNull]
        public decimal? heightPole { get; set; }

        [CanBeNull]
        public string type { get; set; }

        [CanBeNull]
        public string warning { get; set; }
    }
}