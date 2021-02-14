using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConWinTer.Loader {
    public interface IImageLoader {
        /// <summary>
        /// Loads image from file specified by <paramref name="path"/> as a bitmap
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image FromFile(string path);

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
