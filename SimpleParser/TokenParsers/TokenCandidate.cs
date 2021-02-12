namespace SimpleParser.TokenParsers
{
    public class TokenCandidate
    {
        public TokenCandidate(object token, int stringLength, int priority)
        {
            Token        = token;
            StringLength = stringLength;
            Priority     = priority;
        }

        public override string ToString()
        {
            return Token.ToString();
        }

        public object Token        { get; }
        public int    StringLength { get; }
        public int    Priority     { get; }
    }
}