using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
	interface ISudokuComponent
	{
		bool IsValid(int maxNumber);
		void ChangeValueAtPosition(int value, Point position);
		void ToggleNoteAtPosition(int value, Point position);
		Cell CellAtPosition(Point position);
		bool Contains(Point point);
	}
}
