using sudoku.Game;
using sudoku.Validation;
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
		public bool Solve(Puzzle puzzle, IValidator validator)
		{
			(int X, int Y)? location = puzzle.FirstEmptyCellLocation();
			if (location != null)
			{
				Cell cell = puzzle.Rows[location.Value.Y].Cells[location.Value.X];
				for (int i = 1; i <= puzzle.MaxNumber; i++)
				{
					cell.Number = i;
					if (validator.ValidateOne(puzzle, cell, location.Value) && Solve(puzzle, validator))
					{
						return true; // valid option found for cell
					}
				}
				cell.Number = 0;
				cell.Conflicts.Clear();
				return false; // No valid option found for cell
			}
			else
			{
				return true; // No empty cell found
			}
		}
	}
}
