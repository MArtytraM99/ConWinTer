using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class SvgImageLoader : IImageLoader {
        private readonly SvgPaintServer svgPaintServer;

        public SvgImageLoader() : this(Color.Black) {}

        public SvgImageLoader(Color color) {
            svgPaintServer = new SvgColourServer(color);
        }

        public Image FromFile(string path) {
            var svgDoc = SvgDocument.Open<SvgDocument>(path);

            processNodes(svgDoc.Descendants(), svgPaintServer);

            return svgDoc.Draw();
        }

        private void processNodes(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer) {
            foreach (var node in nodes) {
                if (node.Fill != SvgPaintServer.None) node.Fill = colorServer;
                if (node.Color != SvgPaintServer.None) node.Color = colorServer;
                if (node.Stroke != SvgPaintServer.None) node.Stroke = colorServer;

                processNodes(node.Descendants(), colorServer);
            }
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return new List<string> { ".svg" };
        }
    }
}
