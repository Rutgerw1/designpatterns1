using sudoku.Game;

namespace sudoku.Creation
{
	public interface ISudokuFactory
	{
		Puzzle CreatePuzzle(string file);
	}
}
