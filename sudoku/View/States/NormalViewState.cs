using sudoku.Game;
using System;
using System.Drawing;

namespace sudoku.View.States
{
	internal class NormalViewState : GameView
	{
		public override int ReprintFactorX { get; }
		public override int ReprintFactorY { get; }

		public NormalViewState(Puzzle puzzle) : base(puzzle)
		{
			ReprintFactorX = 4;
			ReprintFactorY = 2;

			FitConsole();
		}

		public override void PrintCell(Point pos)
		{
			Cell previousCell = Puzzle.CellAtPosition(new Point(pos.X - 1, pos.Y));
			Cell currentCell = Puzzle.CellAtPosition(pos);

			ConsoleColor color = ConsoleColor.White;
			if (AllSameRegion(previousCell, currentCell))
			{
				color = ConsoleColor.DarkBlue;
			}

			// only print cell separators if at least 1 of them exists AND is active
			bool printCellSeparator = previousCell != null || currentCell != null;
			PrintMessage(printCellSeparator ? " | " : "   ", color);

			ConsoleColor bgColor = ConsoleColor.Black;
			if (currentCell?.Conflicts.Count > 0)
			{
				bgColor = ConsoleColor.Red;
			}
			if (Puzzle.Cursor.Equals(pos))
			{
				bgColor = ConsoleColor.DarkYellow;
			}
			string message = currentCell?.ToString() ?? " ";
			PrintMessage(message, backgroundColor: bgColor);
			if (pos.X == Puzzle.Size - 1)
			{
				PrintMessage(printCellSeparator ? " |\n" : "  \n");
			}
		}

		public override void PrintRow(int y)
		{
			PrintRowSeparator(Puzzle.Size, y);
			for (int x = 0; x < Puzzle.Size; x++)
			{
				PrintCell(new Point(x, y));
			}
		}

		public override void PrintRowSeparator(int length, int rowNumber)
		{
			PrintMessage(" ");
			for (int i = 0; i < length; i++)
			{
				// we need some info on the surrounding cells to determine the kind of separator character
				Cell cell1 = Puzzle.CellAtPosition(new Point(rowNumber - 1, i));
				Cell cell2 = Puzzle.CellAtPosition(new Point(rowNumber, i));
				Cell cell3 = Puzzle.CellAtPosition(new Point(rowNumber - 1, i - 1));
				Cell cell4 = Puzzle.CellAtPosition(new Point(rowNumber, i - 1));

				// only print cell separators if at least 1 of them exists
				bool printCellSeparator = cell1 != null || cell2 != null;
				bool printCellCrossing = printCellSeparator || cell3 != null || cell4 != null;

				ConsoleColor color = AllSameRegion(cell1, cell2, cell3, cell4) ? ConsoleColor.DarkBlue : ConsoleColor.White;
				PrintMessage(printCellCrossing ? "+" : " ", color);
				color = AllSameRegion(cell1, cell2) ? ConsoleColor.DarkBlue : ConsoleColor.White;
				PrintMessage(printCellSeparator ? "---" : "   ", color);

				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}
	}
}
