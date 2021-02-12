using System.Text.RegularExpressions;

namespace SimpleParser.TokenParsers
{
    public class StringNoSpaceTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 0;
        }

        protected override Regex GetRegex()
        {
            return NoSpaceTextRegex;
        }

        protected override object ParseValue(Match m)
        {
            return m.Groups[1].Value;
        }

        private static readonly Regex NoSpaceTextRegex =
            new Regex(NoSpaceTextFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);


        private const string NoSpaceTextFilter = @"^\s*([^\s]+)";
    }
}