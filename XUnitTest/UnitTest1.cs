using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using Powykonawcza;
using Powykonawcza.DAL;
using Powykonawcza.Model.Szablon;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        public static string GetResourceByName(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var reader   = new StreamReader(assembly.GetManifestResourceStream(resource));
            return reader.ReadToEnd();
        }


        [Fact]
        public void ImportTXT()
        {
            List<SzablonItem> szablonItems;
            string projectDirectory;
            try
            {
                string path = Directory.GetCurrentDirectory();
                System.Console.WriteLine("The current directory is {0}", path);
                string workingDirectory = Environment.CurrentDirectory;
                projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName.Replace("\\bin", "\\PrzykładyTXT");

                szablonItems =
                    JsonUtils.LoadJsonFile<List<SzablonItem>>(projectDirectory+ @"\Szablony\SzablonImportuTXT1.dat");
                //if (szablonItems is null)
                //  MessageBox.Show("brak pliku SzablonImportu.dat");

                szablonItems = szablonItems.Where(p => p.import).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ex.Message);
            }

            //Assert.Equal(1, 2);
            ;
            var expectedColumns = szablonItems.Count();
            if (expectedColumns < 4)
                Assert.Equal(4, expectedColumns);


            string filePath = projectDirectory + @"\GK.6640.2865.2020_BDOT.txt";
            var rtbLines  =   System.IO.File.ReadLines(filePath).ToList();
            //czyszczenie z ewidentnie pustych linii 
            rtbLines = rtbLines.Where(w => w.Length > 4).ToList();

            var lineNo = 0;
            foreach (var txtline in rtbLines)
            {
                lineNo++;
                if (txtline.Length < 10)
                {
                    Assert.Equal(10, txtline.Length);
                }
            }

            foreach (var txtline in rtbLines)
            {
                var txt     = txtline.Replace("\t", " ").Replace("\r", "");
                var objects = new Tokenizer().Parse(txt);
                if (objects.Tokens.Length != expectedColumns)
                {
                    Assert.Equal(objects.Tokens.Length, expectedColumns);
                }
            }

      
        }
    }
}