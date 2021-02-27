using ConWinTer.Model;
using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class CsvLoader : ITableLoader {
        private readonly string separator;

        /// <summary>
        /// Initializes CsvLoader instace with <paramref name="separator"/> to be used for parsing file. If left empty/null than separator will be deduced from file.
        /// </summary>
        /// <param name="separator"></param>
        public CsvLoader(string separator) {
            this.separator = separator;
        }

        public Table FromFile(string path) {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
                return new Table();

            var deducedSeparator = separator;
            if (string.IsNullOrEmpty(separator))
                deducedSeparator = DeduceSeparator(lines);

            int cols = lines[0].Split(separator, StringSplitOptions.None).Length;

            string[,] data = new string[lines.Length, cols];
            for(int i = 0; i < lines.Length; i++) {
                string[] splitLine = lines[i].Split(separator, StringSplitOptions.None);
                if (splitLine.Length != cols)
                    throw new FormatException($"File '{path}' does not contain a valid table. {cols} columns were expected but line '{lines[i]}' contains {splitLine.Length} columns. Using '{separator}' as separator");
                for (int j = 0; j < cols; j++)
                    data[i, j] = splitLine[j];
            }

            return new Table(data);
        }

        private string DeduceSeparator(string[] lines) {
            // deduce separator from first two lines by comparing number of columns that the separator splits the lines into
            var possibleSeparators = new List<string> { "\t", ";", "," }; // ordered by priority
            if(lines.Length == 1) {
                return possibleSeparators.First(sep => lines[0].Contains(sep));
            }
            
            foreach(var possibleSeparator in possibleSeparators) {
                var firstLineColNum = lines[0].Split(possibleSeparator, StringSplitOptions.None).Length;
                var secondLineColNum = lines[1].Split(possibleSeparator, StringSplitOptions.None).Length;
                if (firstLineColNum > 1 && firstLineColNum == secondLineColNum)
                    return possibleSeparator;
            }

            return ","; // default separator
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".csv" };
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }
    }
}
