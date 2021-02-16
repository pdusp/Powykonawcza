using JetBrains.Annotations;

namespace Powykonawcza.Model
{
    public class RegGeoPoint
    {
        [NotNull]
        public string Point { get; set; }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal H { get; set; }

        [CanBeNull]
        public string Code { get; set; }

        [CanBeNull]
        public string Date { get; set; }

        [CanBeNull]
        public string Mn { get; set; }

        [CanBeNull]
        public string Mh { get; set; }

        [CanBeNull]
        public string Mp { get; set; }

        [CanBeNull]
        public int? E { get; set; }

        [CanBeNull]
        public int? Sat { get; set; }

        [CanBeNull]
        public decimal? Pdop { get; set; }

        [CanBeNull]
        public decimal? PoleHeight { get; set; }

        [CanBeNull]
        public string Type { get; set; }

        [CanBeNull]
        public string Warning { get; set; }
    }
}