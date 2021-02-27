using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ConWinTer.Export {
    public class CompositeImageExporter : IImageExporter {
        private Dictionary<string, IImageExporter> extensionExporterMap;

        public CompositeImageExporter() {
            extensionExporterMap = new Dictionary<string, IImageExporter>();
        }

        public void RegisterExporter(IEnumerable<string> extensions, IImageExporter exporter) {
            foreach (string extension in extensions)
                extensionExporterMap[extension] = exporter;
        }

        /// <summary>
        /// Registers <paramref name="exporter"/> that will be used for exporting images with <paramref name="extension"/>
        /// </summary>
        /// <param name="extension">Supported extension</param>
        /// <param name="exporter"></param>
        public void RegisterExporter(string extension, IImageExporter exporter) {
            RegisterExporter(new List<string> { extension }, exporter);
        }

        /// <summary>
        /// Registers <paramref name="exporter"/> that will be used for exporting images accepting all extensions that <paramref name="exporter"/> supports.
        /// </summary>
        /// <param name="exporter"></param>
        public void RegisterExporter(IImageExporter exporter) {
            RegisterExporter(exporter.GetSupportedExtensions(), exporter);
        }

        public void Export(Image image, string path) {
            string extension = Path.GetExtension(path);
            var exporter = extensionExporterMap[extension];

            if (exporter == null)
                throw new ArgumentException($"Unsupported extension '{extension}' for exporting");

            exporter.Export(image, path);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return extensionExporterMap.Keys;
        }

        public bool IsSupportedFile(string path) {
            return PathUtils.HasExtension(path, GetSupportedExtensions());
        }
    }
}
