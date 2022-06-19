using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	public class Cell
	{
		private int _number;
		public int Number { get => _number; set => _number = value; }

		private readonly bool _isActive;
		public bool IsActive { get => _isActive; }

		private List<Cell> _conflicts;
		public List<Cell> Conflicts { get => _conflicts; set => _conflicts = value; }

		private bool _outOfBounds;
		public bool OutOfBounds { get => _outOfBounds; set => _outOfBounds = value; }

		private List<int> _notes;
		public List<int> Notes { get => _notes; set => _notes = value; }

		private Region _region;
		public Region Region { get => _region; set => _region = value; }

		public Cell(int number, bool active = true)
		{
			_number = number;
			_isActive = active;
			_conflicts = new List<Cell>();
		}

		public override string ToString()
		{
			return _number != 0 ? _number.ToString() : " ";
		}
	}
}
