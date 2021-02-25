using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConWinTer.Export {
    public interface ITableExporter {
        /// <summary>
        /// Exports <paramref name="table"/> to file specified by <paramref name="path"/>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        public void Export(Table table, string path);
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
