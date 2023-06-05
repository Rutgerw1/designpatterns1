using sudoku.View;
using sudoku.View.States;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	class Puzzle : Composite
	{
		public Point Cursor { get; set; }
		public IViewState State {get; private set;}

		public int Size { get; }

		public Puzzle(int size)
		{
			Cursor = new Point(0, 0);
			State = new NormalViewState(this);

			Size = size;
		}

		public void ChangeState(IViewState state)
		{
			State = state;
		}

		public void TryMove(Direction direction, int depth = 0)
		{
			// recursion failsafe
			if (depth > 50) throw new ArgumentOutOfRangeException("Stuck in loop while moving.");

			Point nextCursor = Cursor;

			switch (direction)
			{
				case Direction.Up:
					nextCursor.Y--; break;
				case Direction.Right:
					nextCursor.X++; break;
				case Direction.Down:
					nextCursor.Y++; break;
				case Direction.Left:
					nextCursor.X--; break;
			}

			// cases for Out Of Bounds (loop around)
			if (nextCursor.X >= Size) nextCursor.X = 0;
			else if (nextCursor.X < 0) nextCursor.X = Size - 1;
			else if (nextCursor.Y >= Size) nextCursor.Y = 0;
			else if (nextCursor.Y < 0) nextCursor.Y = Size - 1;

			Cursor = nextCursor;

			// recursively move again if Cursor is not pointing at a cell (gaps in samurai)
			if (!Contains(Cursor)) TryMove(direction, depth + 1);
		}

		public void ChangeValueAtCursor(int value)
		{
			if (State is NotesViewState)
			{
				ToggleNoteAtPosition(value, Cursor);
			}
			else
			{
				ChangeValueAtPosition(value, Cursor);
			}
		}

		public Point? FirstEmptyCellPosition()
		{
			for (int j = 0; j < Size; j++)
			{
				for (int i = 0; i < Size; i++)
				{
					Cell cell = CellAtPosition(new Point(i, j));
					if (cell != null && cell.Value == 0)
					{
						return cell.Position;
					}
				}
			}

			// no empty cells
			return null;
		}
	}
}
