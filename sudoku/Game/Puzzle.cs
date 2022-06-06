using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Puzzle
    {
        private Row[] _rows;
		public Row[] Rows { get => _rows; }

		private Column[] _columns;
		public Column[] Columns { get => _columns; }

		private Region[] _regions;
		public Region[] Regions { get => _regions; }

		public Puzzle(Row[] rows, Column[] columns, Region[] regions)
		{
			_rows = rows;
			_columns = columns;
			_regions = regions;
		}

	}
}
