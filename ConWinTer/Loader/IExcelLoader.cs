using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConWinTer.Loader {
    public interface IExcelLoader {
        public IEnumerable<Table> LoadAllSheets(string path);
        /// <summary>
        /// Returns true if file extension is supported
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsSupportedFile(string path);

        /// <summary>
        /// Returns all supported extensions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSupportedExtensions();
    }
}
