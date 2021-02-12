using System.Text.RegularExpressions;

namespace SimpleParser.TokenParsers
{
    public class SingleQuoteStringTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 0;
        }

        protected override Regex GetRegex()
        {
            return SingleQuoteWrappedRegex;
        }

        protected override object ParseValue(Match m)
        {
            return m.Groups[1].Value;
        }

        private static readonly Regex SingleQuoteWrappedRegex =
            new Regex(SingleQuoteWrappedFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string SingleQuoteWrappedFilter = @"^\s*('(?:(?:[^'\\])|(?:\\')|(?:\\\\))*')";
    }
}