using SimpleParser.TokenParsers;
using Xunit;
using Xunit.Abstractions;

namespace SimpleParser.Tests
{
    public class DoubleTokenizerTest
    {
        public DoubleTokenizerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T01_Should_not_parse_int(string prefix)
        {
            var txt = prefix + "1 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new DoubleTokenizer().Parse(txt);
            Assert.Null(x);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T02_Should_parse_fract(string prefix)
        {
            var txt = prefix + "1.23 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new DoubleTokenizer().Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -1.23 : 1.23;
            Assert.Equal(expected, (double)x.Token);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T03_Should_parse_no_fract_exp(string prefix)
        {
            var txt = prefix + "12e3 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new DoubleTokenizer().Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -12e3 : 12e3;
            Assert.Equal(expected, (double)x.Token);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T04_Should_parse_fract_exp(string prefix)
        {
            var txt = prefix + "1.23e3 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new DoubleTokenizer().Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -1.23e3 : 1.23e3;
            Assert.Equal(expected, (double)x.Token);
        }

        private readonly ITestOutputHelper _testOutputHelper;
    }
}