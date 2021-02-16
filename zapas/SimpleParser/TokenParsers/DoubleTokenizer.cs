using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleParser.TokenParsers
{
    public class DoubleTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 100; // najwyższy
        }

        protected override Regex GetRegex()
        {
            return DoubleRegex;
        }

        protected override object ParseValue(Match m)
        {
            return double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
        }

        private static readonly Regex DoubleRegex =
            new Regex(DoubleFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string DoubleFilter = @"^\s*([+-]?\d+(?:(?:(?:\.\d+)(?:e[+-]?\d+)?)|(?:(?:e[+-]?\d+))))";
    }
}
/*
 
 ^\s*
(
[+-]?
\d+
(?:
(?:(?:\.\d+)(?:e[+-]?\d+)?)
|
(?:(?:e[+-]?\d+))
)

)

 */