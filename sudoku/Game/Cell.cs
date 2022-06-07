using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Cell
    {
        private int? _number;
        public int? Number { get => _number; set => _number = value; }

        private int[] _notes;
        public int[] Notes { get => _notes; set => _notes = value; }

		private Region _region;
		public Region Region { get => _region; set => _region = value; }

		public Cell(int number)
		{
			_number = number;
		}

		public override string ToString()
		{
			return _number != 0 ? _number.ToString() : " ";
		}
	}
}
