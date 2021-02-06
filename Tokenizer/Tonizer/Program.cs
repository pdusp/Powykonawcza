using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TokensDemo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var objects = Tokenizer.Parse("1  1.23gg 1e33  TEKST_BEZ SPACJI 'TESKST W APOSTROFACH' \t PO_TABULATORZE");

            foreach (var o in objects.Tokens)
            {
                 Console.WriteLine(o);
            }
            Console.ReadKey();

        }
    }


    public class Tokenizer
    {
        public static TokenizerResult Parse(string txt)
        {
            var l = new List<object>();
            var spacerequired = false;
            while (txt.Length > 0)
            {
                if (spacerequired)
                    if (!char.IsWhiteSpace(txt[0]))
                        break;
                var item = Parse1(ref txt);
                if (item is null)
                    break;

                l.Add(item);
                spacerequired = true;
            }

            return new TokenizerResult
            {
                Tokens = l.ToArray(),
                NotParsedEnd = txt
            };
        }

        private static ValueTokenizer[] GetTokenizers()
        {
            var candidates = new ValueTokenizer[]
            {
                new DoubleTokenizer(),
                new IntegerTokenizer(),
                new StringTokenizer()
            };
            return candidates;
        }

        private static object Parse1(ref string txt)
        {
            var candidates = GetTokenizers();
            var t = txt;
            var c = candidates
                .Select(a => { return a.Parse(t); })
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

        public class TokenizerResult
        {
            public object[] Tokens { get; set; }
            public string NotParsedEnd { get; set; }
        }
    }

    internal abstract class ValueTokenizer
    {
        public abstract TokenCandidate Parse(string txt);
    }

    internal class TokenCandidate
    {
        public TokenCandidate(object token, int stringLength, int priority)
        {
            Token = token;
            StringLength = stringLength;
            Priority = priority;
        }

        public override string ToString()
        {
            return Token.ToString();
        }

        public object Token { get; }
        public int StringLength { get; }
        public int Priority { get; }
    }

    internal abstract class AbstractRegexpTokenizer : ValueTokenizer
    {
        public override TokenCandidate Parse(string txt)
        {
            var m = getRegex().Match(txt);
            if (!m.Success)
                return null;
            var q = ParseValue(m);
            if (q is null)
                return null;
            return new TokenCandidate(q, m.Index + m.Length, GetPriority());
        }

        protected abstract int GetPriority();

        protected abstract Regex getRegex();

        protected abstract object ParseValue(Match m);
    }

    internal class DoubleTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 100; // najwyższy
        }

        protected override Regex getRegex()
        {
            return DoubleRegex;
        }

        protected override object ParseValue(Match m)
        {
            return double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
        }

        private static readonly Regex DoubleRegex =
            new Regex(DoubleFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private const string DoubleFilter = @"^\s*([+-]?(?:\d+(?:\.\d+)(?:e[+-]?\d+)?)|(?:\d+(?:e[+-]?\d+)))";
    }


    internal class IntegerTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 90;
        }

        protected override Regex getRegex()
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

    internal class StringTokenizer : AbstractRegexpTokenizer
    {
        protected override int GetPriority()
        {
            return 0;
        }

        protected override Regex getRegex()
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