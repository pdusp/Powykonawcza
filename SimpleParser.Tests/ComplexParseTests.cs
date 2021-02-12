using SimpleParser.TokenParsers;
using Xunit;

namespace SimpleParser.Tests
{
    public class ComplexParseTests
    {
        [Fact]
        public void T01_Should_parse_two_numbers()
        {
            var q = new Tokenizer().Parse(" 123 3.44   ");
            Assert.Equal(3, q.NotParsedEnd.Length);
            var tokens = q.Tokens;
            Assert.Equal(2, tokens.Length);
            Assert.Equal(123l, (long)tokens[0]);
            Assert.Equal(3.44, (double)tokens[1]);
        }


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
}