using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class NetpbmImageLoader : IImageLoader {
        private readonly string[] whitespaces = new string[] { " ", "\t", "\n", "\r" };
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
            while ((c = reader.ReadChar()) != '\n')
                sb.Append(c);

            sb.Append(c);
            return sb.ToString();
        }

        private bool ReadNumber(BinaryReader reader, out int result, int maxLength = -1) {
            // first remove whitespaces and line comments
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

            while (string.IsNullOrWhiteSpace((char)reader.PeekChar() + ""))
                reader.ReadChar();

            return int.TryParse(digits, out result);
        }

        private Bitmap LoadPBM(int width, int height, BinaryReader reader, bool isBinary) {
            Bitmap bitmap = new Bitmap(width, height);
            bool[] pixelData;
            if (isBinary) {
                int byteCount = (width * height - 1) / 8 + 1;
                byte[] rawData = new byte[byteCount];
                if(reader.Read(rawData, 0, byteCount) != byteCount) {
                    throw new FormatException($"Expecting {byteCount} bytes of pixels");
                }
                pixelData = new bool[width * height];
                for(int i = 0; i < width*height; i++) {
                    int byteIndex = i / 8;
                    int bitIndex = 7 - (i % 8); // 7 = LSB, 0 = MSB
                    pixelData[i] = (rawData[byteIndex] & (1 << bitIndex)) > 0;
                }
            } else {
                pixelData = new bool[width * height];
                for(int i = 0; i < pixelData.Length; i++) {
                    if (!ReadNumber(reader, out int pixel, 1))
                        throw new FormatException($"Expecting {pixelData.Length} pixels as 0/1 but found only {i}");
                    pixelData[i] = pixel == 1;
                }
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bitmap.SetPixel(x, y, (pixelData[y * width + x] ? Color.Black : Color.White));
            return bitmap;
        }

        private Bitmap LoadPGM(int width, int height, BinaryReader reader, bool isBinary) {
            throw new NotImplementedException();
        }

        private Bitmap LoadPPM(int width, int height, BinaryReader reader, bool isBinary) {
            throw new NotImplementedException();
        }

        public bool IsSupportedFile(string path) {
            var extension = Path.GetExtension(path);
            return GetSupportedExtensions().Contains(extension);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".ppm", ".pgm", ".pbm" };
        }
    }
}
