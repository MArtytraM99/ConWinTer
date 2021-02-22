using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConWinTer.Export {
    public interface IImageExporter {
        /// <summary>
        /// Exports <paramref name="image"/> to file specified in <paramref name="path"/>. File type is specified by file extension.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        void Export(Image image, string path);
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
