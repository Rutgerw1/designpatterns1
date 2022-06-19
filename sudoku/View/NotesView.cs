using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
    class NotesView : IView
    {
		private readonly Puzzle _puzzle;
        private readonly int _reprintFactorX;
        private readonly int _reprintFactorY;
		private readonly int _sqrt;
		private readonly int _sqrtCeiling;
        private int _notesCounter;

		public NotesView(Puzzle puzzle)
        {
			_puzzle = puzzle;
			_reprintFactorY = 4;
			if(puzzle.SubPuzzles != null)
            {
				_sqrt = (int)Math.Sqrt(_puzzle.SubPuzzles[0].Rows[0].Cells.Count);
				_sqrtCeiling = (int)Math.Ceiling(Math.Sqrt(_puzzle.SubPuzzles[0].Rows[0].Cells.Count));
			}
			else
            {
				_sqrt = (int)Math.Sqrt(_puzzle.Rows[0].Cells.Count);
				_sqrtCeiling = (int)Math.Ceiling(Math.Sqrt(_puzzle.Rows[0].Cells.Count));
			}
			if (PuzzleLengthDivisibleBy2())
			{
				_reprintFactorX = 5;
			}
			else
			{
				_reprintFactorX = 6;
			}
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

		private void PrintInstructions() //TODO extract method to shared class
		{
            Console.SetCursorPosition(_puzzle.Columns.Count / 2, _puzzle.Rows.Count * 5 - 2);
            PrintMessage("  Quit game: ");
            PrintMessage("Esc", ConsoleColor.Magenta);

            Console.SetCursorPosition(_puzzle.Columns.Count / 2, _puzzle.Rows.Count * 5 - 1);
            PrintMessage("  Clear cell: ");
            PrintMessage("Delete", ConsoleColor.Magenta);

            Console.SetCursorPosition(_puzzle.Columns.Count / 2, _puzzle.Rows.Count * 5);
            PrintMessage("  Switch modes: ");
            PrintMessage("Space", ConsoleColor.Magenta);

            Console.SetCursorPosition(_puzzle.Columns.Count / 2, _puzzle.Rows.Count * 5 + 1);
            PrintMessage("  Check: ");
            PrintMessage("C", ConsoleColor.Magenta);

            Console.SetCursorPosition(_puzzle.Columns.Count / 2, _puzzle.Rows.Count * 5 + 2);
            PrintMessage("  Solve: ");
            PrintMessage("S", ConsoleColor.Magenta);
        }

		public void PrintRow(Row row, int currentRow)
		{
			PrintRowSeparator(row.Cells.Count, _puzzle.Rows.IndexOf(row));
			for (int consoleRowAmount = 0; consoleRowAmount < _sqrt; consoleRowAmount++)
			{
				SetNotesFromLine(consoleRowAmount); //Is dit clean? Denk het niet maar wat is het alternatief? Kent variabele niet overal in deze for loops
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
					if (AreSameRegion(new Cell[] { previousCell, currentCell }))
					{
						color = ConsoleColor.DarkBlue;
					}
					PrintMessage(printCellSeparator ? " | " : "   ", color);
					ConsoleColor bgColor = ConsoleColor.Black;
					if (_puzzle.Location.Y == currentRow && _puzzle.Location.X == i)
					{
						bgColor = ConsoleColor.DarkYellow;
					}
					
					//prints cell value
					for (int notesNumber = 0; notesNumber < _sqrtCeiling; notesNumber++)
					{
						if (currentCell.Number != 0)
                        {
							if(consoleRowAmount == 1)
                            {
								if(PuzzleLengthDivisibleBy2())
                                {
									PrintMessage(" " + currentCell.ToString(), backgroundColor: bgColor);
                                }
								else
                                {
									PrintMessage(" " + currentCell.ToString() + " ", backgroundColor: bgColor);
                                }
                            }
							else
                            {
								if (PuzzleLengthDivisibleBy2())
								{
									PrintMessage("  ", backgroundColor: bgColor);
								}
								else
								{
									PrintMessage("   ", backgroundColor: bgColor);
								}
								
							}
							break;
						}
						else if (currentCell.Notes.Contains(_notesCounter))
						{
							PrintMessage(_notesCounter.ToString(), backgroundColor: bgColor);
						}
						else
						{
							PrintMessage(" ", backgroundColor: bgColor);
						}
						_notesCounter++;
					}
					SetNotesFromLine(consoleRowAmount);
					if (i == row.Cells.Count - 1)
					{
						PrintMessage(printCellSeparator ? " |\n" : "  \n");
					}
				}
			}
		}

		private bool PuzzleLengthDivisibleBy2()
        {
			return _sqrtCeiling % 2 == 0;
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
				PrintMessage(printCellSeparator ? new string('-', _sqrtCeiling + 2) : new string(' ', _sqrtCeiling + 2), color);
				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}

		private void SetNotesFromLine(int consoleRow)
        {
			for (int i = 1; i < 4; i++)
			{
				if (consoleRow == 0)
				{
					_notesCounter = 1;
				}
				else
				{
					if (consoleRow == i)
					{
						_notesCounter = (int)Math.Ceiling(Math.Sqrt(_puzzle.Rows[0].Cells.Count) * i + 1);
					}
				}
			}
		}

        internal void RePrintCells((int X, int Y)[] locations)
        {
			foreach ((int X, int Y) in locations)
			{
				Console.CursorTop = Y * _reprintFactorY + 1;
				Console.CursorLeft = X * _reprintFactorX + 3;
				if (_puzzle.Location.Y == Y && _puzzle.Location.X == X)
				{
					PrintBigCell(X, Y, _puzzle.Rows[Y].Cells[X], ConsoleColor.DarkYellow);
				}
				else { 
					PrintBigCell(X, Y, _puzzle.Rows[Y].Cells[X]);
				}
				Console.CursorTop = Y * _reprintFactorY + 1;
				Console.CursorLeft = X * _reprintFactorX + 3;
			}
		}

		private void PrintBigCell(int X, int Y, Cell cell, ConsoleColor bgColor = ConsoleColor.Black)
        {
			SetNotesFromLine(0);
            for (int i = 0; i < _sqrt; i++)
            {
				Console.CursorTop = Y * ((int)Math.Sqrt(_puzzle.Rows[0].Cells.Count) + 1) + 1 + i;
                for (int j = 0; j < _sqrtCeiling; j++)
                {
					if (cell.Number != 0)
					{
						if (i == 1)
						{
							if (PuzzleLengthDivisibleBy2())
							{
								PrintMessage(" " + cell.ToString(), backgroundColor: bgColor);
							}
							else
							{
								PrintMessage(" " + cell.ToString() + " ", backgroundColor: bgColor);
							}
							
						}
						else
						{
							if (PuzzleLengthDivisibleBy2())
							{
								PrintMessage("  ", backgroundColor: bgColor);
							}
							else
							{
								PrintMessage("   ", backgroundColor: bgColor);
							}
						}
						break;
					}
					else if (cell.Notes.Contains(_notesCounter))
					{
						PrintMessage(_notesCounter.ToString(), backgroundColor: bgColor);
					}
					else
					{
						PrintMessage(" ", backgroundColor: bgColor);
					}
					_notesCounter++;
				}
				Console.BackgroundColor = ConsoleColor.Black;
				Console.CursorLeft = X * _reprintFactorX + 3;
			}
				Console.CursorLeft--;
			Console.CursorTop--;
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
