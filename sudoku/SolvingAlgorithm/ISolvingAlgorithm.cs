using sudoku.Game;
using sudoku.View.States;

namespace sudoku.SolvingAlgorithm
{
	public interface ISolvingAlgorithm
	{
		bool Solve(Puzzle puzzle);
	}
}
