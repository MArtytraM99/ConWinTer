using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ConWinTer.Loader {
    public interface IImageLoader {
        public abstract Image FromFile(string path);

        public bool IsSupportedFile(string path);

        public IEnumerable<string> GetSupportedExtensions();
    }
}
