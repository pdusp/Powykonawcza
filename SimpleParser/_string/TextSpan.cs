using System;

namespace SimpleParser
{
    public struct TextSpan
    {
        public TextSpan(ITextSpanSource s, int start, int len)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            Source = s;
            Start  = start;
            Length = len;
        }

        public static TextSpan Concat(TextSpan[] items)
        {
            if (items == null || items.Length == 0)
                return Empty;
            if (items.Length == 1)
                return items[0];
            var result = items[0];
            for (var index = 1; index < items.Length; index++)
            {
                var i = items[index];
                if (ReferenceEquals(result.Source, i.Source))
                {
                    var max = result.Start + result.Length;
                    if (max == i.Start)
                        result = new TextSpan(result.Source, result.Start, max + i.Length);
                    else
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        public static TextSpan FromString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Empty;
            return new TextSpan(new StringTextSpanSource(text), 0, text.Length);
        }


        public TextSpan Substring(in int start, in int length)
        {
            return new TextSpan(Source, Start + start, length);
        }

        public TextSpan Substring(in int start)
        {
            return new TextSpan(Source, Start + start, Length - start);
        }

        public override string ToString()
        {
            if (Start == 0 && Length == Source.Text.Length)
                return Source.Text;
            return Source.Text.Substring(Start, Length);
        }

        public static TextSpan Empty => new TextSpan(null, 0, 0);

        public int Length { get; }

        public int Start { get; }

        public ITextSpanSource Source { get; }

        public bool IsNullOrEmpty => Length == 0;

        public char this[int i] => Source.Text[i + Start];
    }
}