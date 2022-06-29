using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	public class Puzzle
	{
		private readonly List<Group> _rows;
		public List<Group> Rows { get => _rows; }

		private readonly List<Group> _columns;
		public List<Group> Columns { get => _columns; }

		private readonly List<Region> _regions;
		public List<Region> Regions { get => _regions; }

		private readonly List<Puzzle> _subPuzzles;
		public List<Puzzle> SubPuzzles { get => _subPuzzles; }

		private (int X, int Y) _location = (0, 0);
		public (int X, int Y) Location { get => _location; }
		public Cell SelectedCell { get => _rows[_location.Y].Cells[_location.X]; }

        private bool _notesMode;
		public bool NotesMode { get => _notesMode; }

		public int MaxNumber { get => SubPuzzles == null ? Rows.Count : SubPuzzles[0].Rows.Count; }

		public Puzzle(List<Group> rows, List<Group> columns, List<Region> regions, List<Puzzle> subPuzzles = null)
		{
			_rows = rows;
			_columns = columns;
			_regions = regions;
			_subPuzzles = subPuzzles;
			_notesMode = false;
		}

		public void TryMove(Direction direction)
		{
			int nextX = _location.X;
			int nextY = _location.Y;

			switch (direction)
			{
				case Direction.Up:
					nextY = _location.Y - 1;
					break;
				case Direction.Right:
					nextX = _location.X + 1;
					break;
				case Direction.Down:
					nextY = _location.Y + 1;
					break;
				case Direction.Left:
					nextX = _location.X - 1;
					break;
			}
			// loop back to other side
			if (nextX == _columns.Count) nextX = 0;
			if (nextX == -1) nextX = _columns.Count - 1;
			if (nextY == _rows.Count) nextY = 0;
			if (nextY == -1) nextY = _rows.Count - 1;

			_location.X = nextX;
			_location.Y = nextY;

			// recursively move again if new location is inactive cell
			if (!_rows[nextY].Cells[nextX].IsActive)
			{
				TryMove(direction);
			}
		}

		public bool ChangeCellValue(int number)
		{
			Cell cell = _rows[_location.Y].Cells[_location.X];
			{
				if (_notesMode)
				{
					if (cell.Notes.Contains(number))
					{
						cell.Notes.Remove(number);
					}
					else
					{
						cell.Notes.Add(number);
					}
					return false;
				}
				else
				{
					bool hadError = cell.Conflicts.Count > 0 || cell.OutOfBounds;
					if (cell.Number == number)
					{
						cell.Number = 0;
					}
					else
					{
						cell.Number = number;
					}
					return hadError;
				}
			}
		}

		public (int, int)? FirstEmptyCellLocation()
		{
			for (int y = 0; y < _rows.Count; y++)
			{
				for (int x = 0; x < _rows[0].Cells.Count; x++)
				{
					Cell cell = _rows[y].Cells[x];
					if (cell.Number == 0 && cell.IsActive) return (x, y);
				}
			}
			return null;
		}

		public void ToggleNotesMode()
        {
			_notesMode = !_notesMode;
        }

		public (int, int)? GetCellLocation(Cell cell)
		{
			for (int y = 0; y < _rows.Count; y++)
			{
				for (int x = 0; x < _rows[0].Cells.Count; x++)
				{
					if (cell == _rows[y].Cells[x]) return (x, y);
				}
			}
			return null;
		}
	}
}
