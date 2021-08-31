using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using Xunit;

namespace ConWinTer.UnitTests {
    public class PathUtilsTest {

        public static IEnumerable<object[]> GetPathsToCombineUsingExtension() {
            yield return new object[] { @"C:\", "file.txt", ".png", @"C:\file.png" };

            yield return new object[] { "C:/", "file.txt", ".png", "C:/file.png" };

            yield return new object[] { @"C:\dir\", @"subdir\file.txt", ".png", @"C:\dir\subdir\file.png" };

            yield return new object[] { "", @"subdir\file.txt", ".png", @"subdir\file.png" };

            yield return new object[] { @"C:\", @"dir\subdir\file", ".png", @"C:\dir\subdir\file.png" };

            yield return new object[] { @"C:\", "file.ext.txt", ".png", @"C:\file.ext.png" };

            yield return new object[] { @"C:\", "file.txt", "png", @"C:\file.png" };

            yield return new object[] { @"C:\", "file", "png", @"C:\file.png" };
        }

        [Theory]
        [MemberData(nameof(GetPathsToCombineUsingExtension))]
        public void CombineUsingExtension_CorrectOutput(string path1, string pathToFile, string extension, string expectedPath) {
            var combinedPath = PathUtils.CombineUsingExtension(path1, pathToFile, extension);

            Assert.Equal(expectedPath, combinedPath);
        }

        public static IEnumerable<object[]> GetPathsToAppendToFilename() {
            yield return new object[] { "file.txt", "_appended", "file_appended.txt" };

            yield return new object[] { "file.png.txt", "_appended", "file.png_appended.txt" };

            yield return new object[] { @"C:\dir\file.txt", "_appended", @"C:\dir\file_appended.txt" };
        }

        [Theory]
        [MemberData(nameof(GetPathsToAppendToFilename))]
        public void AppendToFilename_CorrectOutput(string path, string stringToAppend, string expectedPath) {
            var appended = PathUtils.AppendToFilename(path, stringToAppend);

            Assert.Equal(expectedPath, appended);
        }

        public static IEnumerable<object[]> GetPathsToTestHasExtension() {
            yield return new object[] { "file.txt", new List<string> { "txt" }, true };

            yield return new object[] { "file.txt", new List<string> { ".txt" }, true };

            yield return new object[] { "file.txt", new List<string> { "TXT" }, true };

            yield return new object[] { "file.TXT", new List<string> { "txt" }, true };

            yield return new object[] { "file.TXT", new List<string> { ".TXT" }, true };

            yield return new object[] { "file.txt", new List<string> { "png", "jpg", "txt" }, true };

            yield return new object[] { "file.txt", new List<string> { "png", "jpg" }, false };

            yield return new object[] { "file.txt", new List<string> { }, false };

            yield return new object[] { "file.txt", new List<string> { "tx" }, false };

            yield return new object[] { "file.txt", new List<string> { ".tx" }, false };

            yield return new object[] { "file.txt", new List<string> { "t", "x", "t" }, false };

            yield return new object[] { "file.png.txt", new List<string> { "txt" }, true };

            yield return new object[] { "file.png.txt", new List<string> { ".txt" }, true };

            yield return new object[] { "file.png.txt", new List<string> { "png" }, false };

            yield return new object[] { @"C:\dir\file.txt", new List<string> { ".txt" }, true };
        }

        [Theory]
        [MemberData(nameof(GetPathsToTestHasExtension))]
        public void HasExtension_CorrectOutput(string path, IEnumerable<string> extensions, bool expectedResult) {
            var result = PathUtils.HasExtension(path, extensions);

            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetPathToTestGetImageFormat() {
            yield return new object[] { "image.bmp", ImageFormat.Bmp };
            yield return new object[] { "image.dib", ImageFormat.Bmp };
            yield return new object[] { "image.gif", ImageFormat.Gif };
            yield return new object[] { "image.jpg", ImageFormat.Jpeg };
            yield return new object[] { "image.jpeg", ImageFormat.Jpeg };
            yield return new object[] { "image.png", ImageFormat.Png };
            yield return new object[] { "image.tif", ImageFormat.Tiff };
            yield return new object[] { "image.tiff", ImageFormat.Tiff };
        }

        [Theory]
        [MemberData(nameof(GetPathToTestGetImageFormat))]
        public void GetImageFormat_DecodesFormat(string path, ImageFormat imageFormat) {
            var result = PathUtils.GetImageFormat(path);

            Assert.Equal(imageFormat, result);
        }

        public static IEnumerable<object[]> GetPathToTestGetImageFormatThrows() {
            yield return new object[] { "image" };
            yield return new object[] { "file.txt" };
        }

        [Theory]
        [MemberData(nameof(GetPathToTestGetImageFormatThrows))]
        public void GetImageFormat_Throws(string path) {
            Assert.Throws<ArgumentException>(() => PathUtils.GetImageFormat(path));
        }
    }
}
