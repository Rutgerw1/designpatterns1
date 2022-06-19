using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Group : ICell
    {
        private readonly List<Cell> _cells;
		public List<Cell> Cells { get => _cells; }

		public Group(List<Cell> cells)
		{
			_cells = cells;
		}

		public override string ToString()
		{
			string result = "";
			_cells.ForEach(cell => result.Concat(cell.ToString()));
			return result;
		}
	}
}
