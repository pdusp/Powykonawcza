using SimpleParser.TokenParsers;
using Xunit;
using Xunit.Abstractions;

namespace SimpleParser.Tests
{
    public class SingleQuoteStringTokenizerTest
    {
        public SingleQuoteStringTokenizerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void T01_Should_parse(string prefix)
        {
            const string expected = @"' Ala ma kota \'Jan\' \\'";
            var          txt      = prefix + expected + "  23 2 1";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new SingleQuoteStringTokenizer().Parse(txt);
            Assert.NotNull(x);

            Assert.Equal(expected, (string)x.Token);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void T02_Should_not_parse_missing_closing_quote(string prefix)
        {
            const string txt  = @"' Ala ma kota \'Jan\' \\  ";
            var          txt2 = prefix + txt + "  23 2 1";
            _testOutputHelper.WriteLine("Parse '{0}'", txt2);
            var x = new SingleQuoteStringTokenizer().Parse(txt2);
            Assert.Null(x);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void T03_Should_not_parse_single_backslash(string prefix)
        {
            const string txt  = @"' Ala ma kota \ other '";
            var          txt2 = prefix + txt + "  23 2 1";
            _testOutputHelper.WriteLine("Parse '{0}'", txt2);
            var x = new SingleQuoteStringTokenizer().Parse(txt2);
            Assert.Null(x);
        }

        private readonly ITestOutputHelper _testOutputHelper;
    }
}