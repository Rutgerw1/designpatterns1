using sudoku.Game;
using System;
using System.Drawing;

namespace sudoku.View.States
{
	internal class NotesViewState : GameView
	{
		private int NotesWidth { get; }
		private int NotesHeight { get; }
		public override int ReprintFactorX { get; }
		public override int ReprintFactorY { get; }

		public NotesViewState(Puzzle puzzle) : base(puzzle)
		{
			if (Puzzle.Components[0].GetType() == typeof(Puzzle))
			{ // samurai
				NotesWidth = 3;
				NotesHeight = 3;
			}
			else
			{
				NotesWidth = (int)Math.Sqrt(Puzzle.Size);
				NotesHeight = Puzzle.Size / NotesWidth;
			}
			ReprintFactorX = 3 + NotesWidth;
			ReprintFactorY = 1 + NotesHeight;
		}

		public override void PrintRow(int y)
		{
			PrintRowSeparator(Puzzle.Size, y);
			for (int x = 0; x < Puzzle.Size; x++)
			{
				Console.SetCursorPosition(x * ReprintFactorX, y * ReprintFactorY + 1);
				PrintCell(new Point(x, y));
			}
		}

		public override void PrintCell(Point pos)
		{
			// noteY denotes the relative vertical position of a note within a cell
			for (int noteY = 0; noteY < NotesHeight; noteY++)
			{
				Console.SetCursorPosition(pos.X * ReprintFactorX, pos.Y * ReprintFactorY + noteY + 1);
				PrintCellNotesRow(pos, noteY);
			}
		}

		private void PrintCellNotesRow(Point pos, int noteY)
		{
			Cell previousCell = Puzzle.CellAtPosition(new Point(pos.X - 1, pos.Y));
			Cell currentCell = Puzzle.CellAtPosition(pos);
			ConsoleColor bgColor = BG_BASE;

			if (currentCell?.Conflicts.Count > 0)
			{
				bgColor = CONFLICT;
			}
			ConsoleColor color = FG_BASE;

			if (AllSameRegion(previousCell, currentCell))
			{
				color = SAME_REGION;
			}

			// only print cell separators if at least 1 of them exists
			bool printCellSeparator = previousCell != null || currentCell != null;
			PrintMessage(printCellSeparator ? " | " : "   ", color);

			if (Puzzle.Cursor.Equals(pos))
			{
				bgColor = CURSOR;
			}

			for (int noteX = 0; noteX < NotesWidth; noteX++)
			{
				PrintNote(noteX, noteY, currentCell, bgColor);
			}
			if (pos.X == Puzzle.Size - 1)
			{
				PrintMessage(printCellSeparator ? " |\n" : "  \n");
			}
		}

		private void PrintNote(int noteX, int noteY, Cell currentCell, ConsoleColor bgColor)
		{
			string message = " ";
			if (currentCell?.Value == 0)
			{
				int note = noteY * NotesWidth + noteX + 1;
				if (currentCell.Notes.Contains(note))
				{
					message = note.ToString();
				}
			}
			else if (noteX == NotesWidth / 2 && noteY == NotesHeight / 2)
			{
				message = currentCell?.Value.ToString();
			}
			PrintMessage(message, backgroundColor: bgColor);
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
				PrintMessage(printCellSeparator ? new string('-', NotesWidth + 2) : new string(' ', NotesWidth + 2), color);

				if (i == length - 1)
				{
					PrintMessage(printCellCrossing ? "+\n" : " \n");
				}
			}
		}
	}
}
