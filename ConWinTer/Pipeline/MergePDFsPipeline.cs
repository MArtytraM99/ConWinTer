using ConWinTer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace ConWinTer.Pipeline
{
    public class MergePDFsPipeline
    {
        public static List<string> SUPPORTED_EXTENSIONS = new List<string> { "pdf" };

        public MergePDFsPipeline()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public void Run(IEnumerable<string> input, string output)
        {
            if (input == null || input.Count() <= 1)
            {
                throw new ArgumentException("Input must include at least 2 files to merge");
            }

            var nonExistent = input.Where(s => !File.Exists(s));
            if (nonExistent.Any())
            {
                throw new ArgumentException($"File(s) '{string.Join(',', nonExistent)}' don't exist");
            }
            
            var notSupported = input.Where(s => !PathUtils.HasExtension(s, SUPPORTED_EXTENSIONS));
            if (notSupported.Any())
            {
                throw new ArgumentException($"File(s) '{string.Join(',', notSupported)}' are not supported");
            }

            if (string.IsNullOrEmpty(output))
            {
                // same folder as first input
                var dirName = Path.GetDirectoryName(input.First());
                output = Path.Combine(dirName, "merged.pdf");
            }

            if (string.IsNullOrEmpty(Path.GetExtension(output)))
            {
                output = Path.ChangeExtension(output, ".pdf");
            }

            if (!PathUtils.HasExtension(output, SUPPORTED_EXTENSIONS))
            {
                throw new ArgumentException($"Output path '{output}' has unsupported extension");
            }

            var pdfs = input.Select(path => PdfReader.Open(path, PdfDocumentOpenMode.Import));
            var resultPdf = new PdfDocument();

            foreach (var pdf in pdfs)
            {
                foreach (var page in pdf.Pages)
                {
                    resultPdf.AddPage(page);
                }
            }

            resultPdf.Save(output);
        }

        public int RunWithExceptionHandling(IEnumerable<string> input, string output)
        {
            try
            {
                Run(input, output);
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return 1;
            }
        }
    }
}
