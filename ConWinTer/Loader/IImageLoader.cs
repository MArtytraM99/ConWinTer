using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace ConWinTer.Loader {
    public interface IImageLoader {
        Image FromFile(string path);
    }
}
