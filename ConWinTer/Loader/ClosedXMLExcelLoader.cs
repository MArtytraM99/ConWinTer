using ClosedXML.Excel;
using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class ClosedXMLExcelLoader : IExcelLoader {
        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".xlsx" };
        }

        public bool IsSupportedFile(string path) {
            var extension = Path.GetExtension(path);
            return GetSupportedExtensions().Contains(extension);
        }

        public IEnumerable<Table> LoadAllSheets(string path) {
            var wb = new XLWorkbook(path, XLEventTracking.Disabled);

            List<Table> tables = new List<Table>();
            foreach(var sheet in wb.Worksheets) {
                tables.Add(LoadSheet(sheet));
            }

            return tables;
        }

        public Table LoadSheet(IXLWorksheet worksheet) {
            var usedRange = worksheet.RangeUsed();

            string[,] data = new string[usedRange.RowCount(), usedRange.ColumnCount()];
            int rowIndex = 0;
            foreach(var row in usedRange.Rows()) {
                for (int colIndex = 0; colIndex < data.GetLength(1); colIndex++)
                    data[rowIndex, colIndex] = row.Cell(colIndex+1).GetString();
                rowIndex++;
            }

            return new Table(data, worksheet.Name);
        }
    }
}
