using System.Text.RegularExpressions;

namespace SimpleParser.TokenParsers
{
    public abstract class AbstractRegexpTokenizer : ValueTokenizer
    {
        public override TokenCandidate Parse(string text)
        {
            var regex = GetRegex();
            var m     = regex.Match(text);
            if (!m.Success)
                return null;
            var q = ParseValue(m);
            if (q is null)
                return null;
            return new TokenCandidate(q, m.Index + m.Length, GetPriority());
        }

        protected abstract int GetPriority();

        protected abstract Regex GetRegex();

        protected abstract object ParseValue(Match m);
    }
}