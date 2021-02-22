using ConWinTer.Export;
using ConWinTer.Loader;
using ConWinTer.Pipeline;
using ConWinTer.Utils;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConWinTer {
    class Program {
        private static ImagePipeline imagePipeline;
        static async Task<int> Main(string[] args) {
            Configure();
            var rootCommand = ConfigureCommandLineOptions();

            return await rootCommand.InvokeAsync(args);
        }

        static int Run(string input, string output, string outputFormat) {
            try {
                imagePipeline.Run(input, output, outputFormat);
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }

        private static void Configure() {
            var compositeLoader = new CompositeImageLoader();

            compositeLoader.RegisterLoader(new BasicImageLoader());
            compositeLoader.RegisterLoader(new SvgImageLoader());
            compositeLoader.RegisterLoader(new NetpbmImageLoader());

            var exporter = new BasicImageExporter();

            imagePipeline = new ImagePipeline(compositeLoader, exporter);
        }

        private static RootCommand ConfigureCommandLineOptions() {
            var rootCommand = new RootCommand("Converts an image from one format to another");

            var inputPathOpt = new Option<string>(
                new string[] { "--input", "-i" },
                "Path to image that should be converted"
            );
            inputPathOpt.IsRequired = true;

            var outputPathOpt = new Option<string>(
                new string[] { "--output", "-o" },
                "Path where conveted image should be saved. If left empty same path as input will be taken."
            );

            var outputFormatOpt = new Option<string>(
                new string[] { "--output-format", "-f" },
                "Format in which converted image should be saved. If left empty it will be deduced from output path."
            );

            rootCommand.AddOption(inputPathOpt);
            rootCommand.AddOption(outputPathOpt);
            rootCommand.AddOption(outputFormatOpt);
            rootCommand.TreatUnmatchedTokensAsErrors = true;
            rootCommand.Handler = CommandHandler.Create<string, string, string>(Run);

            return rootCommand;
        }
    }
}
