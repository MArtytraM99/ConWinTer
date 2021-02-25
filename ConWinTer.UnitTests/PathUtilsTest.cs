﻿using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
