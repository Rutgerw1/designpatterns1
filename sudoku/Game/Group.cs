using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Group
    {
        private Cell[] _cells;
		public Cell[] Cells { get => _cells; }

		public Group(Cell[] cells)
		{
			_cells = cells;
		}
	}
}
