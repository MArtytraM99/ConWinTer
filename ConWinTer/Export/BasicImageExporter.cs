using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Export {
    public class BasicImageExporter : IImageExporter {
        public void Export(Image image, string path) {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be empty");
            var imageFormat = PathUtils.GetImageFormat(path);
            image.Save(path, imageFormat);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".dib", ".tif", ".tiff" };
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }
    }
}
