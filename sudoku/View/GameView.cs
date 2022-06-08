using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	class GameView : IView
	{
		private Puzzle _puzzle;

		public GameView(Puzzle puzzle)
		{
			_puzzle = puzzle;
		}

		public void PrintMessage(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
		{
			Console.ForegroundColor = foregroundColor;
			Console.BackgroundColor = backgroundColor;
			Console.Write(message);
		}

		public void PrintGame()
		{
			Console.Clear();
			for (int i = 0; i < _puzzle.Rows.Length; i++)
			{
				PrintRow(_puzzle.Rows[i], i);
			}
			PrintRowSeparator(_puzzle.Rows[0].Cells.Length, _puzzle.Rows.Length);
			Console.WindowHeight = _puzzle.Rows.Length * 2 + 1;
			Console.WindowWidth = _puzzle.Columns.Length * 4 + 3;
			Console.SetCursorPosition(0, 0);
		}

		public void PrintRow(Row row, int currentRow)
		{
			PrintRowSeparator(row.Cells.Length, Array.IndexOf(_puzzle.Rows, row));
			for (int i = 0; i < row.Cells.Length; i++)
			{
				Cell previousCell = i > 0 ? row.Cells[i - 1] : null;
				Cell currentCell = row.Cells[i];

				ConsoleColor color = ConsoleColor.White;
				if (IsSameRegion(previousCell, currentCell))
				{
					color = ConsoleColor.DarkBlue;
				}
				// only print cell separators if at least 1 of them exists AND is active
				bool printCellSeparator =
					(previousCell != null && previousCell.IsActive) ||
					(currentCell != null && currentCell.IsActive);

				PrintMessage(printCellSeparator ? " | " : "   ", color);

				ConsoleColor bgColor = ConsoleColor.Black;
				if (_puzzle.Location.Y == currentRow && _puzzle.Location.X == i)
				{
					bgColor = ConsoleColor.DarkYellow;
				}
				PrintMessage(currentCell.ToString(), backgroundColor: bgColor);
				if (i == row.Cells.Length - 1)
				{
					PrintMessage(printCellSeparator ? " |\n" : "  \n");
				}
			}
		}

		private void PrintRowSeparator(int length, int rowNumber)
		{
			PrintMessage(" ");
			for (int i = 0; i < length; i++)
			{
				Cell cell1 = null;
				Cell cell2 = null;
				// these are needed for determining whether a '+' should be drawn
				Cell cell3 = null;
				Cell cell4 = null;
				if (rowNumber > 0)
				{
					cell1 = _puzzle.Rows[rowNumber - 1].Cells[i];
					if (i > 0)
					{
						cell3 = _puzzle.Rows[rowNumber - 1].Cells[i - 1];
					}
				}
				if (rowNumber < _puzzle.Rows.Length)
				{
					cell2 = _puzzle.Rows[rowNumber].Cells[i];
					if (i > 0)
					{
						cell4 = _puzzle.Rows[rowNumber].Cells[i - 1];
					}
				}
				// only print cell separators if at least 1 of them exists AND is active
				bool printCellSeparator =
					(cell1 != null && cell1.IsActive) ||
					(cell2 != null && cell2.IsActive);

				bool printCellCrossing = printCellSeparator ||
					(cell3 != null && cell3.IsActive) ||
					(cell4 != null && cell4.IsActive);

				ConsoleColor color = !IsSameRegion(cell1, cell2) ? ConsoleColor.White : ConsoleColor.DarkBlue;
				PrintMessage(printCellCrossing ? "+" : " ");
				PrintMessage(printCellSeparator ? "---" : "   ", color);
				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}

		internal void RePrintCells((int X, int Y)[] locations)
		{
			foreach ((int X, int Y) in locations)
			{
				Console.CursorTop = Y * 2 + 1;
				Console.CursorLeft = X * 4 + 3;
				if (_puzzle.Location.Y == Y && _puzzle.Location.X == X)
				{
					PrintMessage(_puzzle.Rows[Y].Cells[X].ToString(), backgroundColor: ConsoleColor.DarkYellow);
				}
				else
				{
					PrintMessage(_puzzle.Rows[Y].Cells[X].ToString());
				}
				Console.CursorLeft--;
			}
		}

		private bool IsSameRegion(Cell cell1, Cell cell2)
		{
			return cell1?.Region == cell2?.Region;
		}
	}
}
