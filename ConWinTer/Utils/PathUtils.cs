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

        public static string ChangeExtesion(string path, string extension) {
            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            return CombineUsingExtension(directory, filename, extension);
        }
    }
}
