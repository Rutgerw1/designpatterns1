using sudoku.Game;
using sudoku.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.SolvingAlgorithm
{
	class BacktrackAlgorithm : ISolvingAlgorithm
	{
		public bool Solve(Puzzle puzzle, bool persist = true)
		{
			(int X, int Y)? location = puzzle.FirstEmptyCellLocation();
			if (location != null)
			{
				Cell cell = puzzle.Rows[location.Value.Y].Cells[location.Value.X];
				int maxNumber = puzzle.SubPuzzles == null ? puzzle.Rows.Count : puzzle.SubPuzzles[0].Rows.Count;
				for (int i = 1; i <= maxNumber; i++)
				{
					cell.Number = i;
					if (Valid(puzzle, cell, location.Value) && Solve(puzzle, persist))
					{
						return true; // valid option found for cell
					}
				}
				cell.Number = 0;
				return false; // No valid option found for cell
			}
			else
			{
				return true; // No empty cell found
			}
		}

		private bool Valid(Puzzle puzzle, Cell newCell, (int X, int Y) location)
		{
			if (puzzle.SubPuzzles != null)
			{
				return ValidateSamurai(puzzle, newCell, location);
			}
			return
				RowValid(puzzle.Rows[location.Y], newCell) &&
				ColumnValid(puzzle.Columns[location.X], newCell) &&
				RegionValid(newCell.Region, newCell);
		}

		private bool ValidateSamurai(Puzzle puzzle, Cell newCell, (int X, int Y) location)
		{
			if (!RegionValid(newCell.Region, newCell))
			{
				return false;
			}
			Puzzle subPuzzle = null;
			(int X, int Y)? subLocation = null;
			if (location.X < puzzle.Columns.Count / 2 - 1)
			{
				if (location.Y < puzzle.Rows.Count / 2 - 1)
				{
					subPuzzle = puzzle.SubPuzzles[0];
					subLocation = location;
				}
				else if (location.Y > puzzle.Rows.Count / 2 + 1)
				{
					subPuzzle = puzzle.SubPuzzles[3];
					subLocation = (location.X, location.Y - 12);
				}
			}
			else if (location.X > puzzle.Columns.Count / 2 + 1)
			{
				if (location.Y < puzzle.Rows.Count / 2 - 1)
				{
					subPuzzle = puzzle.SubPuzzles[1];
					subLocation = (location.X - 12, location.Y);
				}
				else if (location.Y > puzzle.Rows.Count / 2 + 1)
				{
					subPuzzle = puzzle.SubPuzzles[4];
					subLocation = (location.X - 12, location.Y - 12);
				}
			}
			if (subPuzzle != null && subLocation != null)
			{
				if (!RowValid(subPuzzle.Rows[subLocation.Value.Y], newCell) ||
					!ColumnValid(subPuzzle.Columns[subLocation.Value.X], newCell))
				{
					return false;
				}
			}
			if ( 6 <= location.X && location.X < 15 && 6 <= location.Y && location.Y < 15)
			{
				subPuzzle = puzzle.SubPuzzles[2];
				subLocation = (location.X - 6, location.Y - 6);

				return
					RowValid(subPuzzle.Rows[subLocation.Value.Y], newCell) &&
					ColumnValid(subPuzzle.Columns[subLocation.Value.X], newCell);
			}
			return true;
		}

		private bool RowValid(Row row, Cell newCell)
		{
			return !row.Cells.Any(cell => cell.Number == newCell.Number && cell != newCell);
		}

		private bool ColumnValid(Column column, Cell newCell)
		{
			return !column.Cells.Any(cell => cell.Number == newCell.Number && cell != newCell);
		}

		private bool RegionValid(Region region, Cell newCell)
		{
			return !region.Cells.Any(cell => cell.Number == newCell.Number && cell != newCell);
		}
	}
}
