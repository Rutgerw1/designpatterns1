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
		private readonly Puzzle _puzzle;

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
			Console.CursorVisible = false;
			for (int i = 0; i < _puzzle.Rows.Count; i++)
			{
				PrintRow(_puzzle.Rows[i], i);
			}
			PrintRowSeparator(_puzzle.Rows[0].Cells.Count, _puzzle.Rows.Count);
			Console.SetCursorPosition(0, 0);
			PrintInstructions();
		}

		private void PrintInstructions()
		{
			Console.SetCursorPosition(_puzzle.Columns.Count * 4 + 2, _puzzle.Rows.Count - 2);
			PrintMessage("  Quit game: ");
			PrintMessage("Esc", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * 4 + 2, _puzzle.Rows.Count - 1);
			PrintMessage("  Clear cell: ");
			PrintMessage("Delete", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * 4 + 2, _puzzle.Rows.Count);
			PrintMessage("  Switch modes: ");
			PrintMessage("Space", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * 4 + 2, _puzzle.Rows.Count + 1);
			PrintMessage("  Check: ");
			PrintMessage("C", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * 4 + 2, _puzzle.Rows.Count + 2);
			PrintMessage("  Solve: ");
			PrintMessage("S", ConsoleColor.Magenta);
		}

		public void PrintRow(Group row, int currentRow)
		{
			PrintRowSeparator(row.Cells.Count, _puzzle.Rows.IndexOf(row));
			for (int i = 0; i < row.Cells.Count; i++)
			{
				Cell previousCell = i > 0 ? row.Cells[i - 1] : null;
				Cell currentCell = row.Cells[i];

				ConsoleColor color = ConsoleColor.White;
				if (AreSameRegion(new Cell[] { previousCell, currentCell }))
				{
					color = ConsoleColor.DarkBlue;
				}
				// only print cell separators if at least 1 of them exists AND is active
				bool printCellSeparator =
					(previousCell != null && previousCell.IsActive) ||
					(currentCell != null && currentCell.IsActive);

				PrintMessage(printCellSeparator ? " | " : "   ", color);

				ConsoleColor bgColor = ConsoleColor.Black;
				if (currentCell.Conflicts.Count > 0 || currentCell.OutOfBounds)
				{
					bgColor = ConsoleColor.Red;
				}
				if (_puzzle.Location.Y == currentRow && _puzzle.Location.X == i)
				{
					bgColor = ConsoleColor.DarkYellow;
				}
				PrintMessage(currentCell.ToString(), backgroundColor: bgColor);
				if (i == row.Cells.Count - 1)
				{
					PrintMessage(printCellSeparator ? " |\n" : "  \n");
				}
			}
		}

		internal void ClearErrorMessage()
		{
			Console.SetCursorPosition(3, _puzzle.Rows.Count * 2 + 2);
			PrintMessage(new string(' ', Console.BufferWidth));
			Console.SetCursorPosition(3, _puzzle.Rows.Count * 2 + 2);
		}

		internal void PrintErrorsPresent()
		{
			ClearErrorMessage();
			PrintMessage("Errors are present, highlighted in red.");
		}

		internal void PrintNoErrorsPresent()
		{
			ClearErrorMessage();
			PrintMessage("Currently no errors in puzzle.");
		}

		internal void PrintFinish()
		{
			ClearErrorMessage();
			PrintMessage("Game over. Thanks for playing!");
			Console.ReadKey(true);
		}

		internal void PrintUnsolvable()
		{
			ClearErrorMessage();
			PrintMessage("The current state of this puzzle is unsolvable, but there are no immediate conflicts.");
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
				if (rowNumber < _puzzle.Rows.Count)
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

				ConsoleColor color = !AreSameRegion(new Cell[] { cell1, cell2, cell3, cell4 }) ? ConsoleColor.White : ConsoleColor.DarkBlue;
				PrintMessage(printCellCrossing ? "+" : " ", color);
				color = !AreSameRegion(new Cell[] { cell1, cell2 }) ? ConsoleColor.White : ConsoleColor.DarkBlue;
				PrintMessage(printCellSeparator ? "---" : "   ", color);
				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}

		internal void RePrintCells(List<(int X, int Y)> locations)
		{
			foreach ((int X, int Y) in locations)
			{
				Console.CursorTop = Y * 2 + 1;
				Console.CursorLeft = X * 4 + 3;

				Cell cell = _puzzle.Rows[Y].Cells[X];
				if (_puzzle.Location.Y == Y && _puzzle.Location.X == X)
				{
					PrintMessage(cell.ToString(), backgroundColor: ConsoleColor.DarkYellow);
				}
				else if (cell.Conflicts.Count > 0 || cell.OutOfBounds)
				{
					PrintMessage(cell.ToString(), backgroundColor: ConsoleColor.Red);
				}
				else
				{
					PrintMessage(cell.ToString());
				}
				Console.BackgroundColor = ConsoleColor.Black;
				Console.CursorLeft--;
			}
		}

		private bool AreSameRegion(Cell[] cells)
		{
			return cells.Length <= 1 ||
				(
					cells[0]?.Region == cells[1]?.Region &&
					AreSameRegion(new ArraySegment<Cell>(cells, 1, cells.Length - 1).ToArray())
				);
		}
	}
}
