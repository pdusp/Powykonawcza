namespace SimpleParser.TokenParsers
{
    public class TokenizerResult
    {
        public object[] Tokens       { get; set; }
        public TextSpan NotParsedEnd { get; set; }
    }
}