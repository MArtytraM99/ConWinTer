using System;
using System.Collections.Generic;
using System.Text;

namespace ConWinTer.Model {
    public class Table {
        public int Rows { get => data.GetLength(0); }
        public int Cols { get => data.GetLength(1); }

        public string[,] As2DArray { get => data; }
        public string[][] AsRowsArray {
            get {
                string[][] rows = new string[data.GetLength(0)][];
                for (int i = 0; i < data.GetLength(0); i++) {
                    rows[i] = new string[data.GetLength(1)];
                    for (int j = 0; j < data.GetLength(1); j++)
                        rows[i][j] = data[i, j] == null ? "" : (string)data[i, j].Clone();
                }

                return rows;
            }
        }
        public string[][] AsColsArray { 
            get {
                string[][] cols = new string[data.GetLength(1)][];
                for (int i = 0; i < data.GetLength(1); i++) {
                    cols[i] = new string[data.GetLength(0)];
                    for (int j = 0; j < data.GetLength(0); j++)
                        cols[i][j] = data[j, i] == null ? "" : (string)data[j, i].Clone();
                }
                    
                return cols;
            }
        }

        public string Name { get; }

        private readonly string[,] data;

        public Table(string[,] data, string name = "") {
            this.data = data;
            Name = name;
        }

        public Table() : this(new string[0, 0]) {} // empty table
    }
}
