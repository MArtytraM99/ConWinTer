using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ConWinTer.Loader {
    class BasicImageLoader : IImageLoader {
        public Image FromFile(string path) {
            return Image.FromFile(path);
        }
    }
}
