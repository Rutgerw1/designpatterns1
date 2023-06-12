using System.Collections.Generic;
using System.Drawing;

namespace sudoku.Game
{
	public class Cell : ISudokuComponent
	{
		public Point Position { get; set; }
		public int Value { get; set; }
		public int RegionNumber { get; }
		public List<int> Notes { get; }
		public List<Cell> Conflicts { get;}

		public Cell(Point position, int value, int regionCounter)
		{
			Position = position;
			Value = value;
			RegionNumber = regionCounter;
			Notes = new List<int>();
			Conflicts = new List<Cell>();
		}

		public bool IsValid(int maxNumber)
		{
			if (Value > maxNumber)
			{
				if (!Conflicts.Contains(this)) Conflicts.Add(this);
				return false;
			}
			return true;
		}

		public void ChangeValueAtPosition(int value, Point position)
		{
			if (Contains(position))
			{
				Value = value;
				Conflicts.ForEach(conflict => {
					if (conflict != this) conflict.Conflicts.Remove(this);
				});
				Conflicts.Clear();
			}
		}

		public void ToggleNoteAtPosition(int value, Point position)
		{
			if (Contains(position))
			{
				if (Notes.Contains(value))
				{
					Notes.Remove(value);
				}
				else
				{
					Notes.Add(value);
				}
			}
		}

		public Cell CellAtPosition(Point position)
		{
			if (Contains(position)) return this;
			return null;
		}

		public bool Contains(Point point)
		{
			return point.Equals(Position);
		}

		public override string ToString()
		{
			return Value != 0 ? Value.ToString() : " ";
		}

		public void Offset(Point offset)
		{
			Position = new Point(Position.X + offset.X, Position.Y + offset.Y);
		}
	}
}
