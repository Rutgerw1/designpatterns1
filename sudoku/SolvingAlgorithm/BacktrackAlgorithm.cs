using sudoku.Game;

namespace sudoku.SolvingAlgorithm
{
	public class BacktrackAlgorithm : ISolvingAlgorithm
	{
		public bool Solve(Puzzle puzzle)
		{
			Cell cell = puzzle.FirstEmptyCell();
			if (cell != null)
			{
				for (int i = 1; i <= puzzle.MaxNumber; i++)
				{
					cell.ChangeValueAtPosition(i, cell.Position); // invoke method to deal with clearing of conflicts on other cells

					if (puzzle.IsValidIgnoreConflicts(cell.Position) && Solve(puzzle))
					{
						return true; // valid option found for cell
					}
				}
				cell.ChangeValueAtPosition(0, cell.Position);

				return false; // No valid option found for cell
			}
			else
			{
				return true; // No empty cell found
			}
		}
	}
}
