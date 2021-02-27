using System;
using System.Collections.Generic;
using System.Text;

namespace ConWinTer.Loader {
    public interface ILoader {
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
