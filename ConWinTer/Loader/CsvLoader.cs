using ConWinTer.Model;
using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConWinTer.Loader {
    public class CsvLoader : ITableLoader {
        private readonly string separator;

        public CsvLoader(string separator) {
            this.separator = separator;
        }

        public Table FromFile(string path) {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length == 0)
                return new Table();
            int cols = lines[0].Split(separator, StringSplitOptions.None).Length;

            string[,] data = new string[lines.Length, cols];
            for(int i = 0; i < lines.Length; i++) {
                string[] splitLine = lines[i].Split(separator, StringSplitOptions.None);
                if (splitLine.Length != cols)
                    throw new FormatException($"File '{path}' does not contain a valid table. {cols} columns were expected but line '{lines[i]}' contains {splitLine.Length} columns");
                for (int j = 0; j < cols; j++)
                    data[i, j] = splitLine[j];
            }

            return new Table(data);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".csv" };
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }
    }
}
