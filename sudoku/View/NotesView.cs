using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
	class NotesView : GameView
	{
		private readonly int _sqrt;
		private readonly int _sqrtCeiling;
		private readonly int _reprintFactorX;
		private readonly int _reprintFactorY;
		public override int ReprintFactorX { get => _reprintFactorX; }
		public override int ReprintFactorY { get => _reprintFactorY; }

		public NotesView(Puzzle puzzle) : base(puzzle)
		{
			if (puzzle.SubPuzzles != null)
			{
				_sqrt = (int)Math.Sqrt(_puzzle.SubPuzzles[0].Rows.Count);
				_sqrtCeiling = _puzzle.SubPuzzles[0].Rows.Count / _sqrt;
			}
			else
			{
				_sqrt = (int)Math.Sqrt(_puzzle.Rows.Count);
				_sqrtCeiling = _puzzle.Rows.Count / _sqrt;
			}
			_reprintFactorY = 1 + _sqrt;
			_reprintFactorX = 3 + _sqrtCeiling;
		}

		public override void PrintRow(Group row, int currentRow)
		{
			PrintRowSeparator(row.Cells.Count, _puzzle.Rows.IndexOf(row));
			for (int i = 0; i < row.Cells.Count; i++)
			{
				Console.SetCursorPosition(i * _reprintFactorX, currentRow * _reprintFactorY + 1);
				PrintCell(row, currentRow, i);
			}
		}

		public override void PrintCell(Group row, int currentRow, int cellIndex)
		{
			for (int noteRowCount = 0; noteRowCount < _sqrt; noteRowCount++)
			{
				 Console.SetCursorPosition(cellIndex * _reprintFactorX, currentRow * _reprintFactorY + noteRowCount + 1);
				PrintCellNotesRow(row, currentRow, noteRowCount, cellIndex);
			}
		}

		private void PrintCellNotesRow(Group row, int currentRow, int noteRowCount, int cellIndex)
		{
			Cell previousCell = cellIndex > 0 ? row.Cells[cellIndex - 1] : null;
			Cell currentCell = row.Cells[cellIndex];
			ConsoleColor bgColor = ConsoleColor.Black;
			if (currentCell.Conflicts.Count > 0 || currentCell.OutOfBounds)
			{
				bgColor = ConsoleColor.Red;
			}
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
			if (_puzzle.Location.Y == currentRow && _puzzle.Location.X == cellIndex)
			{
				bgColor = ConsoleColor.DarkYellow;
			}

			for (int notesIndex = noteRowCount * _sqrtCeiling; notesIndex < _sqrtCeiling * (noteRowCount + 1); notesIndex++)
			{
				PrintNote(notesIndex, currentCell, bgColor);
				if (notesIndex == _sqrtCeiling * (noteRowCount + 1) - 1)
				{
					if (cellIndex == row.Cells.Count - 1)
					{
						PrintMessage(printCellSeparator ? " |" : "  ");
						if (noteRowCount == _sqrt - 1)
						{
							PrintMessage("\n");
						}
					}
				}
			}
		}

		private void PrintNote(int notesIndex, Cell currentCell, ConsoleColor bgColor)
		{
			string message = " ";
			if (currentCell.Number == 0)
			{
				int note = notesIndex + 1;
				if (currentCell.Notes.Contains(note))
				{
					message = note.ToString();
				}
			}
			else if (notesIndex == _sqrtCeiling + 1)
			{
				message = currentCell.Number.ToString();
			}
			PrintMessage(message, backgroundColor: bgColor);
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
				PrintMessage(printCellSeparator ? new string('-', _sqrtCeiling + 2) : new string(' ', _sqrtCeiling + 2), color);
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
				Cell cell = _puzzle.Rows[Y].Cells[X];
				Console.CursorTop = Y * _reprintFactorY + 1;
				Console.CursorLeft = X * _reprintFactorX + 3;
				if (_puzzle.Location.Y == Y && _puzzle.Location.X == X)
				{
					PrintBigCell(X, Y, _puzzle.Rows[Y].Cells[X], ConsoleColor.DarkYellow);
				}
				else if(cell.Conflicts.Count > 0 || cell.OutOfBounds)
				{
                    PrintBigCell(X, Y, _puzzle.Rows[Y].Cells[X], ConsoleColor.Red);
                }
				else
				{
					PrintBigCell(X, Y, _puzzle.Rows[Y].Cells[X]);
				}
				Console.CursorTop = Y * _reprintFactorY + 1;
				Console.CursorLeft = X * _reprintFactorX + 3;
			}
		}

		private void PrintBigCell(int X, int Y, Cell cell, ConsoleColor bgColor = ConsoleColor.Black)
		{
			for (int noteRowCount = 0; noteRowCount < _sqrt; noteRowCount++)
			{
				Console.SetCursorPosition(X * _reprintFactorX + 3, Y * _reprintFactorY + noteRowCount + 1);
				for (int notesIndex = noteRowCount * _sqrtCeiling; notesIndex < _sqrtCeiling * (noteRowCount + 1); notesIndex++)
				{
					PrintNote(notesIndex, cell, bgColor);
				}
			}
		}
	}
}
