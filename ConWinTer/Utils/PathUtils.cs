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
    }
}
