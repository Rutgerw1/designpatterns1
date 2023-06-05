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

			ConsoleColor color = FG_BASE;
			if (AllSameRegion(previousCell, currentCell))
			{
				color = SAME_REGION;
			}

			// only print cell separators if at least 1 of them exists
			bool printCellSeparator = previousCell != null || currentCell != null;
			PrintMessage(printCellSeparator ? " | " : "   ", color);

			ConsoleColor bgColor = BG_BASE;
			if (currentCell?.Conflicts.Count > 0)
			{
				bgColor = CONFLICT;
			}
			if (Puzzle.Cursor.Equals(pos))
			{
				bgColor = CURSOR;
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

				ConsoleColor color = AllSameRegion(cell1, cell2, cell3, cell4) ? SAME_REGION : FG_BASE;
				PrintMessage(printCellCrossing ? "+" : " ", color);
				color = AllSameRegion(cell1, cell2) ? SAME_REGION : FG_BASE;
				PrintMessage(printCellSeparator ? "---" : "   ", color);

				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}
	}
}
