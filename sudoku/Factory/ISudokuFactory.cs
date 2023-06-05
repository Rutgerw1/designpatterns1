using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Factory
{
    interface ISudokuFactory
    {
        Puzzle CreatePuzzle(string file, Point offset = default);
    }
}
