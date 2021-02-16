using SimpleParser.TokenParsers;
using Xunit;
using Xunit.Abstractions;

namespace SimpleParser.Tests
{
    public class IntegerTokenizerTest
    {
        public IntegerTokenizerTest(ITestOutputHelper testOutputHelper)
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
        public void T01_Should_parse(string prefix)
        {
            var txt = prefix + "123 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new IntegerTokenizer().Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -123 : 123;
            Assert.Equal(expected, (long)x.Token);
        }

        private readonly ITestOutputHelper _testOutputHelper;
    }
}