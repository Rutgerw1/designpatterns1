using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Puzzle
    {
        private Cell[] _cells;
        public Puzzle(Cell[] cells)
        {
            _cells = cells;
        }
    }
}
