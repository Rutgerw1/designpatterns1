using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Validation
{
	class Validator : IValidator
	{
		public List<(int X, int Y)> ValidateWhole(Puzzle puzzle)
		{
			List<(int X, int Y)> errorLocations = new List<(int X, int Y)>();
			for (int y = 0; y < puzzle.Rows.Count; y++)
			{
				for (int x = 0; x < puzzle.Columns.Count; x++)
				{
					Cell cell = puzzle.Rows[y].Cells[x];

					bool isValid = ValidateOne(puzzle, cell, (x, y));

					if (!isValid)
					{
						errorLocations.Add((x, y));
					}
				}
			}
			return errorLocations;
		}
		public bool ValidateOne(Puzzle puzzle, Cell newCell, (int X, int Y) location)
		{
			if (newCell.Number == 0)
			{
				newCell.OutOfBounds = false;
				return true;
			}
			if (newCell.Number > puzzle.Rows.Count)
			{
				newCell.OutOfBounds = true;
				return false;
			}
			newCell.OutOfBounds = false;
			if (puzzle.SubPuzzles != null)
			{
				return ValidateSamurai(puzzle, newCell, location);
			}

			bool validInRow = CellValid(puzzle.Rows[location.Y], newCell);
			bool validInColumn = CellValid(puzzle.Columns[location.X], newCell);
			bool validInRegion = CellValid(newCell.Region, newCell);

			return validInRow && validInColumn && validInRegion;
		}

		private bool ValidateSamurai(Puzzle puzzle, Cell newCell, (int X, int Y) location)
		{
			newCell.OutOfBounds = newCell.Number > 9;

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
			bool validInRow1 = true;
			bool validInColumn1 = true;
			if (subPuzzle != null && subLocation != null)
			{
				validInRow1 = CellValid(subPuzzle.Rows[subLocation.Value.Y], newCell);
				validInColumn1 = CellValid(subPuzzle.Columns[subLocation.Value.X], newCell);
			}
			bool validInRow2 = true;
			bool validInColumn2 = true;
			if (6 <= location.X && location.X < 15 && 6 <= location.Y && location.Y < 15)
			{
				subPuzzle = puzzle.SubPuzzles[2];
				subLocation = (location.X - 6, location.Y - 6);

				validInRow2 = CellValid(subPuzzle.Rows[subLocation.Value.Y], newCell);
				validInColumn2 = CellValid(subPuzzle.Columns[subLocation.Value.X], newCell);
			}
			bool validInRegion = CellValid(newCell.Region, newCell);

			return validInRow1 && validInColumn1 && validInRow2 && validInColumn2 && validInRegion;
		}

		private bool CellValid(Group group, Cell targetCell)
		{
			int conflicts = 0; 
			group.Cells.ForEach(cell => conflicts += ConflictsWith(cell, targetCell) ? 1 : 0);
			return conflicts == 0;
		}

		private bool ConflictsWith(Cell otherCell, Cell targetCell)
		{
			if (otherCell.Number == targetCell.Number && otherCell != targetCell)
			{
				targetCell.Conflicts.Add(otherCell);
				return true;
			} else
			{
				targetCell.Conflicts.Remove(otherCell);
				return false;
			}
		}
	}
}
