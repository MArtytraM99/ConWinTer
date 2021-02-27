using ConWinTer.Export;
using ConWinTer.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConWinTer.Pipeline {
    public class ImagePipeline : ConversionPipelineBase {
        private readonly IImageLoader loader;
        private readonly IImageExporter exporter;

        public ImagePipeline(IImageLoader loader, IImageExporter exporter) {
            this.loader = loader;
            this.exporter = exporter;
        }

        public override void Run(string input, string output, string outputFormat) {
            if (!File.Exists(input))
                throw new ArgumentException($"Input file in path '{input}' doesn't exist.");

            var outputPath = GetOutputPath(input, output, outputFormat);

            if (!loader.IsSupportedFile(input)) {
                var msg = $"Input file '{input}' is not supported.\nSupported input extensions: {string.Join(", ", loader.GetSupportedExtensions())}";
                throw new ArgumentException(msg);
            }

            if (!exporter.IsSupportedFile(outputPath)) {
                var msg = $"Output file '{outputPath}' is not supported.\nSupported output extensions: {string.Join(", ", exporter.GetSupportedExtensions())}";
                throw new ArgumentException(msg);
            }

            var image = loader.FromFile(input);

            exporter.Export(image, outputPath);
        }
    }
}
