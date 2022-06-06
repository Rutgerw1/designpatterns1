using sudoku.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Reader
{
    interface ISudokuReader
    {
        Puzzle CreatePuzzle(string file);
    }
}
