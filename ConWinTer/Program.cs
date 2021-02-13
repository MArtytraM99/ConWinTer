using ConWinTer.Loader;
using ConWinTer.Utils;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Drawing.Imaging;
using System.IO;

namespace ConWinTer {
    class Program {
        static void Main(string[] args) {
            var rootCommand = ConfigureCommandLineOptions();

            rootCommand.InvokeAsync(args);
        }

        static void Run(string input, string output, string outputFormat) {
            if (!File.Exists(input)) {
                Console.Error.WriteLine($"Input file in path '{input}' doesn't exist.");
                return;
            }
            
            if (string.IsNullOrEmpty(output)) {
                output = Path.ChangeExtension(input, outputFormat);
            } else if (!Directory.Exists(Path.GetDirectoryName(output))) {
                Console.Error.WriteLine($"Directory in path '{Path.GetDirectoryName(output)}' doesn't exist.");
                return;
            }

            var loader = new BasicImageLoader();

            var image = loader.FromFile(input);

            image.Save(output);
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
