using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	abstract class GameView : IGameView
	{
		public readonly Puzzle _puzzle;
		public abstract int ReprintFactorX { get; }
		public abstract int ReprintFactorY { get; }

		public GameView(Puzzle puzzle)
		{
			_puzzle = puzzle;
		}

		public abstract void RePrintCells(List<(int X, int Y)> redrawLocations);
		public abstract void PrintRow(Group row, int currentRow);
		public abstract void PrintRowSeparator(int length, int rowNumber);
		public abstract void PrintCell(Group row, int currentRow, int cellIndex);

		public void PrintMessage(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
		{
			Console.ForegroundColor = foregroundColor;
			Console.BackgroundColor = backgroundColor;
			Console.Write(message);
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
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
			Console.SetCursorPosition(_puzzle.Columns.Count * ReprintFactorX + 2, _puzzle.Rows.Count * ReprintFactorY / 2 - 2);
			PrintMessage("  Quit game: ");
			PrintMessage("Esc", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * ReprintFactorX + 2, _puzzle.Rows.Count * ReprintFactorY / 2 - 1);
			PrintMessage("  Clear cell: ");
			PrintMessage("Delete", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * ReprintFactorX + 2, _puzzle.Rows.Count * ReprintFactorY / 2);
			PrintMessage("  Switch modes: ");
			PrintMessage("Space", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * ReprintFactorX + 2, _puzzle.Rows.Count * ReprintFactorY / 2 + 1);
			PrintMessage("  Check: ");
			PrintMessage("C", ConsoleColor.Magenta);

			Console.SetCursorPosition(_puzzle.Columns.Count * ReprintFactorX + 2, _puzzle.Rows.Count * ReprintFactorY / 2 + 2);
			PrintMessage("  Solve: ");
			PrintMessage("S", ConsoleColor.Magenta);
		}

		public void ClearErrorMessage()
		{
			Console.SetCursorPosition(3, _puzzle.Rows.Count * ReprintFactorY + 2);
			PrintMessage(new string(' ', Console.BufferWidth));
			Console.SetCursorPosition(3, _puzzle.Rows.Count * ReprintFactorY + 2);
		}

		public void PrintErrorsPresent()
		{
			ClearErrorMessage();
			PrintMessage("Errors are present, highlighted in red.");
		}

		public void PrintNoErrorsPresent()
		{
			ClearErrorMessage();
			PrintMessage("Currently no errors in puzzle.");
		}

		public void PrintFinish()
		{
			ClearErrorMessage();
			PrintMessage("Game over. Thanks for playing!");
			Console.ReadKey(true);
		}

		public void PrintUnsolvable()
		{
			ClearErrorMessage();
			PrintMessage("The current state of this puzzle is unsolvable, but there are no immediate conflicts.");
		}

		public bool AreSameRegion(Cell[] cells)
		{
			return cells.Length <= 1 ||
				(
					cells[0]?.Region == cells[1]?.Region &&
					AreSameRegion(new ArraySegment<Cell>(cells, 1, cells.Length - 1).ToArray())
				);
		}
	}
}
