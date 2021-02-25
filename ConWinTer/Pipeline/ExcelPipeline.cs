using ConWinTer.Export;
using ConWinTer.Loader;
using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConWinTer.Pipeline {
    public class ExcelPipeline {
        private readonly IExcelLoader loader;
        private readonly ITableExporter tableExporter;

        public ExcelPipeline(IExcelLoader loader, ITableExporter tableExporter) {
            this.loader = loader;
            this.tableExporter = tableExporter;
        }
        public void Run(string input, string output, string outputFormat) {
            if (!File.Exists(input))
                throw new ArgumentException($"Input file in path '{input}' doesn't exist.");

            var outputPath = GetOutputPath(input, output, outputFormat);

            if (!loader.IsSupportedFile(input)) {
                var msg = $"Input file '{input}' is not supported.\nSupported input extensions: {string.Join(", ", loader.GetSupportedExtensions())}";
                throw new ArgumentException(msg);
            }

            if (!tableExporter.IsSupportedFile(outputPath)) {
                var msg = $"Output file '{outputPath}' is not supported.\nSupported output extensions: {string.Join(", ", tableExporter.GetSupportedExtensions())}";
                throw new ArgumentException(msg);
            }

            var tables = loader.LoadAllSheets(input);
            foreach(var table in tables) {
                var toAppend = string.IsNullOrEmpty(table.Name) ? "" : $"_{table.Name}";
                var tableOutputPath = PathUtils.AppendToFilename(outputPath, toAppend);
                tableExporter.Export(table, tableOutputPath);
            }
        }

        private string GetOutputPath(string input, string output, string outputFormat) {
            if (string.IsNullOrEmpty(output))
                return Path.ChangeExtension(input, outputFormat);
            if (!string.IsNullOrEmpty(outputFormat))
                return Path.ChangeExtension(output, outputFormat);
            return output;
        }

        public int RunWithExceptionHandling(string input, string output, string outputFormat) {
            try {
                Run(input, output, outputFormat);
                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return 1;
            }
        }
    }
}
