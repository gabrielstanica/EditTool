using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;

namespace EditTool
{
    class Program
    {
        private static string filename = String.Empty;
        private static string tempfile = String.Empty;


        void breakString(String file, string keyToAdd, string lines)
        {
            string[] toCheck = lines.ToLower().Split(',');

            StreamWriter streamWriter = new StreamWriter(tempfile);
            if (!File.Exists(filename)) throw new FileNotFoundException("The file does not exist");

            var allLines = File.ReadAllLines(file);

            using (streamWriter)
            {
                foreach (var linie in allLines)
                {

                    foreach (var element in toCheck)
                    {
                        if (linie.ToLower().Contains(element))
                        {
                            streamWriter.WriteLine(linie);
                        }
                    }
                }
            }


            allLines = File.ReadAllLines(tempfile);

            var moreElements = keyToAdd.Split(';');
            foreach (var item in moreElements)
            {
                var eachLine = item.Split(',');

                string juiceLine = String.Format("{0}={1}", eachLine[0], eachLine[1]);
                if (Regex.Match(File.ReadAllText(tempfile), String.Format(@"{0}=", eachLine[0])).Success)
                {
                    File.WriteAllText(tempfile, Regex.Replace(File.ReadAllText(tempfile), String.Format(@"{0}=\d[0-9].+", eachLine[0]), juiceLine));
                }
                else
                {
                    File.AppendAllText(tempfile, juiceLine);
                }
            }
        }


        static void Main(string[] args)
        {
            Program util = new Program();

            string juiceValue = String.Empty;
            string toKeepLines = String.Empty;

            if (!Debugger.IsAttached)
            {
                if (args.Length > 2)
                {
                    filename = args[0];
                    tempfile = args[1];
                    toKeepLines = args[2];
                    juiceValue = args[3];
                }
                else
                {
                    throw new Exception("Fraiereee");
                }
            }
            else
            {
                filename = @"C:\Users\gastanica\Downloads\juice.ini";
                tempfile = @"C:\Users\gastanica\Downloads\juice.temp.ini";

                juiceValue = ConfigurationManager.AppSettings["ToAdd"];
                toKeepLines = ConfigurationManager.AppSettings["ToKeep"];

            }
            util.breakString(filename, juiceValue, toKeepLines);

        }
    }
}
