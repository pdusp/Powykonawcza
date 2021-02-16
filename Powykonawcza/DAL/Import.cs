using System.Collections.Specialized;
using System.Windows.Controls;

namespace Powykonawcza.DAL
{
    public static class Import
    {
        public static StringCollection GetLinesCollectionFromTextBox(TextBox textBox)
        {
            var lines = new StringCollection();

            // lineCount may be -1 if TextBox layout info is not up-to-date.
            var lineCount = textBox.LineCount;

            for (var line = 0; line < lineCount; line++)
                // GetLineText takes a zero-based line index.
                lines.Add(textBox.GetLineText(line));

            return lines;
        }
    }
}