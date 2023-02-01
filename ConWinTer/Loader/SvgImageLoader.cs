using ConWinTer.Utils;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ConWinTer.Loader {
    public class SvgImageLoader : IImageLoader {
        private readonly SvgPaintServer defaultPaintServer;

        public SvgImageLoader() : this(Color.Black) {}

        public SvgImageLoader(Color defaultColor) {
            defaultPaintServer = new SvgColourServer(defaultColor);
        }

        public Image FromFile(string path) {
            var svgDoc = SvgDocument.Open<SvgDocument>(path);

            processNodes(svgDoc.Descendants(), defaultPaintServer);

            return svgDoc.Draw();
        }

        private void processNodes(IEnumerable<SvgElement> nodes, SvgPaintServer colorServer) {
            foreach (var node in nodes) {
                if (!node.ContainsAttribute("stroke") && !node.ContainsAttribute("fill")) node.Stroke = colorServer;  // explicitely setting stroke when fill is set creates weird artifacts along edges of layered shapes
                if (!node.ContainsAttribute("fill")) node.Fill = colorServer;
                if (!node.ContainsAttribute("color")) node.Color = colorServer;

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
