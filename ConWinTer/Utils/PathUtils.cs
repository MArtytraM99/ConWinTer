using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace ConWinTer.Utils {
    public static class PathUtils {
        public static string CombineUsingExtension(string path, string pathToFile, string extension) {
            var trimmedExtension = extension.Trim('.');
            var fileDirectory = Path.GetDirectoryName(pathToFile);
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(pathToFile);
            var filenameWithNewExtension = filenameWithoutExtension + "." + trimmedExtension;
            return Path.Combine(path, fileDirectory, filenameWithNewExtension);
        }

        /// <summary>
        /// Returns modified <paramref name="path"/> where filename is appended <paramref name="stringToAppend"/> preseving the same file extension
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringToAppend"></param>
        /// <returns></returns>
        public static string AppendToFilename(string path, string stringToAppend) {
            var filenameWOExt = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);
            var newFilename = $"{filenameWOExt}{stringToAppend}{extension}";
            return Path.Combine(Path.Combine(Path.GetDirectoryName(path), newFilename));
        }

        /// <summary>
        /// Determines if <paramref name="path"/> has extension defined in <paramref name="extensions"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public static bool HasExtension(string path, IEnumerable<string> extensions) {
            var extension = Path.GetExtension(path);
            return extensions.Contains(extension, new ExtensionComparer());
        }

        private class ExtensionComparer : IEqualityComparer<string> {
            public bool Equals([AllowNull] string x, [AllowNull] string y) {
                if (x == null && y == null)
                    return true;
                if (x != null && y == null)
                    return true;
                if (x == null && y != null)
                    return true;
                var trimmedX = x.Trim().Trim('.');
                var trimmedY = y.Trim().Trim('.');
                return trimmedX.Equals(trimmedY, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode([DisallowNull] string obj) {
                var trimmed = obj.Trim().Trim('.');
                return trimmed.GetHashCode();
            }
        }

        private static readonly Dictionary<string, ImageFormat> extensionImageFormatMap = new Dictionary<string, ImageFormat>(new ExtensionComparer()) {
            { "bmp", ImageFormat.Bmp },
            { "dib", ImageFormat.Bmp },
            { "gif", ImageFormat.Gif },
            { "jpg", ImageFormat.Jpeg },
            { "jpeg", ImageFormat.Jpeg },
            { "png", ImageFormat.Png },
            { "tif", ImageFormat.Tiff },
            { "tiff", ImageFormat.Tiff },
        };

        public static System.Drawing.Imaging.ImageFormat GetImageFormat(string path) {
            string extension = Path.GetExtension(path);

            if (!extensionImageFormatMap.TryGetValue(extension, out ImageFormat imageFormat))
                throw new ArgumentException("Path does not have a valid image extension.");

            return imageFormat;
        }
    }
}
