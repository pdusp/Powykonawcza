using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Powykonawcza.DAL
{
    public static class Import
    {
        public static StringCollection GetLinesCollectionFromTextBox(TextBox textBox)
        {
            StringCollection lines = new StringCollection();

            // lineCount may be -1 if TextBox layout info is not up-to-date.
            int lineCount = textBox.LineCount;

            for (int line = 0; line < lineCount; line++)
                // GetLineText takes a zero-based line index.
                lines.Add(textBox.GetLineText(line));

            return lines;
        }


    }
}
