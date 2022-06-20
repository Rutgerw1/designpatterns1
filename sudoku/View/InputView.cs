using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	class InputView : GameView
	{
		private readonly int _reprintFactorX;
		private readonly int _reprintFactorY;
		public override int ReprintFactorX { get => _reprintFactorX; }
		public override int ReprintFactorY { get => _reprintFactorY; }

		public InputView(Puzzle puzzle) : base(puzzle)
		{
			_reprintFactorX = 4;
			_reprintFactorY = 2;
		}

		public override void PrintRow(Group row, int currentRow)
		{
			PrintRowSeparator(row.Cells.Count, _puzzle.Rows.IndexOf(row));
			for (int i = 0; i < row.Cells.Count; i++)
			{
				PrintCell(row, currentRow, i);
			}
		}

		public override void PrintCell(Group row, int currentRow, int cellIndex)
		{
			Cell previousCell = cellIndex > 0 ? row.Cells[cellIndex - 1] : null;
			Cell currentCell = row.Cells[cellIndex];

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
			if (_puzzle.Location.Y == currentRow && _puzzle.Location.X == cellIndex)
			{
				bgColor = ConsoleColor.DarkYellow;
			}
			PrintMessage(currentCell.ToString(), backgroundColor: bgColor);
			if (cellIndex == row.Cells.Count - 1)
			{
				PrintMessage(printCellSeparator ? " |\n" : "  \n");
			}
		}

		public override void PrintRowSeparator(int length, int rowNumber)
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

		public override void RePrintCells(List<(int X, int Y)> locations)
		{
			foreach ((int X, int Y) in locations)
			{
				Console.CursorTop = Y * _reprintFactorY + 1;
				Console.CursorLeft = X * _reprintFactorX + 3;

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

				Console.CursorLeft--;
			}
		}
	}
}
