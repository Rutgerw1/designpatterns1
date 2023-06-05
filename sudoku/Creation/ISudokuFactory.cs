using sudoku.Game;
using System.Drawing;

namespace sudoku.Creation
{
	interface ISudokuFactory
	{
		Puzzle CreatePuzzle(string file, Point offset = default);
	}
}
