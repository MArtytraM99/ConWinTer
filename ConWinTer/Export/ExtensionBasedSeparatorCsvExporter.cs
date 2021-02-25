using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Export {
    public class ExtensionBasedSeparatorCsvExporter : ITableExporter {
        private readonly CsvTableExporter csvTableExporter;
        public ExtensionBasedSeparatorCsvExporter() {
            csvTableExporter = new CsvTableExporter();
        }
        public void Export(Table table, string path) {
            var extension = Path.GetExtension(path);
            var separatorCode = extension.Substring(4);
            var separator = GetSeparatorFromCode(separatorCode);

            csvTableExporter.Separator = separator;
            var csvPath = Path.ChangeExtension(path, "csv");
            csvTableExporter.Export(table, csvPath);
        }

        private string GetSeparatorFromCode(string separatorCode) {
            switch(separatorCode) {
                case "":
                case ",":
                    return ",";
                case ";":
                    return ";";
                case "t":
                    return "\t";
                default:
                    return ",";
            }
        }

        public bool IsSupportedFile(string path) {
            var extension = Path.GetExtension(path);
            return GetSupportedExtensions().Contains(extension);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".csv", ".csv,", ".csv;", ".csvt" };
        }
    }
}
