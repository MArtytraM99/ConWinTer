using ConWinTer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class NetpbmImageLoader : IImageLoader {
        public Image FromFile(string path) {
            var reader = new BinaryReader(File.OpenRead(path));
            var type = string.Join("", reader.ReadChars(2));
            if (!ReadNumber(reader, out int width))
                throw new FormatException("Unable to read image width");
            if (!ReadNumber(reader, out int height))
                throw new FormatException("Unable to read image height");

            return type switch {
                "P1" => LoadPBM(width, height, reader, false),
                "P2" => LoadPGM(width, height, reader, false),
                "P3" => LoadPPM(width, height, reader, false),
                "P4" => LoadPBM(width, height, reader, true),
                "P5" => LoadPGM(width, height, reader, true),
                "P6" => LoadPPM(width, height, reader, true),
                _ => throw new FormatException("Unsupported format of Netpbm image. Supports P1-P6"),
            };
        }

        private string ReadLine(BinaryReader reader) {
            var sb = new StringBuilder();
            char c;
            while ((c = reader.ReadChar()) != '\n') {
                sb.Append(c);
                if (reader.BaseStream.Position == reader.BaseStream.Length)
                    return sb.ToString();
            }

            sb.Append(c);
            return sb.ToString();
        }

        /// <summary>
        /// Eats all whitespaces and comments in <paramref name="reader"/> at current position
        /// </summary>
        /// <param name="reader"></param>
        private void EraseWhitespaceComments(BinaryReader reader) {
            bool shouldTest = true;
            while (shouldTest) {
                bool isWhitespace = string.IsNullOrWhiteSpace((char)reader.PeekChar() + "");
                bool isComment = (char)reader.PeekChar() == '#';
                shouldTest = isWhitespace || isComment;
                if (isWhitespace)
                    while (string.IsNullOrWhiteSpace((char)reader.PeekChar() + ""))
                        reader.ReadChar();
                else if (isComment)
                    ReadLine(reader);
            }
        }

        private bool ReadNumber(BinaryReader reader, out int result, int maxLength = -1) {
            EraseWhitespaceComments(reader);

            string digits = "";
            while (true) {
                if (digits.Length == maxLength)
                    break;
                int pk = reader.PeekChar();
                if (pk == -1)
                    break;

                char c = (char)pk;
                if (!char.IsDigit(c))
                    break;

                digits += c;
                reader.ReadChar();
            }

            EraseWhitespaceComments(reader);

            return int.TryParse(digits, out result);
        }

        private Bitmap LoadPBM(int width, int height, BinaryReader reader, bool isBinary) {
            Bitmap bitmap = new Bitmap(width, height);
            bool[] pixelData = new bool[width * height];
            if (isBinary) {
                int byteCount = (width * height - 1) / 8 + 1;
                byte[] rawData = new byte[byteCount];
                if(reader.Read(rawData, 0, byteCount) != byteCount)
                    throw new FormatException($"Expected {byteCount} bytes of pixels");

                for(int i = 0; i < pixelData.Length; i++) {
                    int byteIndex = i / 8;
                    int bitIndex = 7 - (i % 8); // 7 = LSB, 0 = MSB
                    pixelData[i] = (rawData[byteIndex] & (1 << bitIndex)) > 0;
                }
            } else {
                for(int i = 0; i < pixelData.Length; i++) {
                    if (!ReadNumber(reader, out int pixel, 1))
                        throw new FormatException($"Expected {pixelData.Length} pixels as 0/1 but found only {i}");
                    pixelData[i] = pixel == 1;
                }
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bitmap.SetPixel(x, y, (pixelData[y * width + x] ? Color.Black : Color.White));
            return bitmap;
        }

        private Bitmap LoadPGM(int width, int height, BinaryReader reader, bool isBinary) {
            if (!ReadNumber(reader, out int maxValue))
                throw new FormatException("Expected max pixel value number in PGM format");
            if (maxValue > 255)
                throw new ArgumentException("Only one byte pixel values are supported. Max pixel value cannot be bigger than 255.");
            if (maxValue <= 0)
                throw new ArgumentException("Max pixel value must be a positive integer");

            Bitmap bitmap = new Bitmap(width, height);
            int[] pixelData = new int[width * height];

            if (isBinary) {
                byte[] rawData = new byte[pixelData.Length];
                if (reader.Read(rawData, 0, rawData.Length) != rawData.Length)
                    throw new FormatException($"Expected {rawData.Length} bytes of pixels");

                for (int i = 0; i < pixelData.Length; i++)
                    pixelData[i] = rawData[i];
            } else {
                for (int i = 0; i < pixelData.Length; i++) {
                    if (!ReadNumber(reader, out int pixel))
                        throw new FormatException($"Expected {pixelData.Length} pixels as numbers but found only {i}");
                    pixelData[i] = pixel;
                }
            }

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int grayscale = 255 * pixelData[y * width + x] / maxValue;
                    bitmap.SetPixel(x, y, Color.FromArgb(grayscale, grayscale, grayscale));
                }
            }

            return bitmap;
        }

        private Bitmap LoadPPM(int width, int height, BinaryReader reader, bool isBinary) {
            if (!ReadNumber(reader, out int maxValue))
                throw new FormatException("Expected max pixel value number in PPM format");
            if (maxValue > 255)
                throw new ArgumentException("Only one byte pixel values are supported. Max pixel value cannot be bigger than 255.");
            if (maxValue <= 0)
                throw new ArgumentException("Max pixel value must be a positive integer");

            Bitmap bitmap = new Bitmap(width, height);
            Color[] pixelData = new Color[width * height];

            if (isBinary) {
                byte[] rawData = new byte[pixelData.Length * 3];
                if (reader.Read(rawData, 0, rawData.Length) != rawData.Length)
                    throw new FormatException($"Expected {rawData.Length} bytes of pixels");

                for (int i = 0; i < pixelData.Length; i++)
                    pixelData[i] = Color.FromArgb(255 * rawData[3 * i + 0] / maxValue, 255 * rawData[3 * i + 1] / maxValue, 255 * rawData[3 * i + 2] / maxValue);
            } else {
                for (int i = 0; i < pixelData.Length; i++) {
                    if (!ReadNumber(reader, out int r))
                        throw new FormatException($"Expected {pixelData.Length} pixels as 3 numbers but found only {i}");
                    if (!ReadNumber(reader, out int g))
                        throw new FormatException($"Expected {pixelData.Length} pixels as 3 numbers but found only {i}");
                    if (!ReadNumber(reader, out int b))
                        throw new FormatException($"Expected {pixelData.Length} pixels as 3 numbers but found only {i}");
                    pixelData[i] = Color.FromArgb(255 * r / maxValue, 255 * g / maxValue, 255 * b / maxValue);
                }
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bitmap.SetPixel(x, y, pixelData[y * width + x]);
            return bitmap;
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".ppm", ".pgm", ".pbm" };
        }
    }
}
