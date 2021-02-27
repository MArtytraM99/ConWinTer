using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConWinTer.Loader {
    public interface IImageLoader : ILoader {
        /// <summary>
        /// Loads image from file specified by <paramref name="path"/> as a bitmap
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image FromFile(string path);
    }
}
