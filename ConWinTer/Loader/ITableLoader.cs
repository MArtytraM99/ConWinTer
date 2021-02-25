using ConWinTer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConWinTer.Loader {
    public interface ITableLoader {
        public Table FromFile(string path);
    }
}
