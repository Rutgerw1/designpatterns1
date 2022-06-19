using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Validation
{
	interface IValidator
	{
		List<(int X, int Y)> ValidateWhole(Puzzle puzzle);
		bool ValidateOne(Puzzle puzzle, Cell newCell, (int X, int Y) location);
	}
}
