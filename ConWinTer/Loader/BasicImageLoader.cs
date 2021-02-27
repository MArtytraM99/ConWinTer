using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    class BasicImageLoader : IImageLoader {
        public BasicImageLoader() {
        }

        public Image FromFile(string path) {
            return Image.FromFile(path);
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".dib", ".tif", ".tiff", ".ico" };
        }
    }
}
