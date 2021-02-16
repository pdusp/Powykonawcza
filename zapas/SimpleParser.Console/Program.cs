namespace SimpleParser.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var objects = new Tokenizer()
                .Parse("1  1.23 1e33  TEKST_BEZ SPACJI 'TESKST W APOSTROFACH' \t PO_TABULATORZE");
            foreach (var i in objects.Tokens)
                System.Console.WriteLine(i);
        }
    }
}