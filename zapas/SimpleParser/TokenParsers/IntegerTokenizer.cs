using System.Globalization;
using System.Text.RegularExpressions;

namespace SimpleParser.TokenParsers
{
    public class IntegerTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 90;
        }

        protected override Regex GetRegex()
        {
            return IntegerRegex;
        }

        protected override object ParseValue(Match m)
        {
            return long.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
        }

        private static readonly Regex IntegerRegex =
            new Regex(IntegerFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string IntegerFilter = @"^\s*([+-]?\d+)";
    }
}