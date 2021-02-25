using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
