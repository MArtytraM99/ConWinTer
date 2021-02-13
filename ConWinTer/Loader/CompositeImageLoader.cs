using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ConWinTer.Loader {
    public class CompositeImageLoader : IImageLoader {
        private Dictionary<string, IImageLoader> extensionLoaderMap;

        public CompositeImageLoader() {
            extensionLoaderMap = new Dictionary<string, IImageLoader>();
        }

        /// <summary>
        /// Registers <paramref name="loader"/> that will be used for loading images with <paramref name="extensions"/>
        /// </summary>
        /// <param name="extensions">Supported extensions</param>
        /// <param name="loader"></param>
        public void RegisterLoader(IEnumerable<string> extensions, IImageLoader loader) {
            foreach (string extension in extensions) {
                extensionLoaderMap[extension] = loader;
            }
        }

        /// <summary>
        /// Registers <paramref name="loader"/> that will be used for loading images with <paramref name="extension"/>
        /// </summary>
        /// <param name="extension">Supported extension</param>
        /// <param name="loader"></param>
        public void RegisterLoader(string extension, IImageLoader loader) {
            RegisterLoader(new List<string> { extension }, loader);
        }

        /// <summary>
        /// Registers <paramref name="loader"/> that will be used for loading images accepting all extensions that <paramref name="loader"/> supports.
        /// </summary>
        /// <param name="loader"></param>
        public void RegisterLoader(IImageLoader loader) {
            RegisterLoader(loader.GetSupportedExtensions(), loader);
        }

        public Image FromFile(string path) {
            string extension = Path.GetExtension(path);
            var loader = extensionLoaderMap[extension];

            if(loader == null) {
                throw new ArgumentException($"Unsupported extension '{extension}' for loading");
            }

            return loader.FromFile(path);
        }

        public IEnumerable<string> GetSupportedExtensions() {
            return extensionLoaderMap.Keys;
        }

        public bool IsSupportedFile(string path) {
            string extension = Path.GetExtension(path);
            return extensionLoaderMap.ContainsKey(extension);
        }
    }
}
