using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConWinTer.Loader {
    public interface IExcelLoader : ILoader {
        public IEnumerable<Table> LoadAllSheets(string path);
    }
}
