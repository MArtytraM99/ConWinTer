using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Export {
    public class CsvTableExporter : ITableExporter {
        private readonly string separator;

        public CsvTableExporter(string separator = ",") {
            this.separator = separator;
        }

        public void Export(Table table, string path) {
            if (!IsSupportedFile(path))
                throw new ArgumentException($"File '{path}' is not supported.");

            var writer = new StreamWriter(File.OpenWrite(path));
            foreach (var row in table.AsRowsArray)
                writer.WriteLine(string.Join(separator, row));
            
            writer.Flush();
            writer.Close();
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".csv" };
        }

        public bool IsSupportedFile(string path) {
            var extension = Path.GetExtension(path);
            return GetSupportedExtensions().Contains(extension);
        }
    }
}
