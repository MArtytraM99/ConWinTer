using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Export {
    public class IconImageExporter : IImageExporter {
        private readonly int[] sizes;

        /// <summary>
        /// Constructs IconImageExporter which will output icon containing images with sizes: 256, 48, 32, 16
        /// </summary>
        public IconImageExporter() : this(new int[] { 256, 48, 32, 16 }) {

        }
        public IconImageExporter(int[] sizes) {
            if (sizes.Any(size => size > 256))
                throw new ArgumentException("Sizes for icon cannot be larger than 256");
            if (sizes.Any(size => size <= 0))
                throw new ArgumentException("Sizes cannot be negative or zero");
            this.sizes = sizes;
        }

        public void Export(Image image, string path) {
            using var bw = new BinaryWriter(File.OpenWrite(path));

            bw.Write((short)0); // reserved
            bw.Write((short)1); // 1 = ico, 2 = cur
            bw.Write((short)sizes.Length);
            var offset = 6 + 16 * sizes.Length;

            byte[][] pngData = new byte[sizes.Length][];
            for(int i = 0; i < sizes.Length; i++) {
                var resizedImage = ResizeImage(image, sizes[i]);
                using (var memStream = new MemoryStream()) {
                    resizedImage.Save(memStream, ImageFormat.Png);
                    pngData[i] = memStream.ToArray();
                }


                bw.Write((byte)resizedImage.Width);
                bw.Write((byte)resizedImage.Height);
                bw.Write((byte)0); // no color palette
                bw.Write((byte)0); // reserved
                bw.Write((short)0); // color plane
                bw.Write((short)32); // 32 bit per pixel
                bw.Write((int)pngData[i].Length); // # of bytes in image
                bw.Write((int)offset);;
                offset += (int)pngData[i].Length;
            }

            for (int i = 0; i < sizes.Length; i++)
                bw.Write(pngData[i]);
        }

        public static Bitmap ResizeImage(Image image, int size) {
            if(image.Width >= image.Height) {
                int height = (int)(size * 1.0 * image.Height / image.Width);
                return ResizeImage(image, size, height);
            } else {
                int width = (int)(size * 1.0 * image.Width / image.Height);
                return ResizeImage(image, width, size);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".ico" };
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }
    }
}
