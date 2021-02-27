using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

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
                return obj.GetHashCode();
            }
        }
    }
}
