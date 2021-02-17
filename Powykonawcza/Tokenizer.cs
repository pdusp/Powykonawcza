//using SimpleParser.TokenParsers;

using iSukces.Parsers;
using iSukces.Parsers.TokenParsers;

namespace Powykonawcza
{
    internal class Tokenizer : AbstractTokenizer
    {
        protected override ValueTokenizer[] GetTokenizers()
        {
            var candidates = new ValueTokenizer[]
            {
                new DoubleTokenizer(NumerFlags.AllowLedingSpaces, ',', '.'),
                new IntegerTokenizer(),
                new StringNoSpaceTokenizer(),
                new SingleQuoteStringTokenizer() 
            };
            return candidates;
        }
    }
}