using sudoku.Game;

namespace sudoku.SolvingAlgorithm
{
	public interface ISolvingAlgorithm
	{
		bool Solve(Puzzle puzzle);
	}
}
