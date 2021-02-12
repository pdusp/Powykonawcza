using SimpleParser.TokenParsers;
using Xunit;
using Xunit.Abstractions;

namespace SimpleParser.Tests
{
    public class StringNoSpaceTokenizerTest
    {
        public StringNoSpaceTokenizerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void T01_Should_parse(string prefix)
        {
            const string expected = "A234";
            var          txt      = prefix + expected + "  23 2 1";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new StringNoSpaceTokenizer().Parse(txt);
            Assert.NotNull(x);

            Assert.Equal(expected, (string)x.Token);
        }

        private readonly ITestOutputHelper _testOutputHelper;
    }
}