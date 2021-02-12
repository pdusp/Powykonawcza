using SimpleParser.TokenParsers;

namespace Powykonawcza
{
    internal class Tokenizer : AbstractTokenizer
    {
        protected override ValueTokenizer[] GetTokenizers()
        {
            var candidates = new ValueTokenizer[]
            {
                new DoubleTokenizer(),
                new IntegerTokenizer(),
                new StringNoSpaceTokenizer(),
                new SingleQuoteStringTokenizer()
            };
            return candidates;
        }
    }
}