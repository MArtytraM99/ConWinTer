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

        public static IEnumerable<object[]> GetPathsToChangeExtension() {
            yield return new object[] { @"C:\file.txt", "png", @"C:\file.png" };

            yield return new object[] { @"C:\file", "png", @"C:\file.png" };

            yield return new object[] { @"C:\file.txt", ".png", @"C:\file.png" };

            yield return new object[] { @"C:\file", ".png", @"C:\file.png" };

            yield return new object[] { "/etc/file.txt", "png", @"\etc\file.png" };

            yield return new object[] { "file.txt", "png", "file.png" };

            yield return new object[] { "./file.txt", "png", @".\file.png" };

            yield return new object[] { "./file", "png", @".\file.png" };

            yield return new object[] { @"..\..\dir\file.txt", "png", @"..\..\dir\file.png" };

            yield return new object[] { @"..\..\dir\file", "png", @"..\..\dir\file.png" };
        }

        [Theory]
        [MemberData(nameof(GetPathsToChangeExtension))]
        public void ChangeExtension_CorrectOutput(string filepath, string extension, string expectedPath) {
            var resultPath = PathUtils.ChangeExtesion(filepath, extension);

            Assert.Equal(expectedPath, resultPath);
        }
    }
}
