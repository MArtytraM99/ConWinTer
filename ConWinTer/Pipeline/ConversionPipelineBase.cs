using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConWinTer.Pipeline {
    public abstract class ConversionPipelineBase {
        /// <summary>
        /// Runs pipeline with parameters.
        /// </summary>
        /// <param name="input">Path to input file</param>
        /// <param name="output">Path to output file. If empty then input with path is used with corresponding extension based on <paramref name="outputFormat"/></param>
        /// <param name="outputFormat">Format of output file</param>
        public abstract void Run(string input, string output, string outputFormat);

        protected string GetOutputPath(string input, string output, string outputFormat) {
            if (string.IsNullOrEmpty(output))
                return Path.ChangeExtension(input, outputFormat);
            if (!string.IsNullOrEmpty(outputFormat))
                return Path.ChangeExtension(output, outputFormat);
            return output;
        }

        /// <summary>
        /// Runs method <see cref="Run(string, string, string)"/> and wraps it in try catch block. All exception messages are printed to Console.Error stream.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        public int RunWithExceptionHandling(string input, string output, string outputFormat) {
            try {
                Run(input, output, outputFormat);
                return 0;
            } catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return 1;
            }
        }
    }
}
