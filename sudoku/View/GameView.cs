using sudoku.Game;
using sudoku.View.States;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	abstract class GameView : IViewState
	{
		public Puzzle Puzzle { get; }
		public abstract int ReprintFactorX { get; }
		public abstract int ReprintFactorY { get; }

		public GameView(Puzzle puzzle)
		{
			Puzzle = puzzle;
		}

		public abstract void PrintRow(int y);
		public abstract void PrintRowSeparator(int length, int rowNumber);
		public abstract void PrintCell(Point pos);

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
			for (int y = 0; y < Puzzle.Size; y++)
			{
				PrintRow(y);
			}
			PrintRowSeparator(Puzzle.Size, Puzzle.Size);
			Console.SetCursorPosition(0, 0);
			PrintInstructions();
		}

		public void RePrintCells(List<Point> positions)
		{
			foreach (Point pos in positions)
			{
				Console.CursorTop = pos.Y * ReprintFactorY + 1;
				Console.CursorLeft = pos.X * ReprintFactorX;
				PrintCell(pos);
			}
		}

		protected bool AllSameRegion(params Cell[] cells)
		{
			List<int?> regions = cells.Select(cell => cell?.RegionNumber).ToList();
			return regions.Distinct().Count() == 1;
		}

		private void PrintInstructions()
		{
			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 - 3);
			PrintMessage("  Commands: ");

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 - 2);
			PrintMessage("  Quit game: ");
			PrintMessage("Esc", ConsoleColor.Magenta);

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 - 1);
			PrintMessage("  Clear cell: ");
			PrintMessage("Delete", ConsoleColor.Magenta);

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2);
			PrintMessage("  Switch modes: ");
			PrintMessage("Space", ConsoleColor.Magenta);

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 + 1);
			PrintMessage("  Check: ");
			PrintMessage("C", ConsoleColor.Magenta);

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 + 2);
			PrintMessage("  Solve: ");
			PrintMessage("S", ConsoleColor.Magenta);

			Console.SetCursorPosition(Puzzle.Size * ReprintFactorX + 2, Puzzle.Size * ReprintFactorY / 2 + 3);
			PrintMessage("  Won?: ");
			PrintMessage("Enter", ConsoleColor.Magenta);
		}

		public void ClearErrorMessage()
		{
			Console.SetCursorPosition(0, Puzzle.Size * ReprintFactorY + 2);
			PrintMessage(new string(' ', Console.BufferWidth - 1));
			Console.SetCursorPosition(3, Puzzle.Size * ReprintFactorY + 2);
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

		public void PrintCorrectWin()
		{
			ClearErrorMessage();
			PrintMessage("Congratulations, you completed this sudoku successfully!");
			Console.ReadKey(true);
		}

		public void PrintIncorrectWin()
		{
			ClearErrorMessage();
			PrintMessage("Something in this sudoku is not correct.");
		}

		public void FitConsole()
		{
			Console.SetWindowSize(1, 1);
			int width = Puzzle.Size * ReprintFactorX + 24;
			int height = Puzzle.Size * ReprintFactorY + 3;

			Console.SetBufferSize(width, height);
			if (height > 63)
			{
				height = 63;
			}
			Console.SetWindowSize(width, height);
		}
	}
}
