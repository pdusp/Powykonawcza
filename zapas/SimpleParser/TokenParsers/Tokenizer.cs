using System.Collections.Generic;
using System.Linq;

namespace SimpleParser.TokenParsers
{
    public abstract class AbstractTokenizer
    {
        public TokenizerResult Parse(string txt)
        {
            var l             = new List<object>();
            var spacerequired = false;
            var txtSpan       = TextSpan.FromString(txt);
            while (txtSpan.Length > 0)
            {
                if (spacerequired)
                    if (!char.IsWhiteSpace(txtSpan[0]))
                        break;
                var item = ParseSingleToken(ref txtSpan);
                if (item is null)
                    break;

                l.Add(item);
                spacerequired = true;
            }

            return new TokenizerResult
            {
                Tokens       = l.ToArray(),
                NotParsedEnd = txtSpan
            };
        }

        protected abstract ValueTokenizer[] GetTokenizers();

        private object ParseSingleToken(ref TextSpan txt)
        {
            var candidates = GetTokenizers();
            var tt         = txt.ToString();
            var c = candidates
                .Select(a => { return a.Parse(tt); })
                .Where(a => a != null)
                .OrderByDescending(a => a.Priority)
                .ThenByDescending(a => a.StringLength)
                .ToArray();
            if (c.Length == 0)
                return null;
            var winner = c[0];
            txt = txt.Substring(winner.StringLength);
            return winner.Token;
        }
    }
}