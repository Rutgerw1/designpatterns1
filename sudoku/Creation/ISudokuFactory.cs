using sudoku.Game;
using System.Drawing;

namespace sudoku.Creation
{
	public interface ISudokuFactory
	{
		Puzzle CreatePuzzle(string file);
	}
}
