﻿using ConWinTer.Export;
using ConWinTer.Loader;
using ConWinTer.Pipeline;
using ConWinTer.Utils;
using System;
using System.Collections.Generic;
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
        private static ExcelPipeline excelPipeline;
        private static MergePDFsPipeline mergePDFsPipeline;
        static async Task<int> Main(string[] args) {
            Configure();
            var rootCommand = ConfigureCommandLineOptions();

            return await rootCommand.InvokeAsync(args);
        }

        private static void Configure() {
            var compositeLoader = new CompositeImageLoader();

            compositeLoader.RegisterLoader(new BasicImageLoader());
            compositeLoader.RegisterLoader(new SvgImageLoader());
            compositeLoader.RegisterLoader(new NetpbmImageLoader());

            var compositeExporter = new CompositeImageExporter();

            compositeExporter.RegisterExporter(new BasicImageExporter());
            compositeExporter.RegisterExporter(new IconImageExporter());

            imagePipeline = new ImagePipeline(compositeLoader, compositeExporter);

            excelPipeline = new ExcelPipeline(new ClosedXMLExcelLoader(), new ExtensionBasedSeparatorCsvExporter());

            mergePDFsPipeline = new MergePDFsPipeline();
        }

        private static RootCommand ConfigureCommandLineOptions() {
            var rootCommand = new RootCommand("Converts files from one format to another");

            var inputPathOpt = new Option<string>(
                new string[] { "--input", "-i" },
                "Path to file that should be converted"
            );
            inputPathOpt.IsRequired = true;

            var outputPathOpt = new Option<string>(
                new string[] { "--output", "-o" },
                "Path where conveted file should be saved. If left empty same path as input will be taken."
            );

            var outputFormatOpt = new Option<string>(
                new string[] { "--output-format", "-f" },
                "Format in which converted file should be saved. If left empty it will be deduced from output path."
            );

            var imageCommand = new Command("image", "Command for converting images");
            imageCommand.AddOption(inputPathOpt);
            imageCommand.AddOption(outputPathOpt);
            imageCommand.AddOption(outputFormatOpt);
            imageCommand.Handler = CommandHandler.Create<string, string, string>(imagePipeline.RunWithExceptionHandling);

            var excelCommand = new Command("excel", "Command for converting excel files");
            excelCommand.AddOption(inputPathOpt);
            excelCommand.AddOption(outputPathOpt);
            excelCommand.AddOption(outputFormatOpt);
            excelCommand.Handler = CommandHandler.Create<string, string, string>(excelPipeline.RunWithExceptionHandling);

            var mergePdfsCommand = new Command("merge_pdfs", "Command for merging PDFs");
            var pdfsInputOption = new Option<List<string>>(new string[] { "--input", "-i" }, "Path to input pdfs");
            pdfsInputOption.AllowMultipleArgumentsPerToken = true;
            mergePdfsCommand.AddOption(pdfsInputOption);
            mergePdfsCommand.AddOption(outputPathOpt);
            mergePdfsCommand.Handler = CommandHandler.Create<List<string>, string>(mergePDFsPipeline.RunWithExceptionHandling);

            rootCommand.TreatUnmatchedTokensAsErrors = true;
            rootCommand.AddCommand(imageCommand);
            rootCommand.AddCommand(excelCommand);
            rootCommand.AddCommand(mergePdfsCommand);

            return rootCommand;
        }
    }
}
